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

		public bool DrawLivesIcon = true;
		public bool DrawLivesText = true;

		public int LivesIconOffsetX = 48;
		public int LivesIconOffsetY = 24;
		public int LivesTextOffsetX = 38;
		public int LivesTextOffsetY = 26;



		////////////////

		public bool UpdateToLatestVersion() {
			var newConfig = new LivesConfigData();
			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( versSince >= LivesMod.Instance.Version ) {
				return false;
			}

			this.VersionSinceUpdate = LivesMod.Instance.Version.ToString();

			return true;
		}
	}
}
