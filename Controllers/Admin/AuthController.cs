using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Base.Extensions;
using TravelMasterApi.Base.Utils;
using TravelMasterApi.Databases;
using TravelMasterApi.Dto.v1;
using TravelMasterApi.Enums;
using TravelMasterApi.Extensions;
using TravelMasterApi.Models.Admin.Request;
using TravelMasterApi.SecurityManagers;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Controllers.Admin

{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
        {
            private readonly MasterDBContext _context;
            private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
            private readonly ILogger<AuthController> _logger;

            public AuthController(MasterDBContext context, IJwtAuthenticationManager jwtAuthenticationManager, ILogger<AuthController> logger)
            {
                _context = context;
                _jwtAuthenticationManager = jwtAuthenticationManager;
                _logger = logger;
            }

        [HttpPost("login")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<LoginResponseModel>), description: "successful operation")]
        public async Task<ActionResult> Login([FromBody] LoginDto request)
        {
            var resp = new BaseResponseMessage<LoginResponseModel>();
            try
            {
                var user = await _context.ManagerAccounts.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserName == request.UserName);
                if (user is null)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.ACCOUNT_IS_INVALID);
                    return new OkObjectResult(resp);
                }
                if (user.Password != request.Password)
                {
                    return new OkObjectResult(resp.GetMessage(ErrorCode.ACCOUNT_IS_INVALID));
                }
                if (user.Password != request.Password)
                {
                    return new OkObjectResult(resp.GetMessage(ErrorCode.PASSWORD_IS_INCORRECT));
                }
                if (user.State == 0)
                {
                    return new OkObjectResult(resp.GetMessage(ErrorCode.ACCOUNT_HAS_LOCKED));
                }

                var uuid = user.Uuid;

                var _authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("uuid", uuid),
                };

                var _token = _jwtAuthenticationManager.Authenticate(_authClaims, user.UserName, uuid);
                if (_token is null)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(resp);
                }

                //var session = new Session()
                //{
                //    AccountUuid = uuid,
                //    Token = _token.AccessToken,
                //    Username = user.UserName,
                //    TimeExpired = (DateTime)_token.TimeExpired,
                //    IsManager = true,
                //    RefreshToken = _token.RefreshToken,
                //    TimeExpiredRefresh = _token.TimeExpiredRefresh,
                //    State = 1,

                //};

                //await _context.Session.AddAsync(session);
                await _context.SaveChangesAsync();
                resp.Data = new LoginResponseModel()
                {
                    AccessToken = _token.AccessToken,
                    RefreshToken = _token.RefreshToken,
                    TimeExpired = _token.TimeExpired,

                    TimeStart = _token.TimeStart,
                    UserName = user.UserName ?? "",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                resp.GetMessage(ErrorCode.SYSTEM_ERROR);
            }

            return new OkObjectResult(resp);
        }
        ////[ServiceFilter(typeof(AuthorizeTokenAttribute))]
        //[HttpPost("logout")]
        //[SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "successfully")]
        //public async Task<IActionResult> Logout()
        //{
        //    var resp = new BaseResponseMessage();
        //    var acc = User.GetUuid();
        //    try
        //    {
        //        var session = await _context.Session.Where(x => x.AccountUuid == acc).ToListAsync();
        //        if (session.Any())
        //        {
        //            session.ForEach(x => x.State = 2);
        //            _context.Session.UpdateRange(session);
        //        }
        //        await _context.SaveChangesAsync();
        //        return new OkObjectResult(resp);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error:[{DateTime.Now}] Message : {ex.Message}: {ex.StackTrace}");
        //        resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
        //        return new OkObjectResult(resp);
        //    }
        //}

    }
}
