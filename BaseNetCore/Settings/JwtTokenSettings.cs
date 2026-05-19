namespace TravelMasterApi.Settings
{
    public class JwtTokenSettings
    {
        public string ValidAudience { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public double TokenValidityTime { get; set; } = 1;
        public double RefreshTokenExpirationTime { get; set; } = 30;
    }
}
