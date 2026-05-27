using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Databases;
using TravelMasterApi.Models.RequestModel;
using TravelMasterApi.Models.ResponseModel;
using TravelMasterApi.SecurityManagers;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Controllers.Client
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly MasterDBContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;
        public UserController(MasterDBContext context, ILogger<UserController> logger,JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _logger = logger;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }
        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<AccLoginResp>), description: "successful operation")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resp = new BaseResponseMessage<AccLoginResp>();

            await _context.Database.BeginTransactionAsync();
            try
            {
                if (!string.IsNullOrEmpty(request.Fullname) && request.Fullname.Length > 35)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.NAME_MAX_LENGTH);
                    return new OkObjectResult(resp);
                }
                if (Regex.IsMatch(request.Email, @"^\S+@\S+\.\S+$"))
                {
                    if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                    {
                        resp.error = new BaseResponseMessage.Error(ErrorCode.EMAIL_ALREADY_EXISTS);
                        return new OkObjectResult(resp);
                    }
                }
                else
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.EMAIL_FORMAT);
                    return new OkObjectResult(resp);
                }
                // Kiểm tra số điện thoại
                if (!string.IsNullOrEmpty(request.Phone))
                {
                    if (Regex.IsMatch(request.Phone, @"^\+?\d{10,15}$"))
                    {
                        if (await _context.Users.AnyAsync(u => u.PhoneNumber == request.Phone))
                        {
                            resp.error = new BaseResponseMessage.Error(ErrorCode.PHONENUMBER_ALREADY_EXISTS);
                            return new OkObjectResult(resp);
                        }
                    }
                    else
                    {
                        resp.error = new BaseResponseMessage.Error(ErrorCode.PHONE_FORMAT);
                        return new OkObjectResult(resp);
                    }
                }
                var user = new Users
                {
                    Uuid = Guid.NewGuid().ToString(),
                    FullName = request.Fullname,
                    Email = request.Email,
                 PhoneNumber   = request.Phone,

                };
                _context.Users.Add(user);

                var _authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("uuid", user.Uuid),
                };

                var _token = _jwtAuthenticationManager.Authenticate(_authClaims, user.Email, user.Uuid);
                if (_token is null)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.ACCOUNT_IS_NOT_CORRECT);
                    return new OkObjectResult(resp);
                }

                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                resp.Data = new AccLoginResp()
                {
                    AccessToken = _token.AccessToken,
                    RefreshToken = _token.RefreshToken,
                    TimeExpired = _token.TimeExpired,
                    TimeStart = _token.TimeStart,
                    UserName = user.Email,
                    FullName = user.FullName ?? "",
                    Code = user.Id.ToString(),
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber ?? "",

                    Uuid = user.Uuid,

                };
                return new OkObjectResult(resp);
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                _logger.LogError(ex.StackTrace);
                resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(resp);
            }
        }

    }
}
