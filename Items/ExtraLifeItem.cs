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

		public override void Update( ref float gravity, ref float max_fall_speed ) {
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
			LivesPlayer modplayer = player.GetModPlayer<LivesPlayer>(this.mod);
			return modplayer.AddLives( 1 );
		}

		public override void AddRecipes() {
			var mymod = (LivesMod)this.mod;
			var recipe1 = new ExtraLifeRecipe( mymod, this, ItemID.LifeCrystal, 1 );
			var recipe2 = new ExtraLifeRecipe( mymod, this, ItemID.LifeFruit, 4 );

			recipe1.AddRecipe();
			recipe2.AddRecipe();
		}
	}



	class ExtraLifeRecipe : ModRecipe {
		public ExtraLifeRecipe( LivesMod mymod, ExtraLifeItem myitem, int item_id, int base_quantity ) : base( mymod ) {
			int coins = mymod.ConfigJson.Data.ExtraLifeGoldCoins;

			this.AddIngredient( item_id, base_quantity );

			if( mymod.ConfigJson.Data.ExtraLifeVoodoo ) {
				this.AddIngredient( ItemID.GuideVoodooDoll, 1 );
			}
			if( coins > 0 ) {
				this.AddIngredient( ItemID.GoldCoin, coins );
			}
			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (LivesMod)this.mod;
			return mymod.ConfigJson.Data.Enabled && mymod.ConfigJson.Data.CraftableExtraLives;
		}
	}
}
