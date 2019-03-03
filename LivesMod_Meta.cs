using HamstarHelpers.Components.Config;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-lives-mod";

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + LivesConfigData.ConfigFileName; }
		}

		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( LivesMod.Instance != null ) {
				if( !LivesMod.Instance.ConfigJson.LoadFile() ) {
					LivesMod.Instance.ConfigJson.SaveFile();
				}
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var newConfig = new LivesConfigData();
			//new_config.SetDefaults();

			LivesMod.Instance.ConfigJson.SetData( newConfig );
			LivesMod.Instance.ConfigJson.SaveFile();
		}
	}
}
