using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesMod : Mod {
		public override void PostDrawInterface( SpriteBatch sb ) {
			var config = LivesConfig.Instance;
			if( !config.Enabled ) { return; }

			Player player = Main.LocalPlayer;
			if( player.difficulty == 2 ) { return; }

			var myplayer = TmlHelpers.SafelyGetModPlayer<LivesPlayer>( player );

			if( config.DrawLivesIcon || config.DrawLivesText ) {
				if( config.DrawLivesIcon ) {
					this.DrawLivesIcon( sb );
				}
				if( config.DrawLivesText ) {
					this.DrawLivesText( sb, myplayer.Lives );
				}
			}
			
			if( config.ContinuesLimit > 0 && ( config.DrawContinuesIcon || config.DrawContinuesText) ) {
				if( config.DrawContinuesIcon ) {
					this.DrawContinuesIcon( sb );
				}
				if( config.DrawContinuesText ) {
					this.DrawContinuesText( sb, config.ContinuesLimit - myplayer.ContinuesUsed );
				}
			}
		}


		////////////////

		public void DrawLivesIcon( SpriteBatch sb ) {
			var config = LivesConfig.Instance;
			int offsetX = config.DrawLivesIconOffsetX; //48
			int offsetY = config.DrawLivesIconOffsetY; //24

			int posX = offsetX < 0 ?
				Main.screenWidth + offsetX :
				offsetX;
			int posY = offsetY < 0 ?
				Main.screenHeight + offsetY :
				offsetY;

			PlayerHeadDrawHelpers.DrawPlayerHead( sb, Main.LocalPlayer, posX, posY, 1f, 1f );
		}

		public void DrawLivesText( SpriteBatch sb, int lives ) {
			var config = LivesConfig.Instance;
			int offsetX = config.DrawLivesTextOffsetX; //38
			int offsetY = config.DrawLivesTextOffsetY; //26

			int posX = offsetX < 0 ?
				Main.screenWidth + offsetX :
				offsetX;
			int posY = offsetY < 0 ?
				Main.screenHeight + offsetY :
				offsetY;
			var pos = new Vector2( posX, posY );

			sb.DrawString( Main.fontMouseText, " x" + lives, pos, Color.White );
		}

		////

		public void DrawContinuesIcon( SpriteBatch sb ) {
			var config = LivesConfig.Instance;
			int offsetX = config.DrawContinuesIconOffsetX;
			int offsetY = config.DrawContinuesIconOffsetY;

			int posX = offsetX < 0 ?
				Main.screenWidth + offsetX :
				offsetX;
			int posY = offsetY < 0 ?
				Main.screenHeight + offsetY :
				offsetY;
			
			sb.Draw( Main.itemTexture[ItemID.AnkhCharm], new Vector2(posX, posY), Color.White );
		}

		public void DrawContinuesText( SpriteBatch sb, int continues ) {
			var config = LivesConfig.Instance;
			int offsetX = config.DrawContinuesTextOffsetX;
			int offsetY = config.DrawContinuesTextOffsetY;

			int posX = offsetX < 0 ?
				Main.screenWidth + offsetX :
				offsetX;
			int posY = offsetY < 0 ?
				Main.screenHeight + offsetY :
				offsetY;
			var pos = new Vector2( posX, posY );

			sb.DrawString( Main.fontMouseText, " x" + continues, pos, Color.White );
		}
	}
}
