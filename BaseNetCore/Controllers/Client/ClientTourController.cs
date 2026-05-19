using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TravelMasterApi.Base.BaseExtension;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Controllers.Admin;
using TravelMasterApi.Databases;
using TravelMasterApi.Models.Admin.Request;
using TravelMasterApi.Models.Admin.Response;
using TravelMasterApi.Models.ResponseModel;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Controllers.Client
{
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ClientTourController : ControllerBase
    {
        private readonly MasterDBContext _context;
        private readonly ILogger<ClientTourController> _logger;

        public ClientTourController(MasterDBContext context, ILogger<ClientTourController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost("")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<TourClientResponeModels>>), description: "Success full")]
        public async Task<IActionResult> Get([FromBody] GetTourRequestModelForClient request)
        {
            var reps = new BaseResponseMessage<BaseResponseMessagePage<TourClientResponeModels>>();
            try
            {
                var query = await _context.Tours.AsNoTracking().Include(x => x.DepartureSlugNavigation)
                    .Where(x => x.IsEnable == 1)
                    .Where(x => !request.Ranking.HasValue || request.Ranking.Value == x.Ranking)
                    .Where(x => !request.IsHot.HasValue || request.IsHot.Value == x.IsHot)
                    .Select(x => new TourClientResponeModels
                    {
                        Slug = x.Slug,
                        Title = x.Title,
                       
                        Thumbnail = x.Thumbnail,
                        OriginalPrices = x.OriginalPrices,
                        SalePrices = x.SalePrices,
                        Durations = $"{x.DurationDays}D{x.DurationNights}N",
                        CreatedAt = x.CreatedAt

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
                _logger.LogError($"API Get Tours Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }
    }
}
