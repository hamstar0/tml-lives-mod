using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace Lives.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public LivesConfigData Data;



		////////////////

		private ModSettingsProtocol() { }

		////

		protected override void InitializeServerSendData( int fromWho ) {
			this.Data = LivesMod.Instance.Config;
			if( this.Data == null ) {
				throw new HamstarException( "No mod settings available." );
			}
		}


		////////////////

		protected override void ReceiveReply() {
			LivesMod.Instance.ConfigJson.SetData( this.Data );

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<LivesPlayer>();

			myplayer.FinishLocalModSettingsSync();
		}
	}
}
