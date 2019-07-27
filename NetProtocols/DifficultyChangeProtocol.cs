using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Protocols.Packet.Interfaces;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using Terraria;


namespace Lives.NetProtocols {
	class DifficultyChangeProtocol : PacketProtocolSendToServer {
		public static void SendToServer( byte newDifficulty ) {
			var protocol = new DifficultyChangeProtocol( newDifficulty );
			protocol.SendToServer( false );
		}



		////////////////

		public byte NewDifficulty;



		////////////////

		private DifficultyChangeProtocol() { }

		private DifficultyChangeProtocol( byte newDifficulty ) {
			this.NewDifficulty = newDifficulty;
		}

		////

		protected override void InitializeClientSendData() { }


		////////////////

		protected override void Receive( int fromWho ) {
			Player player = Main.player[ fromWho ];
			var myplayer = TmlHelpers.SafelyGetModPlayer<LivesPlayer>( player );

			switch( this.NewDifficulty ) {
			case 0:
			case 1:
			case 2:
				player.difficulty = this.NewDifficulty;
				break;
			case 3:
				myplayer.ApplyContinueDeathInventoryDropState();
				break;
			default:
				LogHelpers.Warn( "Invalid difficulty setting. " + this.NewDifficulty );
				break;
			}
		}
	}
}
