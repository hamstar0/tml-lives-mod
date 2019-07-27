using HamstarHelpers.Helpers.Debug;
using System;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesPlayer : ModPlayer {
		private void OnSingleConnect() {
			this.UpdateMortality();
		}

		private void OnCurrentClientConnect() {
		}

		private void OnServerConnect() {
			this.UpdateMortality();
		}


		////////////////

		internal void FinishLocalModSettingsSync() {
			if( !this.IsLoaded ) {
				this.IsLoaded = true;
				this.ResetLivesToDefault();
			}

			this.UpdateMortality();
		}
	}
}
