using TravelMasterApi.Models;
using TravelMasterApi.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace TravelMasterApi.SecurityManagers
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        /// <summary>
        /// Hàm thực hiện cho việc login lấy token
        /// </summary>
        /// <param name="_authClaims"></param>
        /// <param name="_username">Tên đăng nhập người dùng</param>
        /// <returns></returns>
        public TokenModel? Authenticate(List<Claim> _authClaims,string _username, string _uuid)
        {
            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_uuid))
                return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            
            // khởi tạo token
            var tokenDescriptor = CreateToken(_authClaims, tokenHandler);            
            // khởi tạo mã refresh token
            var _refreshToken = GenerateRefreshToken();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            DateTime _timeExpiredRefresh = DateTime.Now.AddDays(value: GlobalSetting._jwtTokenSettings.RefreshTokenExpirationTime);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return new TokenModel()
            {
                AccessToken = tokenHandler.WriteToken(tokenDescriptor),                
                TimeExpired = tokenDescriptor.ValidTo.ToLocalTime(),
                RefreshToken = _refreshToken,
                TimeStart =  tokenDescriptor.IssuedAt.ToLocalTime(),  
                TimeExpiredRefresh = _timeExpiredRefresh
            };
        } 
        /// <summary>
        /// Hàm tạo ra token with giá trị cần điền vô claim
        /// </summary>
        /// <param name="authClaims">Danh sach claim</param>
        /// <returns></returns>
        private JwtSecurityToken CreateToken(List<Claim> authClaims, JwtSecurityTokenHandler _tokenHandler)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var tokenKey = Encoding.UTF8.GetBytes(GlobalSetting._jwtTokenSettings.Secret);

            var sercurityKey = new SymmetricSecurityKey(tokenKey);

            var credentials = new SigningCredentials(sercurityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = _tokenHandler.CreateJwtSecurityToken(                
                subject: new ClaimsIdentity(authClaims),
                // thời gian so sách token invalid
                notBefore: DateTime.Now,
                // thời gian tồn tại của token 
                // type: double
                expires: DateTime.Now.AddDays(GlobalSetting._jwtTokenSettings.TokenValidityTime),
                issuedAt : DateTime.Now ,
                signingCredentials: credentials
                );
            return token;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
        /// <summary>
        /// Hàm tạo ra Key refresh
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        /// <summary>
        /// Xử lý lấy thông tin từ claim từ token, check tocken
        /// </summary>
        /// <param name="_token"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? _token)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            // cần cấu hình globalSetting jwttokensetting trước trong program
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: GlobalSetting._jwtTokenSettings.Secret)),
                ValidateLifetime = false
            };
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(_token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }
}
