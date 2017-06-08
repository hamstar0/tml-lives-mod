using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	public enum LivesNetProtocolTypes : byte {
		RequestModSettings,
		ModSettings,
		SignalDifficultyChange
	}


	public static class LivesNetProtocol {
		public static void RoutePacket( LivesMod mymod, BinaryReader reader ) {
			LivesNetProtocolTypes protocol = (LivesNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case LivesNetProtocolTypes.RequestModSettings:
				LivesNetProtocol.ReceiveSettingsRequestWithServer( mymod, reader );
				break;
			case LivesNetProtocolTypes.ModSettings:
				LivesNetProtocol.ReceiveSettingsWithClient( mymod, reader );
				break;
			case LivesNetProtocolTypes.SignalDifficultyChange:
				LivesNetProtocol.ReceiveDifficultyChangeSignalWithServer( mymod, reader );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////////////////////
		// Senders (client)
		////////////////////////////////

		public static void RequestSettingsWithClient( LivesMod mymod, Player player ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.RequestModSettings );
			packet.Write( (int)player.whoAmI );
			packet.Send();
		}

		public static void SignalDifficultyChangeFromClient( LivesMod mymod, Player player, byte difficulty ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.SignalDifficultyChange );
			packet.Write( (int)player.whoAmI );
			packet.Write( (byte)difficulty );
			packet.Send();
		}

		////////////////////////////////
		// Senders (server)
		////////////////////////////////

		public static void SendSettingsFromServer( LivesMod mymod, Player player ) {
			if( Main.netMode != 2 ) { return; } // Server only

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.Config.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}



		////////////////////////////////
		// Recipients (client)
		////////////////////////////////

		private static void ReceiveSettingsWithClient( LivesMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			mymod.Config.DeserializeMe( reader.ReadString() );

			var modplayer = Main.player[Main.myPlayer].GetModPlayer<LivesPlayer>( mymod );
			modplayer.UpdateMortality();
		}

		////////////////////////////////
		// Recipients (server)
		////////////////////////////////

		private static void ReceiveSettingsRequestWithServer( LivesMod mymod, BinaryReader reader ) {
			if( Main.netMode != 2 ) { return; } // Server only

			int who = reader.ReadInt32();

			if( who < 0 || who >= Main.player.Length || Main.player[who] == null ) {
				ErrorLogger.Log( "LivesNetProtocol.ReceiveSettingsRequestOnServer - Invalid player whoAmI. " + who );
				return;
			}

			LivesNetProtocol.SendSettingsFromServer( mymod, Main.player[who] );
		}

		private static void ReceiveDifficultyChangeSignalWithServer( LivesMod mymod, BinaryReader reader ) {
			if( Main.netMode != 2 ) { return; } // Clients only

			int who = reader.ReadInt32();
			byte difficulty = reader.ReadByte();

			if( who < 0 || who >= Main.player.Length || Main.player[who] == null ) {
				ErrorLogger.Log( "LivesNetProtocol.ReceiveDifficultyChangeSignalWithServer - Invalid player whoAmI. " + who );
				return;
			}
			if( difficulty < 0 || difficulty > 2 ) {
				ErrorLogger.Log( "LivesNetProtocol.ReceiveDifficultyChangeSignalWithServer - Invalid difficulty setting. " + difficulty );
				return;
			} 

			Main.player[who].difficulty = difficulty;
		}
	}
}
