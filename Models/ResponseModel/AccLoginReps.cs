namespace TravelMasterApi.Models.ResponseModel
{
    public class AccLoginResp
    {
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        public string Uuid { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        /// <summary>
        /// Token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
        /// <summary>
        /// Token Refresh
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
        /// <summary>
        /// Thời gian được cấp
        /// </summary>
        public DateTime? TimeStart { get; set; }
        /// <summary>
        /// Thời gian có hiệu lực của token
        /// </summary>
        public DateTime? TimeExpired { get; set; }

        public string Code { get; set; } = string.Empty;

        public string? Avatar { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsFollow { get; set; } = false;
    }

}
