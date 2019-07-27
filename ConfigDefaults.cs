using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace Lives {
	public class LivesConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[DefaultValue( true )]
		public bool Enabled = true;


		////

		[DefaultValue( 3 )]
		public int InitialLives = 3;

		[DefaultValue( 99 )]
		public int MaxLives = 99;


		[DefaultValue( true )]
		public bool CraftableExtraLives = true;

		[DefaultValue( 15 )]
		public int ExtraLifeGoldCoins = 15;

		[DefaultValue( true )]
		public bool ExtraLifeVoodoo = true;


		////

		[DefaultValue( 0 )]
		public int ContinuesLimit = 0;  // Less than 0: Unlimited, Greater than 0: Finite, Equal to 0: Game over!

		[DefaultValue( true )]
		public bool ContinueDeathDropItems = true;

		[DefaultValue( 100 )]
		public int ContinueDeathMaxHpToll = 100;

		[DefaultValue( 100 )]
		public int ContinueDeathMaxHpMinimum = 100; // No max hp? Game over!

		[DefaultValue( 10 )]
		public int ContinueDeathRewardsPPToll = 10;

		[DefaultValue( 0 )]
		public int ContinueDeathRewardsPPMinimum = 0;

		[DefaultValue( 10 )]
		public int ContinueDeathMaxStaminaToll = 10;

		[DefaultValue( 50 )]
		public int ContinueDeathMaxStaminaMinimum = 50;


		////

		[DefaultValue( true )]
		public bool DrawLivesIcon = true;

		[DefaultValue( true )]
		public bool DrawLivesText = true;


		[DefaultValue( -48 )]
		public int DrawLivesIconOffsetX = -48;

		[DefaultValue( -24 )]
		public int DrawLivesIconOffsetY = -24;

		[DefaultValue( -38 )]
		public int DrawLivesTextOffsetX = -38;

		[DefaultValue( -26 )]
		public int DrawLivesTextOffsetY = -26;


		[DefaultValue( true )]
		public bool DrawContinuesIcon = true;

		[DefaultValue( true )]
		public bool DrawContinuesText = true;


		[DefaultValue( -56 )]
		public int DrawContinuesIconOffsetX = -56;

		[DefaultValue( -68 )]
		public int DrawContinuesIconOffsetY = -68;

		[DefaultValue( -38 )]
		public int DrawContinuesTextOffsetX = -38;

		[DefaultValue( -59 )]
		public int DrawContinuesTextOffsetY = -59;
	}
}
