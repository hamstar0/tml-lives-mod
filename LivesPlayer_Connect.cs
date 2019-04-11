using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Lives.NetProtocols;
using System;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesPlayer : ModPlayer {
		private void OnSingleConnect() {
			this.UpdateMortality();
		}

		private void OnCurrentClientConnect() {
			PacketProtocolRequestToServer.QuickRequest<ModSettingsProtocol>( -1 );
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
