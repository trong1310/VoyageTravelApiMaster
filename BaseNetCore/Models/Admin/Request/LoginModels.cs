using System;

namespace TravelMasterApi.Models.Admin.Request
{
    public class LoginRequestModel
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginResponseModel
    {
        public string Uuid { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime? TimeExpired { get; set; }
        public DateTime? TimeStart { get; set; }
    }
}
