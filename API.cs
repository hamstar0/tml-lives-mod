﻿using Terraria;

namespace Lives {
	public static class LivesAPI {
		public static LivesConfigData GetModSettings() {
			return LivesMod.Instance.Config.Data;
		}


		public static bool AddLives( Player player, int lives ) {
			var myplayer = player.GetModPlayer<MyPlayer>();
			return myplayer.AddLives( lives );
		}
		
		public static int GetLives( Player player ) {
			var myplayer = player.GetModPlayer<MyPlayer>();
			return myplayer.Lives;
		}
		public static int GetDeaths( Player player ) {
			var myplayer = player.GetModPlayer<MyPlayer>();
			return myplayer.Deaths;
		}
		public static byte GetOriginalDifficulty( Player player ) {
			var myplayer = player.GetModPlayer<MyPlayer>();
			return myplayer.OriginalDifficulty;
		}
		public static bool IsImmorta( Player player ) {
			var myplayer = player.GetModPlayer<MyPlayer>();
			return myplayer.IsImmortal;
		}
	}
}
