using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using Lives.NetProtocol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesMod : Mod {
		public static LivesMod Instance { get; private set; }



		////////////////

		public JsonConfig<LivesConfigData> ConfigJson { get; private set; }
		public LivesConfigData Config => this.ConfigJson.Data;



		////////////////

		public LivesMod() {
			this.ConfigJson = new JsonConfig<LivesConfigData>( LivesConfigData.ConfigFileName, ConfigurationDataBase.RelativePath, new LivesConfigData() );
		}

		public override void Load() {
			string depErr = TmlHelpers.ReportBadDependencyMods( this );
			if( depErr != null ) { throw new HamstarException( depErr ); }

			LivesMod.Instance = this;

			this.LoadConfigs();
		}
		
		private void LoadConfigs() {
			var oldConfig = new JsonConfig<LivesConfigData>( "Lives 1.2.0.json", "", new LivesConfigData() );
			// Update old config to new location
			if( oldConfig.LoadFile() ) {
				oldConfig.DestroyFile();
				oldConfig.SetFilePath( this.ConfigJson.FileName, ConfigurationDataBase.RelativePath );
				this.ConfigJson = oldConfig;
			}
			
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.ConfigJson.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Lives config updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			LivesMod.Instance = null;
		}



		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( LivesAPI ), args );
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int playerWho ) {
			if( Main.netMode == 1 ) {   // Client
				ClientPacketHandlers.HandlePacket( reader );
			} else if( Main.netMode == 2 ) {    // Server
				ServerPacketHandlers.HandlePacket( reader, playerWho );
			}
		}


		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			if( !this.ConfigJson.Data.Enabled ) { return; }

			Player player = Main.player[Main.myPlayer];
			if( player.difficulty == 2 ) { return; }
			
			var myplayer = player.GetModPlayer<LivesPlayer>();
			int lives = myplayer.Lives;
			Vector2 pos = new Vector2(Main.screenWidth - 38, Main.screenHeight - 26);

			PlayerHeadDisplayHelpers.DrawPlayerHead(sb, Main.player[Main.myPlayer], Main.screenWidth - 48, Main.screenHeight - 24, 1f, 1f);
			sb.DrawString( Main.fontMouseText, " x" + lives, pos, Color.White );
		}
	}
}
