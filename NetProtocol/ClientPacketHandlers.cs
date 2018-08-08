using HamstarHelpers.Components.Errors;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Lives.NetProtocol {
	static class ClientPacketHandlers {
		public static void HandlePacket( LivesMod mymod, BinaryReader reader ) {
			LivesNetProtocolTypes protocol = (LivesNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case LivesNetProtocolTypes.ModSettings:
				ClientPacketHandlers.ReceiveSettingsWithClient( mymod, reader );
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
			packet.Send();
		}

		public static void SignalDifficultyChangeFromClient( LivesMod mymod, Player player, byte difficulty ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.SignalDifficultyChange );
			packet.Write( (byte)difficulty );
			packet.Send();
		}



		////////////////////////////////
		// Recipients (client)
		////////////////////////////////

		private static void ReceiveSettingsWithClient( LivesMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			bool success;

			mymod.ConfigJson.DeserializeMe( reader.ReadString(), out success );
			if( !success ) {
				throw new HamstarException("Lives.NetProtocol.ClientPacketHandler.ReceiveSettingsWithClient - Could not deserialize mod settings.");
			}

			var modplayer = Main.player[Main.myPlayer].GetModPlayer<LivesPlayer>( mymod );
			modplayer.UpdateMortality();
		}
	}
}
