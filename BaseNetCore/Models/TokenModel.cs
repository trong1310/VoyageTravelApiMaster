namespace TravelMasterApi.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeExpired { get; set; }
        public DateTime? TimeExpiredRefresh { get; set; }
        
    }
}
