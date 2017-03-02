using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Lives {
	public class LivesPlayer : ModPlayer {
		public int Lives { get; private set; }
		public int Deaths { get; private set; }
		public byte OriginalDifficulty { get; private set; }
		public bool IsMortal { get; private set; }

		private bool IsInitialized = false;


		////////////////

		public override void Initialize() {
			if( !this.IsInitialized ) {
				this.IsInitialized = true;

				this.Lives = LivesMod.Config.Data.InitialLives;
				this.Deaths = 0;
				this.OriginalDifficulty = this.player.difficulty;
			}
		}

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (LivesPlayer)clone;
			
			myclone.IsMortal = this.IsMortal;
			myclone.Lives = this.Lives;
			myclone.Deaths = this.Deaths;
			myclone.OriginalDifficulty = this.OriginalDifficulty;
			myclone.IsInitialized = this.IsInitialized;
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI == this.player.whoAmI ) { // Current player
				if( !LivesMod.Config.Load() ) {
					LivesMod.Config.Save();
				}

				if( Main.netMode == 1 ) { // Client
					LivesNetProtocol.RequestSettingsWithClient( this.mod, player );
				} else {
					this.UpdateMortality();
				}
			}
		}

		////////////////

		public override void LoadLegacy( BinaryReader reader ) {
			this.Lives = reader.ReadInt32();
			this.IsMortal = reader.ReadBoolean();
			if( reader.PeekChar() != -1 ) {
				this.Deaths = reader.ReadInt32();
			}
			
			// Detect true 'Difficulty' based on available information
			if( this.Deaths > 0 && this.Lives == 0 && this.player.difficulty == 2 ) {
				this.OriginalDifficulty = 0;    // Set softcore
			}

			this.UpdateMortality();
		}

		public override void Load( TagCompound tags ) {
			try {
				this.IsMortal = tags.GetBool( "is_mortal" );
				if( this.IsMortal ) {
					this.Lives = tags.GetInt( "lives" );
					this.Deaths = tags.GetInt( "lives_lost" );
					this.OriginalDifficulty = tags.GetByte( "difficulty" );
				} else {    // Preserve initialized values, if invalid load data
					this.IsMortal = true;
					// If settings from previous versions exist, accommodate them
					if( this.Deaths > 0 && this.Lives == 0 && this.player.difficulty == 2 ) {
						this.OriginalDifficulty = 0;
					} else {
						this.OriginalDifficulty = this.player.difficulty;
					}
				}

				this.UpdateMortality();
			} catch( Exception e ) {
				ErrorLogger.Log( e.ToString() );
			}
		}

		public override TagCompound Save() {
			var tags = new TagCompound {
				{"is_mortal", this.IsMortal},
				{"lives", this.Lives},
				{"lives_lost", this.Deaths},
				{"difficulty", this.OriginalDifficulty}
			};
			return tags;
		}

		////////////////

		public override bool PreKill( double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource ) {
			this.Deaths++;
			this.UpdateMortality();
			
			if( this.IsMortal && this.Lives > 0 ) {
				this.Lives -= 1;
			}

			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}


		////////////////

		public bool AddLives( int lives ) {
			if( lives <= 0 ) {
				ErrorLogger.Log( "Invalid lives quantity." );
				return false;
			}
			if( (this.Lives + lives) > LivesMod.Config.Data.MaxLives ) {
				return false;
			}

			this.Lives += lives;
			this.UpdateMortality();

			return true;
		}


		////////////////

		public void UpdateMortality() {
			if( this.Lives > LivesMod.Config.Data.MaxLives ) {
				this.Lives = LivesMod.Config.Data.MaxLives;
			}

			if( this.player.difficulty != 2 ) { // Not hardcore
				if( this.IsMortal ) {
					if( this.Lives == 0 ) {
						this.player.difficulty = 2;  // Set hardcore

						if( Main.netMode == 1 ) {   // Client
							LivesNetProtocol.SignalDifficultyChangeFromClient( this.mod, this.player, 2 );
						}
					}
				}
			} else {
				if( this.Lives > 0 && this.OriginalDifficulty != 2 ) {
					this.player.difficulty = this.OriginalDifficulty;

					if( Main.netMode == 1 ) {   // Client
						LivesNetProtocol.SignalDifficultyChangeFromClient( this.mod, this.player, this.OriginalDifficulty );
					}
				}
			}
		}
	}
}
