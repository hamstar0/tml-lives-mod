using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace Lives {
	public enum LivesNetProtocolTypes : byte {
		SendSettingsRequest,
		SendSettings,
		SignalDifficultyChange
	}


	public class LivesNetProtocol {
		public static void RoutePacket( Mod mod, BinaryReader reader ) {
			LivesNetProtocolTypes protocol = (LivesNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case LivesNetProtocolTypes.SendSettingsRequest:
				LivesNetProtocol.ReceiveSettingsRequestWithServer( mod, reader );
				break;
			case LivesNetProtocolTypes.SendSettings:
				LivesNetProtocol.ReceiveSettingsWithClient( mod, reader );
				break;
			case LivesNetProtocolTypes.SignalDifficultyChange:
				LivesNetProtocol.ReceiveDifficultyChangeSignalWithServer( mod, reader );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////////////////////
		// Senders (client)
		////////////////////////////////

		public static void RequestSettingsWithClient( Mod mod, Player player ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			ModPacket packet = mod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.SendSettingsRequest );
			packet.Write( (int)player.whoAmI );
			packet.Send();
		}

		public static void SignalDifficultyChangeFromClient( Mod mod, Player player, byte difficulty ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			ModPacket packet = mod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.SignalDifficultyChange );
			packet.Write( (int)player.whoAmI );
			packet.Write( (byte)difficulty );
			packet.Send();
		}

		////////////////////////////////
		// Senders (server)
		////////////////////////////////

		public static void SendSettingsFromServer( Mod mod, Player player ) {
			if( Main.netMode != 2 ) { return; } // Server only

			ModPacket packet = mod.GetPacket();

			packet.Write( (byte)LivesNetProtocolTypes.SendSettings );
			packet.Write( (int)LivesMod.Config.Data.InitialLives );
			packet.Write( (int)LivesMod.Config.Data.MaxLives );
			packet.Write( (bool)LivesMod.Config.Data.CraftableExtraLives );
			packet.Write( (int)LivesMod.Config.Data.ExtraLifeGoldCoins );
			packet.Write( (bool)LivesMod.Config.Data.ExtraLifeVoodoo );

			packet.Send( (int)player.whoAmI );
		}



		////////////////////////////////
		// Recipients (client)
		////////////////////////////////

		private static void ReceiveSettingsWithClient( Mod mod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			LivesMod.Config.Data.InitialLives = (int)reader.ReadInt32();
			LivesMod.Config.Data.MaxLives = (int)reader.ReadInt32();
			LivesMod.Config.Data.CraftableExtraLives = (bool)reader.ReadBoolean();
			LivesMod.Config.Data.ExtraLifeGoldCoins = (int)reader.ReadInt32();
			LivesMod.Config.Data.ExtraLifeVoodoo = (bool)reader.ReadBoolean();

			var modplayer = Main.player[Main.myPlayer].GetModPlayer<LivesPlayer>( mod );
			modplayer.UpdateMortality();
		}

		////////////////////////////////
		// Recipients (server)
		////////////////////////////////

		private static void ReceiveSettingsRequestWithServer( Mod mod, BinaryReader reader ) {
			if( Main.netMode != 2 ) { return; } // Server only

			int who = reader.ReadInt32();

			if( who < 0 || who >= Main.player.Length || Main.player[who] == null ) {
				ErrorLogger.Log( "LivesNetProtocol.ReceiveSettingsRequestOnServer - Invalid player whoAmI. " + who );
				return;
			}

			LivesNetProtocol.SendSettingsFromServer( mod, Main.player[who] );
		}

		private static void ReceiveDifficultyChangeSignalWithServer( Mod mod, BinaryReader reader ) {
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
