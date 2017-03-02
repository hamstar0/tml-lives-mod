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
		public static readonly Version ConfigVersion = new Version( 1, 2, 0 );
		public static JsonConfig<ConfigurationData> Config { get; private set; }


		public LivesMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			string filename = "Lives "+LivesMod.ConfigVersion.ToString()+".json";
			LivesMod.Config = new JsonConfig<ConfigurationData>(filename, new ConfigurationData());
		}

		public override void Load() {
			if( !LivesMod.Config.Load() ) {
				LivesMod.Config.Save();
			} else {
				Version vers_since = LivesMod.Config.Data.VersionSinceUpdate != "" ?
					new Version( LivesMod.Config.Data.VersionSinceUpdate ) :
					new Version();

				if( vers_since < LivesMod.ConfigVersion ) {
					ErrorLogger.Log( "Lives config updated to " + LivesMod.ConfigVersion.ToString() );
					LivesMod.Config.Data.VersionSinceUpdate = LivesMod.ConfigVersion.ToString();
					LivesMod.Config.Save();
				}
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
