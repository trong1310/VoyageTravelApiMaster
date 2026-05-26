using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Databases;
using TravelMasterApi.Enums;
using TravelMasterApi.Models.Admin.Request;
using TravelMasterApi.Models.Admin.Response;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Controllers.Admin
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly MasterDBContext _context;
        private readonly ILogger<StatisticsController> _logger;

        public StatisticsController(MasterDBContext context, ILogger<StatisticsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("get-statistics")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<StatisticResponseModel>), description: "Success")]
        public async Task<IActionResult> GetStatistics([FromBody] StatisticRequestModel request)
        {
            var reps = new BaseResponseMessage<StatisticResponseModel>();
            try
            {
                // 1. Tổng đang hoạt động (IsEnable == 1)
                var totalActiveTours = await _context.Tours.AsNoTracking().Where(x => x.IsEnable == 1).CountAsync();
                var totalActiveCars = await _context.Cars.AsNoTracking().Where(x => x.IsEnable == 1).CountAsync();
                var totalActiveHotels = await _context.Hotels.AsNoTracking().Where(x => x.IsEnable == 1).CountAsync();

                // 2. Thống kê đã bán (IsEnable == 1 và trạng thái Processed hoặc PendingProcessing)
                // Theo ngữ cảnh, không tính Cancelled
                var baseBookingQuery = _context.Bookings.AsNoTracking()
                    .Where(x => x.IsEnable == 1 && x.State != (sbyte)eBookingState.Cancelled);

                if (request.From.HasValue)
                {
                    baseBookingQuery = baseBookingQuery.Where(x => x.CreatedAt.Date >= request.From.Value.Date);
                }
                if (request.To.HasValue)
                {
                    baseBookingQuery = baseBookingQuery.Where(x => x.CreatedAt.Date <= request.To.Value.Date);
                }

                // Tổng tour đã bán
                var totalSoldTours = await baseBookingQuery.Where(x => x.CategoryId == (long)eCategories.Tours).CountAsync();

                // Tổng khách sạn đã bán
                var totalSoldHotels = await baseBookingQuery.Where(x => x.CategoryId == (long)eCategories.Hotels).CountAsync();

                // Lượt khách đi xe (Tổng số TotalCustomer của xe)
                var totalCustomerCars = await baseBookingQuery
                    .Where(x => x.CategoryId == (long)eCategories.Car)
                    .SelectMany(x => x.BookingDetail.Where(d => d.IsEnable == 1))
                    .SumAsync(x => (int?)x.TotalCustomer) ?? 0;

                reps.Data = new StatisticResponseModel
                {
                    TotalActiveTours = totalActiveTours,
                    TotalActiveCars = totalActiveCars,
                    TotalActiveHotels = totalActiveHotels,
                    TotalSoldTours = totalSoldTours,
                    TotalCustomerCars = totalCustomerCars,
                    TotalSoldHotels = totalSoldHotels
                };

                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API GetStatistics Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }
    }
}
