using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.Utilities.Config;
using Lives.NetProtocol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	public class LivesMod : Mod {
		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-lives-mod"; } }

		public static string ConfigRelativeFilePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + LivesConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception("Cannot reload configs outside of single player.");
			}
			if( LivesMod.Instance != null ) {
				LivesMod.Instance.Config.LoadFile();
			}
		}

		public static LivesMod Instance { get; private set; }


		////////////////

		public JsonConfig<LivesConfigData> Config { get; private set; }


		public LivesMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.Config = new JsonConfig<LivesConfigData>( LivesConfigData.ConfigFileName, ConfigurationDataBase.RelativePath, new LivesConfigData() );
		}

		public override void Load() {
			LivesMod.Instance = this;

			var old_config = new JsonConfig<LivesConfigData>( "Lives 1.2.0.json", "", new LivesConfigData() );
			// Update old config to new location
			if( old_config.LoadFile() ) {
				old_config.DestroyFile();
				old_config.SetFilePath( this.Config.FileName, ConfigurationDataBase.RelativePath );
				this.Config = old_config;
			}
			
			if( !this.Config.LoadFile() ) {
				this.Config.SaveFile();
			}

			if( this.Config.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Lives config updated to " + LivesConfigData.ConfigVersion.ToString() );
				this.Config.SaveFile();
			}
		}

		public override void Unload() {
			LivesMod.Instance = null;
		}



		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			if( Main.netMode == 1 ) {   // Client
				ClientPacketHandlers.HandlePacket( this, reader );
			} else if( Main.netMode == 2 ) {    // Server
				ServerPacketHandlers.HandlePacket( this, reader, player_who );
			}
		}


		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			if( !this.Config.Data.Enabled ) { return; }

			Player player = Main.player[Main.myPlayer];
			if( player.difficulty == 2 ) { return; }
			
			var modplayer = player.GetModPlayer<LivesPlayer>(this);
			int lives = modplayer.Lives;
			Vector2 pos = new Vector2(Main.screenWidth - 38, Main.screenHeight - 26);

			PlayerHeadDisplayHelpers.DrawPlayerHead(sb, Main.player[Main.myPlayer], Main.screenWidth - 48, Main.screenHeight - 24, 1f, 1f);
			sb.DrawString( Main.fontMouseText, " x" + lives, pos, Color.White );
		}
	}
}
