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

		public bool AddLives( int lives ) {
			var mymod = (LivesMod)this.mod;

			if( (this.Lives + lives) > mymod.ConfigJson.Data.MaxLives ) {
				lives = mymod.ConfigJson.Data.MaxLives - this.Lives;
			} else if( (this.Lives + lives) < 0 ) {
				lives = -this.Lives;
			}

			this.Lives += lives;

			this.UpdateMortality();
			
			return true;
		}


		////////////////

		public void UpdateMortality() {
			var mymod = (LivesMod)this.mod;

			if( this.Lives > mymod.ConfigJson.Data.MaxLives ) {
				this.Lives = mymod.ConfigJson.Data.MaxLives;
			}
			
			if( this.player.difficulty != 2 ) { // Not hardcore
				if( !this.IsImmortal ) {
					if( this.Lives <= 0 ) {
						if( this.IsContinue() ) {
							this.ApplyContinue();
						} else {
							this.ApplyDeathFinal();
						}
					}
				}
			} else {
				if( this.Lives > 0 ) {
					this.ApplyDeathNonFinal();
				}
			}
		}
	}
}
