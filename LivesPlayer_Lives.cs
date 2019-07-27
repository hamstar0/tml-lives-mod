using Lives.NetProtocols;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesPlayer : ModPlayer {
		public bool AddLives( int lives ) {
			var mymod = (LivesMod)this.mod;

			if( ( this.Lives + lives ) > mymod.Config.MaxLives ) {
				lives = mymod.Config.MaxLives - this.Lives;
			} else if( ( this.Lives + lives ) < 0 ) {
				lives = -this.Lives;
			}

			this.Lives += lives;

			this.UpdateMortality();

			return true;
		}


		////////////////

		public void ApplyDeathNonFinal() {
			if( this.OriginalDifficulty == 2 ) {	// Hardcore? Guess we're just dead, then.
				return;
			}

			this.player.difficulty = this.OriginalDifficulty;

			if( Main.netMode == 1 && this.player.whoAmI == Main.myPlayer ) {
				DifficultyChangeProtocol.SendToServer( this.OriginalDifficulty );
			}
		}

		public void ApplyDeathFinal() {
			this.player.difficulty = 2;  // Set hardcore

			if( Main.netMode == 1 && this.player.whoAmI == Main.myPlayer ) {
				DifficultyChangeProtocol.SendToServer( 2 );
			}
		}
	}
}
