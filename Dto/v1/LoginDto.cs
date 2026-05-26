using System.ComponentModel.DataAnnotations;

namespace TravelMasterApi.Dto.v1
{
    public class LoginDto
    {
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [Required]
        public string UserName { get; set; }
		/// <summary>
		/// Mật khẩu
		/// </summary>
		[Required]
		public string Password { get; set; }
    }
}
