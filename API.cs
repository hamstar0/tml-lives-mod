using System;
using Terraria;


namespace Lives {
	public static partial class LivesAPI {
		public static LivesConfigData GetModSettings() {
			return LivesMod.Instance.ConfigJson.Data;
		}


		public static bool AddLives( Player player, int lives ) {
			var myplayer = player.GetModPlayer<LivesPlayer>();
			return myplayer.AddLives( lives );
		}
		
		public static int GetLives( Player player ) {
			var myplayer = player.GetModPlayer<LivesPlayer>();
			return myplayer.Lives;
		}
		public static int GetDeaths( Player player ) {
			var myplayer = player.GetModPlayer<LivesPlayer>();
			return myplayer.Deaths;
		}
		public static byte GetOriginalDifficulty( Player player ) {
			var myplayer = player.GetModPlayer<LivesPlayer>();
			return myplayer.OriginalDifficulty;
		}
		public static bool IsImmortal( Player player ) {
			var myplayer = player.GetModPlayer<LivesPlayer>();
			return myplayer.IsImmortal;
		}
	}
}
