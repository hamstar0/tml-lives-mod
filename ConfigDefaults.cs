using HamstarHelpers.Components.Config;
using System;


namespace Lives {
	public class LivesConfigData : ConfigurationDataBase {
		public static readonly string ConfigFileName = "Lives Config.json";



		////////////////

		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		////

		public int InitialLives = 3;
		public int MaxLives = 99;

		public bool CraftableExtraLives = true;
		public int ExtraLifeGoldCoins = 15;
		public bool ExtraLifeVoodoo = true;

		////

		public int ContinuesLimit = -1;	// Less than 0: Unlimited, Greater than 0: Finite, Equal to 0: Game over!
		public bool ContinueDeathDropItems = true;
		public int ContinueDeathMaxHpToll = 100;
		public int ContinueDeathMaxHpMinimum = 0;	// No max hp? Game over!
		public int ContinueDeathRewardsPPToll = 25;
		public int ContinueDeathMaxStaminaToll = 10;
		public int ContinueDeathMaxStaminaMinimum = 50;

		////

		public bool DrawLivesIcon = true;
		public bool DrawLivesText = true;

		public int DrawLivesIconOffsetX = -48;
		public int DrawLivesIconOffsetY = -24;
		public int DrawLivesTextOffsetX = -38;
		public int DrawLivesTextOffsetY = -26;

		public bool DrawContinuesIcon = false;
		public bool DrawContinuesText = false;

		public int DrawContinuesIconOffsetX = -48;
		public int DrawContinuesIconOffsetY = -12;
		public int DrawContinuesTextOffsetX = -38;
		public int DrawContinuesTextOffsetY = -14;


		
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
