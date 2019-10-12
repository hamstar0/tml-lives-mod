using System;
using Terraria.ModLoader;
using HamstarHelpers.Services.Messages.Inbox;
using HamstarHelpers.Helpers.TModLoader.Mods;


namespace Lives {
	partial class LivesMod : Mod {
		public static LivesMod Instance { get; private set; }



		////////////////

		public LivesConfig Config => ModContent.GetInstance<LivesConfig>();



		////////////////

		public LivesMod() {
			LivesMod.Instance = this;
		}

		public override void Load() {
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



		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( LivesAPI ), args );
		}
	}
}
