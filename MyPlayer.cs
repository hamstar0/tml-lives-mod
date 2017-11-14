using Lives.NetProtocol;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Lives {
	class MyPlayer : ModPlayer {
		public int Lives { get; private set; }
		public int Deaths { get; private set; }
		public byte OriginalDifficulty { get; private set; }
		public bool IsImmortal { get; private set; }


		////////////////

		public override void Initialize() {
			var mymod = (LivesMod)this.mod;

			this.Lives = mymod.Config.Data.InitialLives;
			this.Deaths = 0;
			this.OriginalDifficulty = this.player.difficulty;
			this.IsImmortal = false;
		}

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (MyPlayer)clone;
			
			myclone.IsImmortal = this.IsImmortal;
			myclone.Lives = this.Lives;
			myclone.Deaths = this.Deaths;
			myclone.OriginalDifficulty = this.OriginalDifficulty;
		}


		public override void OnEnterWorld( Player player ) {
			var mymod = (LivesMod)this.mod;

			if( player.whoAmI == this.player.whoAmI ) { // Current player
				if( Main.netMode != 2 ) { // Not server
					if( !mymod.Config.LoadFile() ) {
						mymod.Config.SaveFile();
					}
				}

				if( Main.netMode == 1 ) { // Client
					ClientPacketHandlers.RequestSettingsWithClient( mymod, player );
				} else {
					this.UpdateMortality();
				}
			}
		}

		////////////////
		
		public override void Load( TagCompound tags ) {
			try {
				if( tags.ContainsKey("lives") ) {
					this.IsImmortal = tags.GetBool( "is_immortal" );
					this.Lives = tags.GetInt( "lives" );
					this.Deaths = tags.GetInt( "lives_lost" );
					this.OriginalDifficulty = tags.GetByte( "difficulty" );
				}
				
				this.UpdateMortality();
			} catch( Exception e ) {
				ErrorLogger.Log( e.ToString() );
			}
		}

		public override TagCompound Save() {
			var tags = new TagCompound {
				{"is_immortal", this.IsImmortal},
				{"lives", this.Lives},
				{"lives_lost", this.Deaths},
				{"difficulty", this.OriginalDifficulty}
			};
			return tags;
		}

		////////////////

		public override bool PreKill( double damage, int hit_direction, bool pvp, ref bool play_sound, ref bool gen_gore, ref PlayerDeathReason damage_source ) {
			var mymod = (LivesMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { base.PreKill( damage, hit_direction, pvp, ref play_sound, ref gen_gore, ref damage_source ); }

			this.Deaths++;
			if( !this.IsImmortal ) {
				this.Lives -= 1;
			}

			this.UpdateMortality();

			return base.PreKill( damage, hit_direction, pvp, ref play_sound, ref gen_gore, ref damage_source );
		}


		////////////////

		public bool AddLives( int lives ) {
			var mymod = (LivesMod)this.mod;

			if( (this.Lives + lives) > mymod.Config.Data.MaxLives ) {
				lives = mymod.Config.Data.MaxLives - this.Lives;
			} else if( (this.Lives + lives) < 0 ) {
				lives = -this.Lives;
			}

			this.Lives += lives;

			this.UpdateMortality();
			
			return true;
		}


		////////////////

		public void UpdateMortality() {
			var mymod = (LivesMod)this.mod;

			if( this.Lives > mymod.Config.Data.MaxLives ) {
				this.Lives = mymod.Config.Data.MaxLives;
			}
			
			if( this.player.difficulty != 2 ) { // Not hardcore
				if( !this.IsImmortal ) {
					if( this.Lives <= 0 ) {
						this.player.difficulty = 2;  // Set hardcore

						if( Main.netMode == 1 ) {   // Client
							ClientPacketHandlers.SignalDifficultyChangeFromClient( mymod, this.player, 2 );
						}
					}
				}
			} else {
				if( this.Lives > 0 && this.OriginalDifficulty != 2 ) {
					this.player.difficulty = this.OriginalDifficulty;

					if( Main.netMode == 1 ) {	// Client
						ClientPacketHandlers.SignalDifficultyChangeFromClient( mymod, this.player, this.OriginalDifficulty );
					}
				}
			}
		}
	}
}
