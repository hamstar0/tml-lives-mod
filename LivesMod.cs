using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Utils;
using Utils.JsonConfig;


namespace Lives {
	public class ConfigurationData {
		public string VersionSinceUpdate = "";
		public int InitialLives = 3;
		public int MaxLives = 99;
		public bool CraftableExtraLives = true;
		public int ExtraLifeGoldCoins = 15;
		public bool ExtraLifeVoodoo = true;
	}


	public class LivesMod : Mod {
		public static readonly Version ConfigVersion = new Version( 1, 5, 1 );
		public JsonConfig<ConfigurationData> Config { get; private set; }


		public LivesMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			string filename = "Lives Config.json";
			this.Config = new JsonConfig<ConfigurationData>( filename, "Mod Configs", new ConfigurationData() );
		}

		public override void Load() {
			var old_config = new JsonConfig<ConfigurationData>( "Lives 1.2.0.json", "", new ConfigurationData() );
			// Update old config to new location
			if( old_config.LoadFile() ) {
				old_config.DestroyFile();
				old_config.SetFilePath( this.Config.FileName, "Mod Configs" );
				this.Config = old_config;
			} else if( !this.Config.LoadFile() ) {
				this.Config.SaveFile();
			}
			
			Version vers_since = this.Config.Data.VersionSinceUpdate != "" ?
				new Version( this.Config.Data.VersionSinceUpdate ) :
				new Version();

			if( vers_since < LivesMod.ConfigVersion ) {
				ErrorLogger.Log( "Lives config updated to " + LivesMod.ConfigVersion.ToString() );

				this.Config.Data.VersionSinceUpdate = LivesMod.ConfigVersion.ToString();
				this.Config.SaveFile();
			}
		}



		////////////////

		public override void HandlePacket( BinaryReader reader, int whoAmI ) {
			LivesNetProtocol.RoutePacket( this, reader );
		}

		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			Player player = Main.player[Main.myPlayer];
			if( player.difficulty == 2 ) { return; }
			
			LivesPlayer info = player.GetModPlayer<LivesPlayer>(this);
			int lives = info.Lives;
			Vector2 pos = new Vector2(Main.screenWidth - 38, Main.screenHeight - 26);

			PlayerHead.DrawPlayerHead(sb, Main.player[Main.myPlayer], Main.screenWidth - 48, Main.screenHeight - 24, 1f, 1f);
			sb.DrawString(Main.fontMouseText, " x" + lives, pos, Color.White);

			DebugHelper.PrintToBatch( sb );
		}
	}
}
