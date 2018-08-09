using Lives.NetProtocol;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Lives {
	partial class LivesPlayer : ModPlayer {
		public int Lives { get; private set; }
		public int Deaths { get; private set; }
		public byte OriginalDifficulty { get; private set; }
		public bool IsImmortal { get; private set; }



		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
			var mymod = (LivesMod)this.mod;

			this.Lives = mymod.ConfigJson.Data.InitialLives;
			this.Deaths = 0;
			this.OriginalDifficulty = this.player.difficulty;
			this.IsImmortal = false;
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

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			var mymod = (LivesMod)this.mod;

			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
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
					ErrorLogger.Log( "Lives config " + LivesConfigData.ConfigVersion.ToString() + " created (ModPlayer.OnEnterWorld())." );
				}
			}

			if( Main.netMode == 0 ) {
				this.OnSingleConnect();
			}
			if( Main.netMode == 1 ) {
				this.OnClientConnect();
			}
		}

		private void OnSingleConnect() {
			this.UpdateMortality();
		}
		private void OnClientConnect() {
			ClientPacketHandlers.RequestSettingsWithClient( (LivesMod)this.mod, player );
		}
		private void OnServerConnect() {
			this.UpdateMortality();
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
	}
}
