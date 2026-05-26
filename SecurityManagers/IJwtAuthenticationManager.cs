using System.Security.Claims;
using TravelMasterApi.Models;

namespace TravelMasterApi.SecurityManagers
{
    public interface IJwtAuthenticationManager
    {
        /// <summary>
        /// Hàm thực hiện cho việc login lấy token
        /// </summary>
        /// <param name="AuthClaims"></param>
        /// <param name="_username"></param>
        /// <returns></returns>
        TokenModel? Authenticate(List<Claim> AuthClaims,string _username, string _uuid);
        /// <summary>
        /// Xử lý lấy thông tin từ claim từ token, check token
        /// </summary>
        /// <param name="_accessToken"></param>
        /// <returns></returns>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string _accessToken);
    }
}
