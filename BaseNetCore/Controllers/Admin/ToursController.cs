using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Base.Utils;
using TravelMasterApi.Databases;
using TravelMasterApi.Enums;
using TravelMasterApi.Extensions;
using TravelMasterApi.Settings;
using TravelMasterApi.Models.Admin.Request;
using TravelMasterApi.Models.Admin.Response;
using TravelMasterApi.Base.BaseExtension;

namespace TravelMasterApi.Controllers.Admin
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]

    public class ToursController : ControllerBase
    {
        private readonly MasterDBContext _context;

        private readonly ILogger<ToursController> _logger;

        public ToursController(MasterDBContext context, ILogger<ToursController> logger)
        {
            _context = context;
            _logger = logger;

        }
        [Authorize]
        /// <summary>
        /// thêm tour
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "Success full")]
        public async Task<IActionResult> Created([FromBody] CreateTourManagerRequestModels request)
        {
            var acc = User.GetUuid();
            if (acc == null)
            { return Unauthorized(); }
            var reps = new BaseResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(request.Title))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NAME_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (request.Title.Length < 10 || request.Title.Length > 255)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NAME_LENGTH_IS_INVALID);
                    return new OkObjectResult(reps);
                }
              
                if (!request.DurationNights.HasValue || request.DurationNights < 0)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.DURATION_NIGHT_IS_INVALID);
                    return new OkObjectResult(reps);
                }
                if (!request.DurationDays.HasValue || request.DurationDays < 0)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.DURATION_DAYS_IS_INVALID);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.Introduce))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.INTRODUCE_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.Departure))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.LOCATIONS_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.Thumbnail))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.THUMBNAIL_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (request.OriginalPrices < 0 || request.SalePrices < 0)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.PRICE_IS_INVALID);
                    return new OkObjectResult(reps);
                }
                var slug = Util.GenerateSlug(request.Title);
                var checkSlug = await _context.Tours.Where(x => x.Slug == slug).FirstOrDefaultAsync();
                if (checkSlug != null)
                {
                    slug = $"{slug}-{Util.GenerateRandomNumber()}";
                }
                var tour = new Tours()
                {
                    Thumbnail = request.Thumbnail,
                    DepartureSlug = request.Departure,
                    Introduce = request.Introduce,
                    CategoryId = (long)eCategories.Tours,
                    CreatedBy = acc,
                    Title = request.Title,
                    Slug = slug,
                    Uuid = Guid.NewGuid().ToString(),
                    DurationDays = request.DurationDays,
                    DurationNights = request.DurationNights,
                    IsHot = request.IsHot,
                    Description = request.Description,
                    OriginalPrices = request.OriginalPrices,
                    SalePrices = request.SalePrices,
                    Ranking = request.Ranking,
                };
                await _context.Tours.AddAsync(tour);

                // xử lý ảnh 
                if (request.ImagesUrl != null && request.ImagesUrl.Any())
                {
                    var lstImages = await _context.Images.Where(x => request.ImagesUrl.Contains(x.Path)).ToListAsync();
                    if (lstImages != null && lstImages.Any())
                    {
                        lstImages.ForEach(x =>
                        {
                            x.OwnerUuid = tour.Uuid;
                            x.IsEnable = 1;
                        });
                        _context.Images.UpdateRange(lstImages);
                    }
                }
                // xử lý điểm đến
                if (request.Destinations != null && request.Destinations.Any())
                {
                    var lstAdd = new List<TourDestination>();
                    foreach (var item in request.Destinations)
                    {
                        var locations = await _context.Locations.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == item.Slug);
                        if (locations != null)
                        {
                            var add = new TourDestination()
                            {
                                LocationSlug = locations.Slug,
                                DisplayOrder = item.DisplayOrder,
                                IsEnable = 1,
                                TourSlug = tour.Slug,
                            };
                            lstAdd.Add(add);
                        }

                    }
                    await _context.TourDestination.AddRangeAsync(lstAdd);
                }
                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Booking Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }
        [HttpPost("update")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "Success full")]
        public async Task<IActionResult> Updated([FromBody] UpdateTourManagerRequestModels request)
        {
            var reps = new BaseResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(request.Slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                var tour = await _context.Tours.Where(x => x.Slug == request.Slug).FirstOrDefaultAsync();
                if (tour == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }

                if (string.IsNullOrEmpty(request.Title))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NAME_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (request.OriginalPrices < 0 || request.SalePrices < 0)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.PRICE_IS_INVALID);
                    return new OkObjectResult(reps);
                }

                if (string.IsNullOrEmpty(request.Introduce))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.INTRODUCE_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.Departure))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.LOCATIONS_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.Thumbnail))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.THUMBNAIL_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                // cập nhật thông tin tour
                tour.Slug = request.Slug;
                tour.Title = request.Title;
                tour.Introduce = request.Introduce;
                tour.Description = request.Description;
                tour.DepartureSlug = request.Departure;
                tour.Thumbnail = request.Thumbnail;
                tour.DurationDays = request.DurationDays;
                tour.DurationNights = request.DurationNights;
                tour.IsHot = request.IsHot;
                tour.Ranking = request.Ranking;
                tour.OriginalPrices = request.OriginalPrices;
                tour.SalePrices = request.SalePrices;
                // -- Images
                if (request.ImagesUrl != null && request.ImagesUrl.Any())
                {
                    var requestedPaths = request.ImagesUrl.ToList();
                    var imagesByPath = await _context.Images.Where(x => requestedPaths.Contains(x.Path)).ToListAsync();
                    var existingImages = await _context.Images.Where(x => x.OwnerUuid == tour.Uuid).ToListAsync();
                    foreach (var img in imagesByPath)
                    {
                        img.OwnerUuid = tour.Uuid;
                        img.IsEnable = 1;
                    }
                    var toDisableImages = existingImages.Where(x => !requestedPaths.Contains(x.Path)).ToList();
                    foreach (var di in toDisableImages)
                    {
                        di.IsEnable = 0;
                        di.OwnerUuid = null;
                    }

                    if (imagesByPath.Any())
                        _context.Images.UpdateRange(imagesByPath);
                    if (toDisableImages.Any())
                        _context.Images.UpdateRange(toDisableImages);
                }
                else
                {
                    var allExistingImages = await _context.Images.Where(x => x.OwnerUuid == tour.Uuid && x.IsEnable == 1).ToListAsync();
                    if (allExistingImages.Any())
                    {
                        allExistingImages.ForEach(x =>
                        {
                            x.IsEnable = 0;
                            x.OwnerUuid = null;
                        });
                        _context.Images.UpdateRange(allExistingImages);
                    }
                }

                // -- Destinations
                if (request.Destinations != null && request.Destinations.Any())
                {
                    var existingDestinations = await _context.TourDestination
                        .Where(d => d.TourSlug == tour.Slug)
                        .ToListAsync();

                    var incomingDestinations = request.Destinations.ToList();
                    var incomingLocUuids = incomingDestinations.Select(d => d.Slug).ToHashSet();

                    foreach (var item in incomingDestinations)
                    {
                        var exist = existingDestinations.FirstOrDefault(d => d.LocationSlug == item.Slug);
                        if (exist != null)
                        {
                            exist.DisplayOrder = item.DisplayOrder;
                            exist.IsEnable = 1;
                            _context.TourDestination.Update(exist);
                        }
                        else
                        {
                            var locations = await _context.Locations.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == item.Slug);
                            if (locations != null)
                            {
                                var add = new TourDestination()
                                {
                                    TourSlug = tour.Slug,
                                    LocationSlug = locations.Slug,
                                    DisplayOrder = item.DisplayOrder,
                                    IsEnable = 1,
                                };
                                await _context.TourDestination.AddAsync(add);
                            }

                        }
                    }
                    var toDisableDest = existingDestinations.Where(d => !incomingLocUuids.Contains(d.LocationSlug)).ToList();
                    foreach (var dd in toDisableDest)
                    {
                        dd.IsEnable = 0;
                    }
                    if (toDisableDest.Any())
                        _context.TourDestination.UpdateRange(toDisableDest);
                }
                else
                {
                    var allExistingDest = await _context.TourDestination.Where(d => d.TourSlug == tour.Slug && d.IsEnable == 1).ToListAsync();
                    if (allExistingDest.Any())
                    {
                        allExistingDest.ForEach(d => d.IsEnable = 0);
                        _context.TourDestination.UpdateRange(allExistingDest);
                    }
                }
                _context.Tours.Update(tour);
                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Booking Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }
        [HttpPost("delete/{slug}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "Success full")]
        public async Task<IActionResult> Deleted([FromRoute] string slug)
        {
            var reps = new BaseResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                var tour = await _context.Tours.Where(x => x.Slug == slug).FirstOrDefaultAsync();
                if (tour == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }
                tour.IsEnable = 0;

                var images = await _context.Images.Where(x => x.OwnerUuid == tour.Uuid && x.IsEnable == 1).ToListAsync();
                if (images.Any())
                {
                    images.ForEach(x =>
                    {
                        x.IsEnable = 0;
                        x.OwnerUuid = null;
                    });
                    _context.Images.UpdateRange(images);
                }
                var destinations = await _context.TourDestination.Where(x => x.TourSlug == tour.Slug && x.IsEnable == 1).ToListAsync();
                if (destinations.Any())
                {
                    destinations.ForEach(x => x.IsEnable = 0);
                    _context.TourDestination.UpdateRange(destinations);
                }
                //var vouchers = await _context.TourVoucher.Where(x => x.TourUuid == tour.Uuid && x.IsEnable == 1).ToListAsync();
                //if (vouchers.Any())
                //{
                //    vouchers.ForEach(x => x.IsEnable = 0);
                //    _context.TourVoucher.UpdateRange(vouchers);
                //}
                _context.Tours.Update(tour);
                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Booking Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }
        [HttpPost("get")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<GetTourResponeModels>>), description: "Success full")]
        public async Task<IActionResult> Get([FromBody] GetTourManagerRequestModel request)
        {
            var reps = new BaseResponseMessage<BaseResponseMessagePage<GetTourResponeModels>>();
            try
            {
                var query = await _context.Tours.AsNoTracking().Include(x => x.DepartureSlugNavigation)
                    .Where(x => x.IsEnable == 1)
                    .Where(x => string.IsNullOrEmpty(request.Keyword) || EF.Functions.Like(x.Title.Trim(), $"%{request.Keyword.Trim()}%"))
                    .Where(x => !request.Ranking.HasValue || request.Ranking.Value == x.Ranking)
                    .Select(x => new GetTourResponeModels
                    {
                        Slug = x.Slug,
                        Title = x.Title,
                        CreatedAt = x.CreatedAt,
                        Ranking = $"{x.Ranking}",
                        Thumbnail = x.Thumbnail,
                        IsHot = x.IsHot,
                        OriginalPrices = x.OriginalPrices,
                        SalePrices = x.SalePrices,
                        Departure = x.DepartureSlugNavigation.Name,
                        SlugDeparture = x.DepartureSlug,
                        DurationNights = x.DurationNights,
                        DurationDays = x.DurationDays,

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
        [HttpGet("detail/{slug}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<TourDetailResponeModels>), description: "Success full")]
        public async Task<IActionResult> GetDetail([FromRoute] string slug)
        {
            var reps = new BaseResponseMessage<TourDetailResponeModels>();
            try
            {
                if (string.IsNullOrEmpty(slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }

                var tour = await _context.Tours.AsNoTracking().Include(x => x.DepartureSlugNavigation).FirstOrDefaultAsync(x => x.Slug == slug);
                if (tour is null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }
                var lstImages = await _context.Images.AsNoTracking().Where(x => x.OwnerUuid == tour.Uuid && x.IsEnable == 1).Select(x => x.Path).ToListAsync();
                var lstLocations = await _context.TourDestination.AsNoTracking().Where(x => x.TourSlug == tour.Slug && x.IsEnable == 1)
                    .Select(a => new LocationObject
                    {
                        Name = a.LocationSlugNavigation.Name,
                        Slug = a.LocationSlug,
                    }).ToListAsync();
                reps.Data = new()
                {
                    Slug = tour.Slug,
                    Title = tour.Title,
                    DurationNights = tour.DurationNights,
                    DurationDays = tour.DurationDays,
                    Introduce = tour.Introduce,
                    Thumbnail = tour.Thumbnail,
                    Images = lstImages,
                    IsHot = tour.IsHot,
                    Ranking = tour.Ranking,
                    TourDestinations = lstLocations,
                    Departure = tour.DepartureSlugNavigation.Name,
                    SlugDeparture = tour.DepartureSlug,
                    Uuid = tour.Uuid,
                    OriginalPrices = tour.OriginalPrices,
                    SalePrices = tour.SalePrices,
                };

                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API GetDetail Hotels Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }
    }
}
