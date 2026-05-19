using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TravelMasterApi.Base.BaseExtension;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Databases;
using TravelMasterApi.Models.Admin.Request;
using TravelMasterApi.Models.Admin.Response;
using TravelMasterApi.Models.RequestModel;
using TravelMasterApi.Models.ResponseModel;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Controllers.Client
{
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ClientCarsController : ControllerBase
    {
        private readonly MasterDBContext _context;
        private readonly ILogger<ClientCarsController> _logger;

        public ClientCarsController(MasterDBContext context, ILogger<ClientCarsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost("")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<GetClientCarManagerRespones>>), description: "Success")]
        public async Task<IActionResult> Get([FromBody] CarCientRequestModel request)
        {
            var reps = new BaseResponseMessage<BaseResponseMessagePage<GetClientCarManagerRespones>>();
            try
            {
                var query = await _context.Cars.AsNoTracking()
                    .Where(x => x.IsEnable == 1)
                    .Where(x => !request.IsHot.HasValue || request.IsHot.Value == x.IsHot)
                    .Select(x => new GetClientCarManagerRespones
                    {
                        Name = x.Name,
                        Slug = x.Slug,
                        Brand = x.Brand,
                        ThumbNail = x.ThumbNail,
                        CreatedAt = x.CreatedAt,
                        Routes = x.CarRoute.Where(r => r.IsEnable == 1).Select(r => new CarRouteResponseObject
                        {
                            Name = r.SlugLocationsNavigation.Name,
                            Slug = r.SlugLocations,
                            DisplayOrder = r.DisplayOrder
                        }).OrderBy(r => r.DisplayOrder).ToList()
                    }).OrderByDescending(x => x.CreatedAt).TakePage(request.Page, request.Limit);

                reps.Data = new()
                {
                    Items = query,
                    Pagination = new()
                    {
                        TotalCount = query.TotalCount,
                        TotalPage = query.TotalPages,
                    }
                };
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Get Cars Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }
    }
}
