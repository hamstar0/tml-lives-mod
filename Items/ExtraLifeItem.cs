using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Lives.Items {
	class ExtraLifeItem : ModItem {
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "1-Up" );
			this.Tooltip.SetDefault( "Extra life (except for hardcore folk)" );
		}


		public override void SetDefaults() {
			this.item.maxStack = 99;
			this.item.consumable = true;
			this.item.width = 18;
			this.item.height = 18;
			this.item.useStyle = 4;
			this.item.useTime = 30;
			this.item.useAnimation = 30;
			this.item.UseSound = SoundID.Item4;
			this.item.value = 10000;
			this.item.rare = 4;
		}

		public override void Update( ref float gravity, ref float maxFallSpeed ) {
			if( Main.rand.Next( 6 ) == 0 ) {
				int who = Dust.NewDust( this.item.position, this.item.width, this.item.height, 55, 0f, 0f, 200, Color.Gold, 1f );
				Main.dust[who].velocity *= 0.3f;
				Main.dust[who].scale *= 0.5f;
			}
		}

		public override bool UseItem( Player player ) {
			if( player.itemAnimation > 0 && player.itemTime == 0 ) {
				player.itemTime = item.useTime;
				return true;
			}
			return base.UseItem( player );
		}

		public override bool ConsumeItem( Player player ) {
			LivesPlayer modplayer = ModContent.GetInstance<LivesPlayer>();
			return modplayer.AddLives( 1 );
		}

		public override void AddRecipes() {
			var recipe1 = new ExtraLifeRecipe( this, ItemID.LifeCrystal, 1 );
			var recipe2 = new ExtraLifeRecipe( this, ItemID.LifeFruit, 4 );

			recipe1.AddRecipe();
			recipe2.AddRecipe();
		}
	}




	class ExtraLifeRecipe : ModRecipe {
		public ExtraLifeRecipe( ExtraLifeItem myitem, int itemId, int baseQuantity ) : base( LivesMod.Instance ) {
			var config = LivesConfig.Instance;
			int coins = config.ExtraLifeGoldCoins;

			this.AddIngredient( itemId, baseQuantity );

			if( config.ExtraLifeVoodoo ) {
				this.AddIngredient( ItemID.GuideVoodooDoll, 1 );
			}
			if( coins > 0 ) {
				this.AddIngredient( ItemID.GoldCoin, coins );
			}
			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var config = LivesConfig.Instance;
			return config.Enabled && config.CraftableExtraLives;
		}
	}
}
