using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Timers;
using Lives.NetProtocol;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesPlayer : ModPlayer {
		public bool IsContinue() {
			var mymod = (LivesMod)this.mod;
			LivesConfigData config = mymod.Config;

			if( config.ContinuesLimit > 0 ) {
				if( this.Continues >= config.ContinuesLimit ) {
					return false;
				}
			} else {
				return false;
			}

			if( config.ContinueDeathMaxHpToll > 0 ) {
				int maxHpMin = config.ContinueDeathMaxHpMinimum;
				int maxHp = this.player.statLifeMax;

				if( maxHpMin <= 0 ) {
					return maxHp > maxHpMin;
				}
			}

			return true;
		}

		public void ApplyContinue() {
			var mymod = (LivesMod)this.mod;
			LivesConfigData config = mymod.Config;

			this.Continues--;

			if( config.ContinueDeathDropItems ) {
				this.ApplyContinueInventoryDrop();
			}

			if( config.ContinueDeathMaxHpToll > 0 ) {
				this.ApplyContinueMaxHpToll();
			}
			if( config.ContinueDeathMaxStaminaToll > 0 ) {
				this.ApplyContinueDeathMaxStaminaToll();
			}
			if( config.ContinueDeathRewardsPPToll > 0 ) {
				this.ApplyContinueDeathRewardsPPToll();
			}
		}


		////////////////

		public void ApplyContinueInventoryDrop() {
			int who = this.player.whoAmI;
			byte difficulty = this.player.difficulty;

			this.player.difficulty = 1;  // Set mediumcore

			if( Main.netMode == 1 ) {
				ClientPacketHandlers.SignalDifficultyChangeFromClient( this.player, 3 );
			}

			Timers.SetTimer( "LivesContinueMediumcoreRevert", 60, () => {
				Main.player[who].difficulty = difficulty;
				return false;
			} );
		}

		public void ApplyContinueMaxHpToll() {
			var mymod = (LivesMod)this.mod;
			LivesConfigData config = mymod.Config;

			int newMaxHp = this.player.statMana - config.ContinueDeathMaxHpToll;

			this.player.statLifeMax = Math.Max( newMaxHp, Math.Max( 1, config.ContinueDeathMaxHpMinimum ) );
		}

		public void ApplyContinueDeathMaxStaminaToll() {
			Mod staminaMod = ModLoader.GetMod( "Stamina" );
			if( staminaMod == null ) { return; }

			staminaMod.Call();d
		}

		public void ApplyContinueDeathRewardsPPToll() {
			Mod staminaMod = ModLoader.GetMod( "Rewards" );
			if( staminaMod == null ) { return; }

			staminaMod.Call(); d
		}
	}
}
