using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Lives {
	partial class LivesPlayer : ModPlayer {
		public int Lives { get; private set; }

		public int Deaths { get; private set; }
		public int ContinuesUsed { get; private set; }

		public byte OriginalDifficulty { get; private set; }
		public bool IsImmortal { get; private set; }

		
		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void Initialize() {
			this.OriginalDifficulty = this.player.difficulty;
			this.ResetLivesToDefault();
		}

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (LivesPlayer)clone;
			
			myclone.IsImmortal = this.IsImmortal;
			myclone.Lives = this.Lives;
			myclone.Deaths = this.Deaths;
			myclone.OriginalDifficulty = this.OriginalDifficulty;
		}


		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			var mymod = (LivesMod)this.mod;

			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					this.OnServerConnect();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (LivesMod)this.mod;

			if( Main.netMode == 0 ) {
				if( !mymod.ConfigJson.LoadFile() ) {
					mymod.ConfigJson.SaveFile();
					LogHelpers.Alert( "Lives config " + mymod.Version.ToString() + " created." );
				}
			}

			if( Main.netMode == 0 ) {
				this.OnSingleConnect();
			}
			if( Main.netMode == 1 ) {
				this.OnCurrentClientConnect();
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
				if( tags.ContainsKey("continues") ) {
					this.ContinuesUsed = tags.GetInt( "continues" );
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
				{"difficulty", this.OriginalDifficulty},
				{"continues", this.ContinuesUsed}
			};
			return tags;
		}


		////////////////

		public override bool PreKill( double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource ) {
			var mymod = (LivesMod)this.mod;
			if( !mymod.ConfigJson.Data.Enabled ) {
				base.PreKill( damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource );
			}

			this.DeathHappened( pvp );

			return base.PreKill( damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource );
		}


		////////////////

		public void ResetLivesToDefault() {
			var mymod = (LivesMod)this.mod;

			this.Lives = mymod.ConfigJson.Data.InitialLives;
			this.Deaths = 0;
			this.IsImmortal = false;
		}
	}
}
