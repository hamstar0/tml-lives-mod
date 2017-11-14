namespace Lives {
	public static class LivesAPI {
		public static LivesConfigData GetModSettings() {
			return LivesMod.Instance.Config.Data;
		}
	}
}
