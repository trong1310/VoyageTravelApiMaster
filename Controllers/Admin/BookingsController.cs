using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using TravelMasterApi.Base.BaseExtension;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Databases;
using TravelMasterApi.Enums;
using TravelMasterApi.Extensions;
using TravelMasterApi.Models.Admin.Request;
using TravelMasterApi.Models.Admin.Response;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Controllers.Admin
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]

    public class BookingsController : ControllerBase
    {
        private readonly MasterDBContext _context;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(MasterDBContext context, ILogger<BookingsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("get")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<GetBookingManagerResponeModels>>), description: "Success")]
        public async Task<IActionResult> Get([FromBody] GetBookingManagerRequestModels request)
        {
            var reps = new BaseResponseMessage<BaseResponseMessagePage<GetBookingManagerResponeModels>>();
            try
            {
                var datas = await _context.Bookings.AsNoTracking().Include(x => x.Category)
                    .Include(x => x.BookingDetail)
                    .Where(x => x.IsEnable == 1)
                    .Where(x => string.IsNullOrEmpty(request.Keyword)||
                    x.BookingDetail.Any(d =>
                            EF.Functions.Like(d.FullName, $"{request.Keyword}%") ||
                            EF.Functions.Like(d.PhoneNumber, $"{request.Keyword}%") ||
                            EF.Functions.Like(d.Email, $"{request.Keyword}%")
                        ))
                    .Where(x => !request.CategoryId.HasValue || x.CategoryId == request.CategoryId.Value)
                    .Where(x => !request.State.HasValue || x.State == request.State.Value)
                    .Where(x => !request.From.HasValue || x.CreatedAt >= request.From.Value)
                    .Where(x => !request.To.HasValue || x.CreatedAt <= request.To.Value)
                    .Select(b => new GetBookingManagerResponeModels
                    {
                        Uuid = b.Uuid,
                        State = b.State,
                        CategoryId = b.CategoryId,
                        CategoryName = b.Category.Name,
                        CreatedAt = b.CreatedAt,
                        Slug = b.SlugOwner,
                        ServiceName = b.SlugOwner,
                        FullName = b.BookingDetail.FirstOrDefault(x=>x.BookingUuid==b.Uuid).FullName??"",
                        PhoneNumber = b.BookingDetail.FirstOrDefault(x => x.BookingUuid == b.Uuid).PhoneNumber ?? "",
                        Type = b.CategoryId == (long)eCategories.Tours ? (sbyte)1 : b.CategoryId == (long)eCategories.Hotels ? (sbyte)2 : b.CategoryId == (long)eCategories.Car ? (sbyte)3 : (sbyte)0
                    }).TakePage(request.Page, request.Limit);
                foreach (var item in datas)
                {

                    if (!string.IsNullOrEmpty(item.Slug))
                    {
                        if (item.CategoryId == (long)eCategories.Tours)
                        {
                            var tourName = await _context.Tours.AsNoTracking().Where(t => t.Slug == item.Slug).Select(t => t.Title).FirstOrDefaultAsync();
                            if (!string.IsNullOrEmpty(tourName))
                            {
                                item.ServiceName = tourName;
                            }
                        }
                        else if (item.CategoryId == (long)eCategories.Hotels)
                        {
                            var hotelName = await _context.Hotels.AsNoTracking().Where(h => h.Slug == item.Slug).Select(h => h.Name).FirstOrDefaultAsync();
                            if (!string.IsNullOrEmpty(hotelName))
                            {
                                item.ServiceName = hotelName;
                            }
                        }
                        else if (item.CategoryId == (long)eCategories.Car)
                        {
                            var carName = await _context.Cars.AsNoTracking().Where(c => c.Slug == item.Slug).Select(c => c.Name).FirstOrDefaultAsync();
                            if (!string.IsNullOrEmpty(carName))
                            {
                                item.ServiceName = carName;
                            }
                        }
                    }
                }

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
                _logger.LogError($"API Get Bookings Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }

        [Authorize]
        [HttpGet("detail/{uuid}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<DetailBookingManagerResponeModels>), description: "Success")]
        public async Task<IActionResult> GetDetail([FromRoute] string uuid)
        {
            var reps = new BaseResponseMessage<DetailBookingManagerResponeModels>();
            try
            {
                if (string.IsNullOrEmpty(uuid))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.UUID_IS_EMPTY);
                    return new OkObjectResult(reps);
                }

                var booking = await _context.Bookings
                    .Include(x => x.Category)
                    .Include(x => x.BookingDetail)
                    .FirstOrDefaultAsync(x => x.Uuid == uuid && x.IsEnable == 1);

                if (booking == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }

                var detail = booking.BookingDetail.FirstOrDefault();

                var detailRep = new DetailBookingManagerResponeModels
                {
                    Uuid = booking.Uuid,
                    State = booking.State,
                    CategoryId = booking.CategoryId,
                    CategoryName = booking.Category?.Name,
                    CreatedAt = booking.CreatedAt,
                    FullName = detail?.FullName,
                    PhoneNumber = detail?.PhoneNumber,
                    SpecialRequirements = detail?.SpecialRequirements,
                    Email = detail?.Email,
                    StartTime = detail?.StartTime,
                    EndTime = detail?.EndTime,
                    TotalCustomer = detail?.TotalCustomer ?? 0,

                    ServiceName = booking.SlugOwner
                };

                if (!string.IsNullOrEmpty(booking.SlugOwner))
                {
                    if (booking.CategoryId == (long)eCategories.Tours)
                    {
                        var tour = await _context.Tours.Include(x => x.DepartureSlugNavigation).FirstOrDefaultAsync(t => t.Slug == booking.SlugOwner);
                        if (tour != null)
                        {
                            detailRep.ServiceName = tour.Title;
                        }
                    }
                    else if (booking.CategoryId == (long)eCategories.Hotels)
                    {
                        var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Slug == booking.SlugOwner);
                        if (hotel != null)
                        {
                            detailRep.ServiceName = hotel.Name;
                        }
                    }
                    else if (booking.CategoryId == (long)eCategories.Car)
                    {
                        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Slug == booking.SlugOwner);
                        if (car != null)
                        {
                            detailRep.ServiceName = car.Name;
                        }
                    }
                }

                reps.Data = detailRep;
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API GetDetail Booking Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }

        [Authorize]
        [HttpPost("update-state")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "Success")]
        public async Task<IActionResult> UpdateState([FromBody] UpdateStateBookingRequestModels request)
        {
            var reps = new BaseResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(request.Uuid))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.UUID_IS_EMPTY);
                    return new OkObjectResult(reps);
                }

                var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Uuid == request.Uuid && x.IsEnable == 1);
                if (booking == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }

                booking.State = request.State;
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API UpdateState Booking Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }

        [Authorize]
        [HttpPost("delete/{uuid}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "Success")]
        public async Task<IActionResult> Delete([FromRoute] string uuid)
        {
            var reps = new BaseResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(uuid))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.UUID_IS_EMPTY);
                    return new OkObjectResult(reps);
                }

                var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Uuid == uuid && x.IsEnable == 1);
                if (booking == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }

                booking.IsEnable = 0;

                var details = await _context.BookingDetail.Where(x => x.BookingUuid == booking.Uuid && x.IsEnable == 1).ToListAsync();
                if (details.Any())
                {
                    details.ForEach(x => x.IsEnable = 0);
                    _context.BookingDetail.UpdateRange(details);
                }

                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Delete Booking Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }
    }
}
