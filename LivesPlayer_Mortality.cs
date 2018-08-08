using Lives.NetProtocol;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesPlayer : ModPlayer {
		public override bool PreKill( double damage, int hit_direction, bool pvp, ref bool play_sound, ref bool gen_gore, ref PlayerDeathReason damage_source ) {
			var mymod = (LivesMod)this.mod;
			if( !mymod.ConfigJson.Data.Enabled ) { base.PreKill( damage, hit_direction, pvp, ref play_sound, ref gen_gore, ref damage_source ); }

			this.Deaths++;
			if( !this.IsImmortal ) {
				this.Lives -= 1;
			}

			this.UpdateMortality();

			return base.PreKill( damage, hit_direction, pvp, ref play_sound, ref gen_gore, ref damage_source );
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
						this.player.difficulty = 2;  // Set hardcore

						if( Main.netMode == 1 ) {   // Client
							ClientPacketHandlers.SignalDifficultyChangeFromClient( mymod, this.player, 2 );
						}
					}
				}
			} else {
				if( this.Lives > 0 && this.OriginalDifficulty != 2 ) {
					this.player.difficulty = this.OriginalDifficulty;

					if( Main.netMode == 1 ) {	// Client
						ClientPacketHandlers.SignalDifficultyChangeFromClient( mymod, this.player, this.OriginalDifficulty );
					}
				}
			}
		}
	}
}
