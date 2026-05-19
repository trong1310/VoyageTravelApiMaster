using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TravelMasterApi.Base.BaseExtension;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Databases;
using TravelMasterApi.Models.Admin.Request;
using TravelMasterApi.Models.Admin.Response;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Controllers.Admin
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]

    public class CommonController : ControllerBase
    {
        private readonly MasterDBContext _context;
        private readonly ILogger<CommonController> _logger;

        public CommonController(MasterDBContext context, ILogger<CommonController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// danh sách các địa điểm 
        /// </summary>
        /// <remarks>
        /// 0 trong nước
        /// 1 nước ngoài
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("locations")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<DataFilterLocaltionResponeMessage>>), description: "successful operation")]
        public async Task<IActionResult> GetLocations([FromBody] GetLocationRequestModels request)
        {
            var reps = new BaseResponseMessage<BaseResponseMessagePage<DataFilterLocaltionResponeMessage>>();

            try
            {
                var datas = await _context.Locations.AsNoTracking().Where(x => x.IsEnable == 1)
                    .Where(x => string.IsNullOrEmpty(request.Keyword) || EF.Functions.Like(x.Name, $"{request.Keyword}%"))
                    .Select(x => new DataFilterLocaltionResponeMessage
                    {
                        Name = x.Name,
                        Slug = x.Slug,
                    }).TakePage(request.Page, request.Limit);
                reps.Data = new()
                {
                    Items = datas,
                    Pagination = new()
                    {
                        TotalCount = datas.TotalCount,
                        TotalPage = datas.TotalPages,
                    }
                };
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetLocations Error Message {ex.Message} \n DateTime UTC {DateTime.UtcNow} \n StrackTrace {ex.StackTrace}");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }

        }

    }
}
