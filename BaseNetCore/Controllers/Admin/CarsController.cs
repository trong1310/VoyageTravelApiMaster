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
using TravelMasterApi.Base.Utils;
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

    public class CarsController : ControllerBase
    {
        private readonly MasterDBContext _context;
        private readonly ILogger<CarsController> _logger;

        public CarsController(MasterDBContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("create")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "Success")]
        public async Task<IActionResult> Create([FromBody] CreateCarManagerRequestModels request)
        {
            var acc = User.GetUuid();
            if (acc == null)
            {
                return Unauthorized();
            }
            var reps = new BaseResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NAME_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.LicensePlate))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.VALUE_IS_EMPTY) { Message = "Biển số xe không được để trống" };
                    return new OkObjectResult(reps);
                }
                if (request.SeatCount <= 0)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.VALUES_IS_INVALID) { Message = "Số ghế không hợp lệ" };
                    return new OkObjectResult(reps);
                }

                // Check duplicate license plate
                var dupPlate = await _context.Cars.AnyAsync(x => x.LicensePlate == request.LicensePlate.Trim() && x.IsEnable == 1);
                if (dupPlate)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.FAILED) { Message = "Biển số xe đã tồn tại trong hệ thống" };
                    return new OkObjectResult(reps);
                }

                var slug = Util.GenerateSlug(request.Name);
                var checkSlug = await _context.Cars.Where(x => x.Slug == slug).FirstOrDefaultAsync();
                if (checkSlug != null)
                {
                    slug = $"{slug}-{Util.GenerateRandomNumber()}";
                }

                var car = new Cars()
                {
                    Name = request.Name,
                    Slug = slug,
                    LicensePlate = request.LicensePlate.Trim(),
                    SeatCount = request.SeatCount,
                    Brand = request.Brand,
                    Color = request.Color,
                    ManufactureYear = request.ManufactureYear,
                    Description = request.Description,
                    ThumbNail = request.ThumbNail,
                    IsEnable = 1,
                    CreatedAt = DateTime.Now
                };
                await _context.Cars.AddAsync(car);

                // Routes
                if (request.Routes != null && request.Routes.Any())
                {
                    var lstAdd = new List<CarRoute>();
                    foreach (var item in request.Routes)
                    {
                        var loc = await _context.Locations.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == item.Slug);
                        if (loc != null)
                        {
                            var cr = new CarRoute()
                            {
                                SlugCar = car.Slug,
                                SlugLocations = loc.Slug,
                                DisplayOrder = item.DisplayOrder,
                                IsEnable = 1
                            };
                            lstAdd.Add(cr);
                        }
                    }
                    if (lstAdd.Any())
                    {
                        await _context.CarRoute.AddRangeAsync(lstAdd);
                    }
                }

                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Car Create Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }

        [Authorize]
        [HttpPost("update")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "Success")]
        public async Task<IActionResult> Update([FromBody] UpdateCarManagerRequestModels request)
        {
            var reps = new BaseResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(request.Slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                var car = await _context.Cars.Where(x => x.Slug == request.Slug).FirstOrDefaultAsync();
                if (car == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }

                if (string.IsNullOrEmpty(request.Name))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NAME_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.LicensePlate))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.VALUE_IS_EMPTY) { Message = "Biển số xe không được để trống" };
                    return new OkObjectResult(reps);
                }
                if (request.SeatCount <= 0)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.VALUES_IS_INVALID) { Message = "Số ghế không hợp lệ" };
                    return new OkObjectResult(reps);
                }

                // Check duplicate license plate excluding current car
                var dupPlate = await _context.Cars.AnyAsync(x => x.LicensePlate == request.LicensePlate.Trim() && x.Id != car.Id && x.IsEnable == 1);
                if (dupPlate)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.FAILED) { Message = "Biển số xe đã tồn tại trong hệ thống" };
                    return new OkObjectResult(reps);
                }

                car.Name = request.Name;
                car.LicensePlate = request.LicensePlate.Trim();
                car.SeatCount = request.SeatCount;
                car.Brand = request.Brand;
                car.Color = request.Color;
                car.ManufactureYear = request.ManufactureYear;
                car.Description = request.Description;
                car.ThumbNail = request.ThumbNail;
                car.UpdatedAt = DateTime.Now;

                // -- Routes
                if (request.Routes != null && request.Routes.Any())
                {
                    var existingRoutes = await _context.CarRoute.Where(x => x.SlugCar == car.Slug).ToListAsync();
                    var incomingRoutes = request.Routes.ToList();
                    var incomingSlugs = incomingRoutes.Select(x => x.Slug).ToHashSet();

                    foreach (var item in incomingRoutes)
                    {
                        var exist = existingRoutes.FirstOrDefault(x => x.SlugLocations == item.Slug);
                        if (exist != null)
                        {
                            exist.DisplayOrder = item.DisplayOrder;
                            exist.IsEnable = 1;
                            _context.CarRoute.Update(exist);
                        }
                        else
                        {
                            var loc = await _context.Locations.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == item.Slug);
                            if (loc != null)
                            {
                                var cr = new CarRoute()
                                {
                                    SlugCar = car.Slug,
                                    SlugLocations = loc.Slug,
                                    DisplayOrder = item.DisplayOrder,
                                    IsEnable = 1
                                };
                                await _context.CarRoute.AddAsync(cr);
                            }
                        }
                    }

                    var toDisableRoutes = existingRoutes.Where(x => !incomingSlugs.Contains(x.SlugLocations)).ToList();
                    foreach (var dr in toDisableRoutes)
                    {
                        dr.IsEnable = 0;
                    }
                    if (toDisableRoutes.Any())
                    {
                        _context.CarRoute.UpdateRange(toDisableRoutes);
                    }
                }
                else
                {
                    var allExistingRoutes = await _context.CarRoute.Where(x => x.SlugCar == car.Slug && x.IsEnable == 1).ToListAsync();
                    if (allExistingRoutes.Any())
                    {
                        allExistingRoutes.ForEach(x => x.IsEnable = 0);
                        _context.CarRoute.UpdateRange(allExistingRoutes);
                    }
                }

                _context.Cars.Update(car);
                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Car Update Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }

        [Authorize]
        [HttpPost("delete/{slug}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "Success")]
        public async Task<IActionResult> Delete([FromRoute] string slug)
        {
            var reps = new BaseResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                var car = await _context.Cars.Where(x => x.Slug == slug).FirstOrDefaultAsync();
                if (car == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }
                car.IsEnable = 0;

                var routes = await _context.CarRoute.Where(x => x.SlugCar == car.Slug && x.IsEnable == 1).ToListAsync();
                if (routes.Any())
                {
                    routes.ForEach(x => x.IsEnable = 0);
                    _context.CarRoute.UpdateRange(routes);
                }

                _context.Cars.Update(car);
                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Car Delete Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }

        [Authorize]
        [HttpPost("get")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<GetCarManagerRespones>>), description: "Success")]
        public async Task<IActionResult> Get([FromBody] GetCarManagerRequestModel request)
        {
            var reps = new BaseResponseMessage<BaseResponseMessagePage<GetCarManagerRespones>>();
            try
            {
                var query = await _context.Cars.AsNoTracking()
                    .Where(x => x.IsEnable == 1)
                    .Where(x => string.IsNullOrEmpty(request.Keyword) || 
                                EF.Functions.Like(x.Name.Trim(), $"%{request.Keyword.Trim()}%") ||
                                EF.Functions.Like(x.Brand.Trim(), $"%{request.Keyword.Trim()}%") ||
                                EF.Functions.Like(x.LicensePlate.Trim(), $"%{request.Keyword.Trim()}%"))
                    .Where(x => !request.SeatCount.HasValue || request.SeatCount.Value == x.SeatCount)
                    .Select(x => new GetCarManagerRespones
                    {
                        Name = x.Name,
                        Slug = x.Slug,
                        LicensePlate = x.LicensePlate,
                        SeatCount = x.SeatCount,
                        Brand = x.Brand,
                        Color = x.Color,
                        ManufactureYear = x.ManufactureYear,
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

        [Authorize]
        [HttpGet("detail/{slug}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<DetailCarResponeModels>), description: "Success")]
        public async Task<IActionResult> GetDetail([FromRoute] string slug)
        {
            var reps = new BaseResponseMessage<DetailCarResponeModels>();
            try
            {
                if (string.IsNullOrEmpty(slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }

                var car = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug);
                if (car is null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }
                var routes = await _context.CarRoute.AsNoTracking()
                    .Where(x => x.SlugCar == car.Slug && x.IsEnable == 1)
                    .Include(x => x.SlugLocationsNavigation)
                    .Select(r => new CarRouteResponseObject
                    {
                        Name = r.SlugLocationsNavigation.Name,
                        Slug = r.SlugLocations,
                        DisplayOrder = r.DisplayOrder
                    }).OrderBy(r => r.DisplayOrder).ToListAsync();

                reps.Data = new()
                {
                    Name = car.Name,
                    Slug = car.Slug,
                    LicensePlate = car.LicensePlate,
                    SeatCount = car.SeatCount,
                    Brand = car.Brand,
                    Color = car.Color,
                    ManufactureYear = car.ManufactureYear,
                    Description = car.Description,
                    ThumbNail = car.ThumbNail,
                    CreatedAt = car.CreatedAt,
                    Routes = routes
                };

                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API GetDetail Cars Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }
    }
}
