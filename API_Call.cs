using System;
using Terraria;


namespace Lives {
	public static partial class LivesAPI {
		internal static object Call( string call_type, params object[] args ) {
			Player player;

			switch( call_type ) {
			case "GetModSettings":
				return LivesAPI.GetModSettings();
			case "AddLives":
				if( args.Length < 2 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				if( !( args[1] is int ) ) { throw new Exception( "Invalid parameter points for API call " + call_type ); }
				int lives = (int)args[1];

				return LivesAPI.AddLives( player, lives );
			case "GetLives":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return LivesAPI.GetLives( player );
			case "GetDeaths":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return LivesAPI.GetDeaths( player );
			case "GetOriginalDifficulty":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return LivesAPI.GetOriginalDifficulty( player );
			case "IsImmortal":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return LivesAPI.IsImmortal( player );
			}

			throw new Exception( "No such api call " + call_type );
		}
	}
}
