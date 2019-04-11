using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Lives.NetProtocol {
	static class ServerPacketHandlers {
		public static void HandlePacket( BinaryReader reader, int playerWho ) {
			LivesNetProtocolTypes protocol = (LivesNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case LivesNetProtocolTypes.RequestModSettings:
				ServerPacketHandlers.ReceiveSettingsRequestWithServer( reader, playerWho );
				break;
			case LivesNetProtocolTypes.SignalDifficultyChange:
				ServerPacketHandlers.ReceiveDifficultyChangeSignalWithServer( reader, playerWho );
				break;
			default:
				LogHelpers.Warn( "Invalid packet protocol: " + protocol );
				break;
			}
		}


		
		////////////////////////////////
		// Senders (server)
		////////////////////////////////

		public static void SendSettingsFromServer( Player player ) {
			if( Main.netMode != 2 ) { return; } // Server only

			var mymod = LivesMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.ConfigJson.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}


		
		////////////////////////////////
		// Recipients (server)
		////////////////////////////////

		private static void ReceiveSettingsRequestWithServer( BinaryReader reader, int playerWho ) {
			if( Main.netMode != 2 ) { return; } // Server only

			ServerPacketHandlers.SendSettingsFromServer( Main.player[playerWho] );
		}

		private static void ReceiveDifficultyChangeSignalWithServer( BinaryReader reader, int playerWho ) {
			if( Main.netMode != 2 ) { return; } // Clients only

			var mymod = LivesMod.Instance;
			Player player = Main.player[ playerWho ];

			byte difficulty = reader.ReadByte();

			switch( difficulty ) {
			case 0:
			case 1:
			case 2:
				player.difficulty = difficulty;
				break;
			case 3:
				var myplayer = TmlHelpers.SafelyGetModPlayer<LivesPlayer>( player );
				myplayer.ApplyContinueInventoryDrop();
				break;
			default:
				LogHelpers.Warn( "Invalid difficulty setting. " + difficulty );
				break;
			}
		}
	}
}
