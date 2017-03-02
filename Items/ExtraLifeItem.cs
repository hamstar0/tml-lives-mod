using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Lives.Items {
	public class ExtraLifeItem : ModItem {
		public override void SetDefaults() {
			this.item.name = "1-Up";
			this.item.maxStack = 99;
			this.item.consumable = true;
			this.item.width = 18;
			this.item.height = 18;
			this.item.useStyle = 4;
			this.item.useTime = 30;
			this.item.useAnimation = 30;
			this.item.UseSound = SoundID.Item4;
			this.item.toolTip = "Extra life! Except for hardcore folk.";
			this.item.value = 10000;
			this.item.rare = 4;
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
			var recipe1 = new ExtraLifeRecipe( this.mod, "Life Crystal", 1 );
			recipe1.SetResult( this );
			recipe1.AddRecipe();

			var recipe2 = new ExtraLifeRecipe( this.mod, "Life Fruit", 4 );
			recipe2.SetResult( this );
			recipe2.AddRecipe();
		}
	}



	class ExtraLifeRecipe : ModRecipe {
		public ExtraLifeRecipe( Mod mod, string base_ingredient, int base_quantity ) : base( mod ) {
			int coins = LivesMod.Config.Data.ExtraLifeGoldCoins;

			this.AddIngredient( base_ingredient, base_quantity );

			if( LivesMod.Config.Data.ExtraLifeVoodoo ) {
				this.AddIngredient( "Guide Voodoo Doll", 1 );
			}
			if( coins > 0 ) {
				this.AddIngredient( "Gold Coin", coins );
			}
		}

		public override bool RecipeAvailable() {
			return LivesMod.Config.Data.CraftableExtraLives;
		}
	}
}
