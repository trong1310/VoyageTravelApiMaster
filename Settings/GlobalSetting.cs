namespace TravelMasterApi.Settings
{    
    public static class GlobalSetting
    {
        public static string ROOT_CONTENT { get; set; } = string.Empty;
        public static JwtTokenSettings? _jwtTokenSettings { get; set; }
		public static AppSettings? _appSettings { get; set; }
		public static void Include(AppSettings appSettings,JwtTokenSettings jwtTokenSettings)
        {
            _jwtTokenSettings = jwtTokenSettings;
			_appSettings = appSettings;
		}
    }
}
