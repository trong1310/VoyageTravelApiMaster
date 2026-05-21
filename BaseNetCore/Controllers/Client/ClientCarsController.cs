using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.RegularExpressions;
using TravelMasterApi.Base.BaseExtension;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Databases;
using TravelMasterApi.Enums;
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

        [HttpPost("detail/{slug}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<DetailCarClientResponeModels>), description: "Success")]
        public async Task<IActionResult> Detail([FromRoute] string slug)
        {
            var reps = new BaseResponseMessage<DetailCarClientResponeModels>();
            try
            {
                if (string.IsNullOrEmpty(slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                var query = await _context.Cars.AsNoTracking()
                    .Where(x => x.Slug == slug && x.IsEnable == 1)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }

                var routes = await _context.CarRoute.AsNoTracking()
                    .Where(x => x.SlugCar == query.Slug && x.IsEnable == 1)
                    .Include(x => x.SlugLocationsNavigation)
                    .Select(r => new CarRouteResponseObject
                    {
                        Name = r.SlugLocationsNavigation.Name,
                        Slug = r.SlugLocations,
                        DisplayOrder = r.DisplayOrder
                    }).OrderBy(r => r.DisplayOrder).ToListAsync();

                reps.Data = new DetailCarClientResponeModels()
                {
                    Name = query.Name,
                    Slug = query.Slug,
                    LicensePlate = query.LicensePlate,
                    SeatCount = query.SeatCount,
                    Brand = query.Brand,
                    Color = query.Color,
                    ManufactureYear = query.ManufactureYear,
                    Description = query.Description,
                    ThumbNail = query.ThumbNail,
                    Routes = routes
                };
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Get Car Detail Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
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
                if (request.TotalCustomer > query.SeatCount)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SEAT_ALREADY_EXISTS);
                    return new OkObjectResult(reps);
                }
                var checkBookingTime = await _context.Bookings.AsNoTracking().Where(x => request.Slug == x.SlugOwner && x.CategoryId == (long)eCategories.Car
                && x.BookingDetail.Any(a => a.StartTime.Date == request.StartTime.Value.Date)).ToListAsync();
                if (checkBookingTime.Any())
                {
                    var countSeat = checkBookingTime.Sum(a => a.BookingDetail.Sum(b => b.TotalCustomer));
                    var checkSeat = query.SeatCount - countSeat;
                    if (request.TotalCustomer > checkSeat)
                    {
                        reps.error = new BaseResponseMessage.Error(ErrorCode.FAILED);
                        reps.error.Message = $"Không đủ ghế trống ngày bạn chọn chỉ còn lại {checkSeat} ghế trống";
                        return new OkObjectResult(reps);
                    }
                }
                var booking = new Bookings()
                {
                    Uuid = Guid.NewGuid().ToString(),
                    State = (sbyte)eBookingState.PendingProcessing,
                    CategoryId = (long)eCategories.Car,
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
