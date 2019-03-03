using HamstarHelpers.Components.Config;
using System;


namespace Lives {
	public class LivesConfigData : ConfigurationDataBase {
		public static readonly string ConfigFileName = "Lives Config.json";


		////////////////

		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public int InitialLives = 3;
		public int MaxLives = 99;

		public bool CraftableExtraLives = true;
		public int ExtraLifeGoldCoins = 15;
		public bool ExtraLifeVoodoo = true;



		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new LivesConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= LivesMod.Instance.Version ) {
				return false;
			}

			this.VersionSinceUpdate = LivesMod.Instance.Version.ToString();

			return true;
		}
	}
}
