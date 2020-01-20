using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesPlayer : ModPlayer {
		public void DeathHappened( bool pvp ) {
			var mymod = (LivesMod)this.mod;

			this.Deaths++;

			if( !this.IsImmortal ) {
				this.Lives -= 1;
			}

			this.UpdateMortality();
		}


		////////////////

		public void UpdateMortality() {
			var config = LivesConfig.Instance;

			if( this.Lives > config.MaxLives ) {
				this.Lives = config.MaxLives;
			}
			
//Main.NewText("difficulty: "+this.player.difficulty+", immortal? "+this.IsImmortal+", lives: "+this.Lives+", continues? "+this.IsContinue());
			if( this.player.difficulty != 2 ) { // Not hardcore
				if( !this.IsImmortal ) {
					if( this.Lives <= 0 ) {
						if( this.IsContinue() ) {
//DebugHelpers.Print("LivesContinues", "", 20);
							this.Lives = config.InitialLives;

							string gameOverReason;
							if( !this.ApplyContinue( out gameOverReason ) ) {
								Main.NewText( gameOverReason+" Game over!", Color.Red );
								this.ApplyDeathFinal();
							}
						} else {
//DebugHelpers.Print("LivesFinal", "", 20);
							Main.NewText( "No lives left. Game over!", Color.Red );
							this.ApplyDeathFinal();
						}
					}
				}
			} else {
				if( this.Lives > 0 ) {
//DebugHelpers.Print("LivesNonFinal", "", 20);
					this.ApplyDeathNonFinal();
				}
			}
		}
	}
}
