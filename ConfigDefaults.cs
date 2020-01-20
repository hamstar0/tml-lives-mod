using System;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Lives {
	public class LivesConfig : ModConfig {
		public static LivesConfig Instance => ModContent.GetInstance<LivesConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[DefaultValue( true )]
		public bool Enabled { get; set; } = true;


		////

		[Range( 1, 999 )]
		[DefaultValue( 3 )]
		public int InitialLives { get; set; } = 3;

		[Range( 1, 999 )]
		[DefaultValue( 99 )]
		public int MaxLives { get; set; } = 99;


		[DefaultValue( true )]
		public bool CraftableExtraLives { get; set; } = true;

		[Range( 0, 999 )]
		[DefaultValue( 15 )]
		public int ExtraLifeGoldCoins { get; set; } = 15;

		[DefaultValue( true )]
		public bool ExtraLifeVoodoo { get; set; } = true;


		////

		[Range( -1, 999 )]
		[DefaultValue( 0 )]
		public int ContinuesLimit { get; set; } = 0;  // Less than 0: Unlimited, Greater than 0: Finite, Equal to 0: Game over!

		[DefaultValue( true )]
		public bool ContinueDeathDropItems { get; set; } = true;

		[DefaultValue( 100 )]
		public int ContinueDeathMaxHpToll { get; set; } = 100;

		[DefaultValue( 100 )]
		public int ContinueDeathMaxHpMinimum { get; set; } = 100; // No max hp? Game over!

		[DefaultValue( 10 )]
		public int ContinueDeathRewardsPPToll { get; set; } = 10;

		[DefaultValue( 0 )]
		public int ContinueDeathRewardsPPMinimum { get; set; } = 0;

		[DefaultValue( 10 )]
		public int ContinueDeathMaxStaminaToll { get; set; } = 10;

		[DefaultValue( 50 )]
		public int ContinueDeathMaxStaminaMinimum { get; set; } = 50;


		////

		[DefaultValue( true )]
		public bool DrawLivesIcon { get; set; } = true;

		[DefaultValue( true )]
		public bool DrawLivesText { get; set; } = true;


		[Range( -2048, 2048 )]
		[DefaultValue( -48 )]
		public int DrawLivesIconOffsetX { get; set; } = -48;

		[Range( -1024, 1024 )]
		[DefaultValue( -24 )]
		public int DrawLivesIconOffsetY { get; set; } = -24;

		[Range( -2048, 2048 )]
		[DefaultValue( -38 )]
		public int DrawLivesTextOffsetX { get; set; } = -38;

		[Range( -1024, 1024 )]
		[DefaultValue( -26 )]
		public int DrawLivesTextOffsetY { get; set; } = -26;


		[DefaultValue( true )]
		public bool DrawContinuesIcon { get; set; } = true;

		[DefaultValue( true )]
		public bool DrawContinuesText { get; set; } = true;


		[Range( -2048, 2048 )]
		[DefaultValue( -56 )]
		public int DrawContinuesIconOffsetX { get; set; } = -56;

		[Range( -1024, 1024 )]
		[DefaultValue( -68 )]
		public int DrawContinuesIconOffsetY { get; set; } = -68;

		[Range( -2048, 2048 )]
		[DefaultValue( -38 )]
		public int DrawContinuesTextOffsetX { get; set; } = -38;

		[Range( -1024, 1024 )]
		[DefaultValue( -59 )]
		public int DrawContinuesTextOffsetY { get; set; } = -59;
	}
}
