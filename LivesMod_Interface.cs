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
			if( !this.Config.Enabled ) { return; }

			Player player = Main.LocalPlayer;
			if( player.difficulty == 2 ) { return; }

			var myplayer = TmlHelpers.SafelyGetModPlayer<LivesPlayer>( player );

			if( this.Config.DrawLivesIcon || this.Config.DrawLivesText ) {
				if( this.Config.DrawLivesIcon ) {
					this.DrawLivesIcon( sb );
				}
				if( this.Config.DrawLivesText ) {
					this.DrawLivesText( sb, myplayer.Lives );
				}
			}
			
			if( this.Config.ContinuesLimit > 0 && (this.Config.DrawContinuesIcon || this.Config.DrawContinuesText) ) {
				if( this.Config.DrawContinuesIcon ) {
					this.DrawContinuesIcon( sb );
				}
				if( this.Config.DrawContinuesText ) {
					this.DrawContinuesText( sb, this.Config.ContinuesLimit - myplayer.ContinuesUsed );
				}
			}
		}


		////////////////

		public void DrawLivesIcon( SpriteBatch sb ) {
			int offsetX = this.Config.DrawLivesIconOffsetX; //48
			int offsetY = this.Config.DrawLivesIconOffsetY; //24

			int posX = offsetX < 0 ?
				Main.screenWidth + offsetX :
				offsetX;
			int posY = offsetY < 0 ?
				Main.screenHeight + offsetY :
				offsetY;

			PlayerHeadDrawHelpers.DrawPlayerHead( sb, Main.LocalPlayer, posX, posY, 1f, 1f );
		}

		public void DrawLivesText( SpriteBatch sb, int lives ) {
			int offsetX = this.Config.DrawLivesTextOffsetX; //38
			int offsetY = this.Config.DrawLivesTextOffsetY; //26

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
			int offsetX = this.Config.DrawContinuesIconOffsetX;
			int offsetY = this.Config.DrawContinuesIconOffsetY;

			int posX = offsetX < 0 ?
				Main.screenWidth + offsetX :
				offsetX;
			int posY = offsetY < 0 ?
				Main.screenHeight + offsetY :
				offsetY;
			
			sb.Draw( Main.itemTexture[ItemID.AnkhCharm], new Vector2(posX, posY), Color.White );
		}

		public void DrawContinuesText( SpriteBatch sb, int continues ) {
			int offsetX = this.Config.DrawContinuesTextOffsetX;
			int offsetY = this.Config.DrawContinuesTextOffsetY;

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
