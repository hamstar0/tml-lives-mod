using Lives.NetProtocol;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesPlayer : ModPlayer {
		public bool AddLives( int lives ) {
			var mymod = (LivesMod)this.mod;

			if( ( this.Lives + lives ) > mymod.ConfigJson.Data.MaxLives ) {
				lives = mymod.ConfigJson.Data.MaxLives - this.Lives;
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

			if( Main.netMode == 1 ) {
				ClientPacketHandlers.SignalDifficultyChangeFromClient( this.player, this.OriginalDifficulty );
			}
		}

		public void ApplyDeathFinal() {
			this.player.difficulty = 2;  // Set hardcore

			if( Main.netMode == 1 ) {
				ClientPacketHandlers.SignalDifficultyChangeFromClient( this.player, 2 );
			}
		}
	}
}
