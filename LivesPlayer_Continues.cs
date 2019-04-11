using HamstarHelpers.Services.Timers;
using Lives.NetProtocols;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesPlayer : ModPlayer {
		public bool IsContinue() {
			var mymod = (LivesMod)this.mod;
			LivesConfigData config = mymod.Config;

			if( config.ContinuesLimit > 0 ) {
				if( this.ContinuesUsed >= config.ContinuesLimit ) {
					return false;
				}
			} else {
				return false;
			}

			if( config.ContinueDeathMaxHpToll > 0 ) {
				int maxHp = this.player.statLifeMax;

				if( config.ContinueDeathMaxHpMinimum <= 0 ) {
					return (maxHp - config.ContinueDeathMaxHpToll) > 0;
				}
			}

			return true;
		}

		public void ApplyContinue() {
			var mymod = (LivesMod)this.mod;
			LivesConfigData config = mymod.Config;

			this.ContinuesUsed++;

			if( config.ContinueDeathDropItems ) {
				this.ApplyContinueDeathInventoryDropState();
			}

			if( config.ContinueDeathMaxHpToll > 0 ) {
				this.ApplyContinueDeathMaxHpToll();
			}
			if( config.ContinueDeathMaxStaminaToll > 0 ) {
				this.ApplyContinueDeathMaxStaminaToll();
			}
			if( config.ContinueDeathRewardsPPToll > 0 ) {
				this.ApplyContinueDeathRewardsPPToll();
			}
			
			Main.NewText( "No lives left. Continuing...", Color.Red );

			foreach( string penalty in this.FormatContinuePenalties() ) {
				Main.NewText( "  "+penalty, Color.Yellow );
			}
			Main.NewText( " " );
		}


		////////////////

		public void ApplyContinueDeathInventoryDropState() {
			int who = this.player.whoAmI;
			byte difficulty = this.player.difficulty;

			this.player.difficulty = 1;  // Set mediumcore

			if( Main.netMode == 1 && who == Main.myPlayer ) {
				DifficultyChangeProtocol.SendToServer( 3 );	//<- Special amount to trigger revert after 1s
			}

			Timers.SetTimer( "LivesContinueMediumcoreRevert", 60, () => {
				Main.player[who].difficulty = difficulty;
				return false;
			} );
		}

		public void ApplyContinueDeathMaxHpToll() {
			var mymod = (LivesMod)this.mod;
			LivesConfigData config = mymod.Config;

			int newMaxHp = this.player.statLifeMax - config.ContinueDeathMaxHpToll;
			
			this.player.statLifeMax = Math.Max( newMaxHp, Math.Max( 20, config.ContinueDeathMaxHpMinimum ) );
		}

		public void ApplyContinueDeathMaxStaminaToll() {
			Mod staminaMod = ModLoader.GetMod( "Stamina" );
			if( staminaMod == null ) { return; }

			var mymod = (LivesMod)this.mod;
			LivesConfigData config = mymod.Config;

			staminaMod.Call( "AddMaxStamina", this.player, -config.ContinueDeathMaxStaminaToll );
		}

		public void ApplyContinueDeathRewardsPPToll() {
			Mod rewardsMod = ModLoader.GetMod( "Rewards" );
			if( rewardsMod == null ) { return; }

			var mymod = (LivesMod)this.mod;
			LivesConfigData config = mymod.Config;

			float pp = (float)rewardsMod.Call( "GetPoints", this.player );
			if( pp <= config.ContinueDeathRewardsPPMinimum ) {
				return;
			}

			rewardsMod.Call( "AddPoints", this.player, (float)-config.ContinueDeathRewardsPPToll );
		}


		////////////////

		public IEnumerable<string> FormatContinuePenalties() {
			var mymod = (LivesMod)this.mod;
			LivesConfigData config = mymod.Config;
			var penalties = new List<string>();

			if( config.ContinuesLimit == 0 ) {
				penalties.Add( "Game over!" );
			} else {
				if( config.ContinuesLimit > 0 ) {
					int continues = config.ContinuesLimit - this.ContinuesUsed;
					penalties.Add( "Lost 1 continue ("+continues+" remain)" );
				}

				if( config.ContinueDeathDropItems ) {
					penalties.Add( "Items dropped." );
				}

				if( config.ContinueDeathMaxHpToll > 0 ) {
					if( config.ContinueDeathMaxHpMinimum <= 0 ) {
						penalties.Add( "Lost " + config.ContinueDeathMaxHpToll + " max health (game over if empty)." );
					} else {
						penalties.Add( "Lost " + config.ContinueDeathMaxHpToll + " max health (min " + config.ContinueDeathMaxHpMinimum+")." );
					}
				}

				if( config.ContinueDeathMaxStaminaToll > 0 ) {
					if( config.ContinueDeathMaxStaminaMinimum <= 0 ) {
						penalties.Add( "Lost " + config.ContinueDeathMaxStaminaToll + " max stamina." );
					} else {
						penalties.Add( "Lost " + config.ContinueDeathMaxStaminaToll + " max stamina (min " + config.ContinueDeathMaxStaminaMinimum + ")." );
					}
				}

				if( config.ContinueDeathRewardsPPToll > 0 ) {
					if( config.ContinueDeathRewardsPPMinimum <= 0 ) {
						penalties.Add( "Lost " + config.ContinueDeathRewardsPPToll + " PP." );
					} else {
						penalties.Add( "Lost " + config.ContinueDeathRewardsPPToll + " PP (min " + config.ContinueDeathRewardsPPMinimum + ")." );
					}
				}
			}

			return penalties;
		}
	}
}
