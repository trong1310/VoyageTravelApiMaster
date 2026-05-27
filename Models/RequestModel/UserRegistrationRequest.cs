using System.ComponentModel.DataAnnotations;

namespace TravelMasterApi.Models.RequestModel
{
    public class UserRegistrationRequest
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        public string? Phone { get; set; } = string.Empty;
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }
        public ulong IsEmailNotify { get; set; } = 1;
        public string DeviceId { get; set; } = string.Empty;
        public string? Url { get; set; } = string.Empty;
    }
}
