using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.PlayerHelpers;
using Lives.NetProtocol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesMod : Mod {
		public static LivesMod Instance { get; private set; }


		////////////////

		public JsonConfig<LivesConfigData> ConfigJson { get; private set; }
		public LivesConfigData Config { get { return this.ConfigJson.Data; } }


		////////////////

		public LivesMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.ConfigJson = new JsonConfig<LivesConfigData>( LivesConfigData.ConfigFileName, ConfigurationDataBase.RelativePath, new LivesConfigData() );
		}

		public override void Load() {
			LivesMod.Instance = this;

			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_vers = new Version( 1, 2, 0 );
			if( hamhelpmod.Version < min_vers ) {
				throw new Exception( "Hamstar Helpers must be version " + min_vers.ToString() + " or greater." );
			}

			this.LoadConfigs();
		}
		
		private void LoadConfigs() {
			var old_config = new JsonConfig<LivesConfigData>( "Lives 1.2.0.json", "", new LivesConfigData() );
			// Update old config to new location
			if( old_config.LoadFile() ) {
				old_config.DestroyFile();
				old_config.SetFilePath( this.ConfigJson.FileName, ConfigurationDataBase.RelativePath );
				this.ConfigJson = old_config;
			}
			
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.ConfigJson.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Lives config updated to " + LivesConfigData.ConfigVersion.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			LivesMod.Instance = null;
		}



		////////////////

		public override object Call( params object[] args ) {
			if( args.Length == 0 ) { throw new Exception( "Undefined call type." ); }

			string call_type = args[0] as string;
			if( args == null ) { throw new Exception( "Invalid call type." ); }

			var new_args = new object[args.Length - 1];
			Array.Copy( args, 1, new_args, 0, args.Length - 1 );

			return LivesAPI.Call( call_type, new_args );
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			if( Main.netMode == 1 ) {   // Client
				ClientPacketHandlers.HandlePacket( this, reader );
			} else if( Main.netMode == 2 ) {    // Server
				ServerPacketHandlers.HandlePacket( this, reader, player_who );
			}
		}


		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			if( !this.ConfigJson.Data.Enabled ) { return; }

			Player player = Main.player[Main.myPlayer];
			if( player.difficulty == 2 ) { return; }
			
			var modplayer = player.GetModPlayer<LivesPlayer>(this);
			int lives = modplayer.Lives;
			Vector2 pos = new Vector2(Main.screenWidth - 38, Main.screenHeight - 26);

			PlayerHeadDisplayHelpers.DrawPlayerHead(sb, Main.player[Main.myPlayer], Main.screenWidth - 48, Main.screenHeight - 24, 1f, 1f);
			sb.DrawString( Main.fontMouseText, " x" + lives, pos, Color.White );
		}
	}
}
