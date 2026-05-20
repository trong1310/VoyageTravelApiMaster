using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TravelMasterApi.Base.BaseExtension;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Databases;
using TravelMasterApi.Enums;
using TravelMasterApi.Models.Admin.Response;
using TravelMasterApi.Models.RequestModel;
using TravelMasterApi.Models.ResponseModel;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Controllers.Client
{
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ClientHotelsController : ControllerBase
    {
        private readonly MasterDBContext _context;
        private readonly ILogger<ClientHotelsController> _logger;

        public ClientHotelsController(MasterDBContext context, ILogger<ClientHotelsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<HotelClientResponseModels>>), description: "Success")]
        public async Task<IActionResult> Get([FromBody] HotelClientRequestModel request)
        {
            var reps = new BaseResponseMessage<BaseResponseMessagePage<HotelClientResponseModels>>();
            try
            {
                var query = await _context.Hotels.AsNoTracking().Include(x => x.SlugLocationsNavigation)
                    .Where(x => x.IsEnable == 1)
                    .Where(x => !request.Ranking.HasValue || request.Ranking.Value == x.Ranking)
                    .Where(x => !request.IsHot.HasValue || request.IsHot.Value == x.IsHot)
                    .Where(x => !request.Type.HasValue || request.Type.Value == x.Type)
                    .Where(x => request.Locations == null || !request.Locations.Any() || request.Locations.Contains(x.SlugLocations))
                    .Select(x => new HotelClientResponseModels
                    {
                        Name = x.Name,
                        Slug = x.Slug,
                        Type = x.Type,
                        Ranking = $"{x.Ranking}",
                        RelativePrice = x.RelativePrice,
                        Thumbnail = x.Thumbnail,
                        Locations = x.SlugLocationsNavigation.Name,
                        Address = x.Address,
                        IsHot = x.IsHot,
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
                _logger.LogError($"API Get Hotels Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }

        [HttpPost("detail/{slug}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<DetailHotelClientResponseModels>), description: "Success")]
        public async Task<IActionResult> Detail([FromRoute] string slug)
        {
            var reps = new BaseResponseMessage<DetailHotelClientResponseModels>();
            try
            {
                if (string.IsNullOrEmpty(slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }

                var hotel = await _context.Hotels.AsNoTracking().Include(x => x.SlugLocationsNavigation).Where(x => x.Slug == slug && x.IsEnable == 1).FirstOrDefaultAsync();
                if (hotel == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }

                var lstImages = await _context.Images.AsNoTracking().Where(x => x.OwnerUuid == hotel.Uuid && x.IsEnable == 1).Select(x => x.Path).ToListAsync();

                reps.Data = new DetailHotelClientResponseModels()
                {
                    Name = hotel.Name,
                    Introduce = hotel.Introduce,
                    Thumbnail = hotel.Thumbnail,
                    Images = lstImages,
                    IsHot = hotel.IsHot,
                    Ranking = $"{hotel.Ranking}",
                    Locations = hotel.SlugLocationsNavigation.Name,
                    SlugLocations = hotel.SlugLocations,
                    Type = hotel.Type,
                    RelativePrice = hotel.RelativePrice,
                    Address = hotel.Address,
                    Regulations = hotel.Regulations,
                    Slug = hotel.Slug,
                    HotelsNearby = new List<LocationsObject>(),
                    TouristAttraction = new List<LocationsObject>(),
                    Topic = new List<LocationsObject>()
                };

                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Get Hotel Detail Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
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

                var query = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == request.Slug && x.IsEnable == 1);
                if (query is null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }
                var booking = new Bookings()
                {
                    Uuid = Guid.NewGuid().ToString(),
                    State = (sbyte)eBookingState.PendingProcessing,
                    CategoryId = (long)eCategories.Hotels,
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
