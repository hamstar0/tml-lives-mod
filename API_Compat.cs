using System;
using Terraria;


namespace Lives {
	public static partial class LivesAPI {
		public static void ResetPlayerModData( Player player ) {    // <- In accordance with Mod Helpers convention
			var myplayer = player.GetModPlayer<LivesPlayer>();
			myplayer.ResetLivesToDefault();
		}
	}
}
