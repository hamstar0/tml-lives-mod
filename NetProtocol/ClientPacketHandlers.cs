using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Lives.NetProtocol {
	static class ClientPacketHandlers {
		public static void HandlePacket( BinaryReader reader ) {
			LivesNetProtocolTypes protocol = (LivesNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case LivesNetProtocolTypes.ModSettings:
				ClientPacketHandlers.ReceiveSettingsWithClient( reader );
				break;
			default:
				LogHelpers.Warn( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////////////////////
		// Senders (client)
		////////////////////////////////

		public static void RequestSettingsWithClient( Player player ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			var mymod = LivesMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.RequestModSettings );
			packet.Send();
		}

		public static void SignalDifficultyChangeFromClient( Player player, byte difficulty ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			var mymod = LivesMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.SignalDifficultyChange );
			packet.Write( (byte)difficulty );
			packet.Send();
		}



		////////////////////////////////
		// Recipients (client)
		////////////////////////////////

		private static void ReceiveSettingsWithClient( BinaryReader reader ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			var mymod = LivesMod.Instance;
			bool success;

			mymod.ConfigJson.DeserializeMe( reader.ReadString(), out success );
			if( !success ) {
				throw new HamstarException("Could not deserialize mod settings.");
			}

			var modplayer = Main.player[Main.myPlayer].GetModPlayer<LivesPlayer>();
			modplayer.UpdateMortality();
		}
	}
}
