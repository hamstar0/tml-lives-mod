using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Services.Messages;
using System;
using Terraria.ModLoader;


namespace Lives {
	partial class LivesMod : Mod {
		public static LivesMod Instance { get; private set; }



		////////////////

		public JsonConfig<LivesConfigData> ConfigJson { get; private set; }
		public LivesConfigData Config => this.ConfigJson.Data;



		////////////////

		public LivesMod() {
			this.ConfigJson = new JsonConfig<LivesConfigData>( LivesConfigData.ConfigFileName, ConfigurationDataBase.RelativePath, new LivesConfigData() );
		}

		public override void Load() {
			string depErr = TmlHelpers.ReportBadDependencyMods( this );
			if( depErr != null ) { throw new HamstarException( depErr ); }

			LivesMod.Instance = this;

			this.LoadConfigs();
		}

		public override void PostSetupContent() {
			InboxMessages.SetMessage( "LivesFeaturingContinues",
				"As of v2.0.0, Lives mod now features continues. These need to be enabled by config. Set \"ContinuesLimit\" to -1 for unlimited continues, or > 0 for a finite amount.",
				false
			);
		}

		public override void Unload() {
			LivesMod.Instance = null;
		}

		////

		private bool LoadConfigs() {
			bool hasUpdated = false;
			var oldConfig = new JsonConfig<LivesConfigData>( "Lives 1.2.0.json", "", new LivesConfigData() );

			// Update old config to new location
			if( oldConfig.LoadFile() ) {
				oldConfig.DestroyFile();
				oldConfig.SetFilePath( this.ConfigJson.FileName, ConfigurationDataBase.RelativePath );
				this.ConfigJson = oldConfig;
			}

			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.ConfigJson.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Lives config updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();

				hasUpdated = true;
			}

			return hasUpdated;
		}



		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( LivesAPI ), args );
		}
	}
}
