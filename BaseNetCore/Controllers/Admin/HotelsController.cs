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

    public class HotelsController : ControllerBase
    {
        private readonly MasterDBContext _context;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(MasterDBContext context, ILogger<HotelsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("create")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "Success")]
        public async Task<IActionResult> Create([FromBody] CreateHotelManagerRequestModels request)
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
                if (request.Name.Length < 10 || request.Name.Length > 255)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NAME_LENGTH_IS_INVALID);
                    return new OkObjectResult(reps);
                }
                if (!request.Type.HasValue)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.TYPE_IS_INVALID);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.Introduce))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.INTRODUCE_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.SlugLocations))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.LOCATIONS_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.Thumbnail))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.THUMBNAIL_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (request.RelativePrice < 0)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.PRICE_IS_INVALID);
                    return new OkObjectResult(reps);
                }

                var slug = Util.GenerateSlug(request.Name);
                var checkSlug = await _context.Hotels.Where(x => x.Slug == slug).FirstOrDefaultAsync();
                if (checkSlug != null)
                {
                    slug = $"{slug}-{Util.GenerateRandomNumber()}";
                }

                var hotel = new Hotels()
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    Introduce = request.Introduce,
                    Type = request.Type.Value,
                    IsHot = request.IsHot ?? 0,
                    Ranking = request.Ranking,
                    RelativePrice = request.RelativePrice,
                    Thumbnail = request.Thumbnail,
                    Regulations = request.Regulations,
                    SlugLocations = request.SlugLocations,
                    Description = request.Description,
                    Address = request.Address,
                    Slug = slug,
                };
                await _context.Hotels.AddAsync(hotel);

                // xử lý ảnh
                if (request.ImagesUrl != null && request.ImagesUrl.Any())
                {
                    var lstImages = await _context.Images.Where(x => request.ImagesUrl.Contains(x.Path)).ToListAsync();
                    if (lstImages != null && lstImages.Any())
                    {
                        lstImages.ForEach(x =>
                        {
                            x.OwnerUuid = hotel.Uuid;
                            x.IsEnable = 1;
                        });
                        _context.Images.UpdateRange(lstImages);
                    }
                }

                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Hotel Create Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }

        [Authorize]
        [HttpPost("update")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "Success")]
        public async Task<IActionResult> Update([FromBody] UpdateHotelManagerRequestModels request)
        {
            var reps = new BaseResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(request.Slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                var hotel = await _context.Hotels.Where(x => x.Slug == request.Slug).FirstOrDefaultAsync();
                if (hotel == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }

                if (string.IsNullOrEmpty(request.Name))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NAME_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (request.RelativePrice < 0)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.PRICE_IS_INVALID);
                    return new OkObjectResult(reps);
                }
                if (!request.Type.HasValue)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.TYPE_IS_INVALID);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.Introduce))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.INTRODUCE_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.SlugLocations))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.LOCATIONS_IS_EMPTY);
                    return new OkObjectResult(reps);
                }
                if (string.IsNullOrEmpty(request.Thumbnail))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.THUMBNAIL_IS_EMPTY);
                    return new OkObjectResult(reps);
                }

                hotel.Name = request.Name;
                hotel.Introduce = request.Introduce;
                hotel.Type = request.Type.Value;
                hotel.Ranking = request.Ranking;
                hotel.RelativePrice = request.RelativePrice;
                hotel.Thumbnail = request.Thumbnail;
                hotel.Regulations = request.Regulations;
                hotel.SlugLocations = request.SlugLocations;
                hotel.Description = request.Description;
                hotel.Address = request.Address;
                hotel.IsHot = request.IsHot ?? 0;

                // -- Images
                if (request.ImagesUrl != null && request.ImagesUrl.Any())
                {
                    var requestedPaths = request.ImagesUrl.ToList();
                    var imagesByPath = await _context.Images.Where(x => requestedPaths.Contains(x.Path)).ToListAsync();
                    var existingImages = await _context.Images.Where(x => x.OwnerUuid == hotel.Uuid).ToListAsync();
                    foreach (var img in imagesByPath)
                    {
                        img.OwnerUuid = hotel.Uuid;
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
                    var allExistingImages = await _context.Images.Where(x => x.OwnerUuid == hotel.Uuid && x.IsEnable == 1).ToListAsync();
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

                _context.Hotels.Update(hotel);
                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Hotel Update Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
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
                var hotel = await _context.Hotels.Where(x => x.Slug == slug).FirstOrDefaultAsync();
                if (hotel == null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }
                hotel.IsEnable = 0;

                var images = await _context.Images.Where(x => x.OwnerUuid == hotel.Uuid && x.IsEnable == 1).ToListAsync();
                if (images.Any())
                {
                    images.ForEach(x =>
                    {
                        x.IsEnable = 0;
                        x.OwnerUuid = null;
                    });
                    _context.Images.UpdateRange(images);
                }

                _context.Hotels.Update(hotel);
                await _context.SaveChangesAsync();
                return new OkObjectResult(reps);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Hotel Delete Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }

        [Authorize]
        [HttpPost("get")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<GetHotelManagerRespones>>), description: "Success")]
        public async Task<IActionResult> Get([FromBody] GetHotelManagerRequestModel request)
        {
            var reps = new BaseResponseMessage<BaseResponseMessagePage<GetHotelManagerRespones>>();
            try
            {
                var query = await _context.Hotels.AsNoTracking().Include(x => x.SlugLocationsNavigation)
                    .Where(x => x.IsEnable == 1)
                    .Where(x => string.IsNullOrEmpty(request.Keyword) || EF.Functions.Like(x.Name.Trim(), $"%{request.Keyword.Trim()}%"))
                    .Where(x => !request.Ranking.HasValue || request.Ranking.Value == x.Ranking)
                    .Select(x => new GetHotelManagerRespones
                    {
                        Slug = x.Slug,
                        Name = x.Name,
                        CreatedAt = x.CreatedAt,
                        Ranking = $"{x.Ranking}",
                        Thumbnail = x.Thumbnail,
                        IsHot = x.IsHot,
                        RelativePrice = x.RelativePrice,
                        Type = x.Type,
                        Locations = x.SlugLocationsNavigation.Name,
                        Address = x.Address,
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

        [Authorize]
        [HttpGet("detail/{slug}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<DetailHotelResponeModels>), description: "Success")]
        public async Task<IActionResult> GetDetail([FromRoute] string slug)
        {
            var reps = new BaseResponseMessage<DetailHotelResponeModels>();
            try
            {
                if (string.IsNullOrEmpty(slug))
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.SLUG_IS_EMPTY);
                    return new OkObjectResult(reps);
                }

                var hotel = await _context.Hotels.AsNoTracking().Include(x => x.SlugLocationsNavigation).FirstOrDefaultAsync(x => x.Slug == slug);
                if (hotel is null)
                {
                    reps.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(reps);
                }
                var lstImages = await _context.Images.AsNoTracking().Where(x => x.OwnerUuid == hotel.Uuid && x.IsEnable == 1).Select(x => x.Path).ToListAsync();

                reps.Data = new()
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
                _logger.LogError($"API GetDetail Hotels Error Message: {ex.Message}\n DatetimeUtc:{DateTime.UtcNow}\n StraceTrace:{ex.StackTrace} ");
                reps.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(reps);
            }
        }
    }
}
