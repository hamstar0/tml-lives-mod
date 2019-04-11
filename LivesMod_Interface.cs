using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesMod : Mod {
		public override void PostDrawInterface( SpriteBatch sb ) {
			if( !this.Config.Enabled ) { return; }
			if( !this.Config.DrawLivesIcon && !this.Config.DrawLivesText ) { return; }

			Player player = Main.player[Main.myPlayer];
			if( player.difficulty == 2 ) { return; }
			
			var myplayer = player.GetModPlayer<LivesPlayer>();
			int lives = myplayer.Lives;
			
			if( this.Config.DrawLivesIcon ) {
				int offsetX = this.Config.LivesIconOffsetX; //48
				int offsetY = this.Config.LivesIconOffsetY; //24

				int posX = offsetX < 0 ?
					Main.screenWidth + offsetX :
					offsetX;
				int posY = offsetY < 0 ?
					Main.screenHeight + offsetY :
					offsetY;

				PlayerHeadDisplayHelpers.DrawPlayerHead( sb, Main.player[Main.myPlayer], posX, posY, 1f, 1f );
			}

			if( this.Config.DrawLivesText ) {
				int offsetX = this.Config.LivesTextOffsetX; //38
				int offsetY = this.Config.LivesTextOffsetY; //26

				int posX = offsetX < 0 ?
					Main.screenWidth + offsetX :
					offsetX;
				int posY = offsetY < 0 ?
					Main.screenHeight + offsetY :
					offsetY;
				Vector2 pos = new Vector2( posX, posY );

				sb.DrawString( Main.fontMouseText, " x" + lives, pos, Color.White );
			}
		}
	}
}
