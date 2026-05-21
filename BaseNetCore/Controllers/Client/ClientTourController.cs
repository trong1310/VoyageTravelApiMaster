using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.RegularExpressions;
using TravelMasterApi.Base.BaseExtension;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Controllers.Admin;
using TravelMasterApi.Databases;
using TravelMasterApi.Enums;
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
                    .Where(x => request.Departures == null || !request.Departures.Any() || request.Departures.Contains(x.DepartureSlug))
                    .Where(x => request.Destinations == null || !request.Destinations.Any() || x.TourDestination.Any(a=>a.IsEnable==1&& request.Destinations.Contains(a.LocationSlug)))
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
        [HttpPost("detail/{slug}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<DetailTourClientResponeModels>), description: "Success full")]
        public async Task<IActionResult> Detail([FromRoute] string slug)
        {
            var reps = new BaseResponseMessage<DetailTourClientResponeModels>();
            try
            {
                if (string.IsNullOrEmpty(slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                var query = await _context.Tours.AsNoTracking().Include(x => x.TourDestination).ThenInclude(a => a.LocationSlugNavigation).Where(x => x.Slug == slug && x.IsEnable == 1).FirstOrDefaultAsync();
                if (query == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }
                var images = await _context.Images.AsNoTracking().Where(x => x.OwnerUuid == query.Uuid).Select(x => x.Path).ToListAsync();
                reps.Data = new DetailTourClientResponeModels()
                {
                    Destinations = query.TourDestination.Where(x => x.TourSlug == slug).OrderByDescending(x => x.DisplayOrder).Select(x => x.LocationSlugNavigation.Name).ToList(),
                    Images = images,
                    Slug = query.Slug,
                    Title = query.Title,
                    Thumbnail = query.Thumbnail,
                    OriginalPrices = query.OriginalPrices,
                    SalePrices = query.SalePrices,
                    DurationDays = query.DurationDays,
                    DurationNights = query.DurationNights,
                    Introduce = query.Introduce,

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
        [HttpPost("booking")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "successful operation")]
        public async Task<IActionResult> Bookings([FromBody] BookingTourFixedRequestModels request)
        {
            var reps = new BaseResponseMessage();
            await _context.Database.BeginTransactionAsync();
            try
            {

                if (string.IsNullOrEmpty(request.Slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.FullName))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NAME_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.PhoneNumber))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.PHONENUMBER_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (!Regex.IsMatch(request.PhoneNumber, @"^\+?\d{10,15}$"))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.PHONE_FORMAT);
                    return new OkObjectResult(reps);
                }
                if (!string.IsNullOrEmpty(request.Email) && !Regex.IsMatch(request.Email, @"^\S+@\S+\.\S+$"))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.EMAIL_FORMAT);
                    return new OkObjectResult(reps);
                }
                if (!request.StartTime.HasValue)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.TIME_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
               
                var query = await _context.Tours.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == request.Slug && x.IsEnable == 1);
                if (query is null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }
                var booking = new Bookings()
                {
                    Uuid = Guid.NewGuid().ToString(),
                    State = (sbyte)eBookingState.PendingProcessing,
                    CategoryId = (long)eCategories.Tours,
                    SlugOwner = request.Slug,

                };
                await _context.Bookings.AddAsync(booking);
                var bookingCombo = new BookingDetail()
                {
                    Uuid = Guid.NewGuid().ToString(),
                    BookingUuid = booking.Uuid,
                    TotalCustomer = request.TotalCustomer,
                    Email = request.Email,
                    FullName = request.FullName,
                    PhoneNumber = request.PhoneNumber,
                    SpecialRequirements = request.SpecialRequirements,
                    StartTime = request.StartTime.Value,

                };
                await _context.BookingDetail.AddAsync(bookingCombo);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                _logger.LogError($"Booking Combo Error Message: {ex.Message} \n TimeUtc: {DateTime.UtcNow} \n StrackTrace: {ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(Settings.ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }

    }
}
