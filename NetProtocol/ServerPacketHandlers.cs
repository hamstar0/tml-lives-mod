using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Lives.NetProtocol {
	static class ServerPacketHandlers {
		public static void HandlePacket( LivesMod mymod, BinaryReader reader, int player_who ) {
			LivesNetProtocolTypes protocol = (LivesNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case LivesNetProtocolTypes.RequestModSettings:
				ServerPacketHandlers.ReceiveSettingsRequestWithServer( mymod, reader, player_who );
				break;
			case LivesNetProtocolTypes.SignalDifficultyChange:
				ServerPacketHandlers.ReceiveDifficultyChangeSignalWithServer( mymod, reader, player_who );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}


		
		////////////////////////////////
		// Senders (server)
		////////////////////////////////

		public static void SendSettingsFromServer( LivesMod mymod, Player player ) {
			if( Main.netMode != 2 ) { return; } // Server only

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.ConfigJson.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}


		
		////////////////////////////////
		// Recipients (server)
		////////////////////////////////

		private static void ReceiveSettingsRequestWithServer( LivesMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { return; } // Server only

			ServerPacketHandlers.SendSettingsFromServer( mymod, Main.player[player_who] );
		}

		private static void ReceiveDifficultyChangeSignalWithServer( LivesMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { return; } // Clients only
			
			byte difficulty = reader.ReadByte();
			
			if( difficulty < 0 || difficulty > 2 ) {
				ErrorLogger.Log( "ReceiveDifficultyChangeSignalWithServer - Invalid difficulty setting. " + difficulty );
				return;
			}

			Main.player[player_who].difficulty = difficulty;
		}
	}
}
