using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Databases;
using TravelMasterApi.Models.Admin.Response;
using TravelMasterApi.SecurityManagers;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Controllers.Admin
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]

    public class UploadController : ControllerBase
    {

        private readonly MasterDBContext _context;
        private readonly ILogger<UploadController> _logger;

        public UploadController(MasterDBContext context, ILogger<UploadController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // [RequestSizeLimit(1024 * 1024 * 5)]
        [HttpPost]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessageItem<string>>), description: "Success")]

        public async Task<IActionResult> Upload([FromForm] List<IFormFile> files)
        {
            var respone = new BaseResponseMessage<BaseResponseMessageItem<string>>();
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

            if (!Directory.Exists(rootPath))
            {
                respone.error = new BaseResponseMessage.Error(ErrorCode.FOLDER_IMAGE_NOT_FOUND);
                return new OkObjectResult(respone);
            }
            long maxFileSize = 5 * 1024 * 1024;
            try
            {
                var newDatas = new List<Images>();
                var filesName = files.Select(x => x.FileName).ToList();

                // Lấy năm và tháng hiện tại
                var year = DateTime.UtcNow.Year.ToString();
                var month = DateTime.UtcNow.Month.ToString("D2");

                // Đường dẫn thư mục theo cấu trúc: Resources/{year}/{month}/images
                var folderPath = Path.Combine(rootPath, year, month);

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                foreach (var file in files)
                {
                    if (file.Length > 0 && file.Length <= maxFileSize)
                    {
                        var name = Guid.NewGuid().ToString();
                        var filePath = Path.Combine(folderPath, name + Path.GetExtension(file.FileName));
                        var filePathEx = Path.Combine("Resources", year, month, name + Path.GetExtension(file.FileName)).Replace("\\", "/");

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        newDatas.Add(new()
                        {
                            Uuid = Guid.NewGuid().ToString(),
                            //Path = Util.RemoveResourcesPath(filePathEx),
                            Path = filePathEx,
                            IsEnable = 1
                        });
                    }
                    else
                    {
                        respone.error = new BaseResponseMessage.Error(ErrorCode.FILE_TOO_LARGE);
                        return new OkObjectResult(respone);
                    }
                }

                await _context.Images.AddRangeAsync(newDatas);
                await _context.SaveChangesAsync();

                respone.Data = new()
                {
                    Items = newDatas.Select(x => x.Path).ToList(),
                };
                return new OkObjectResult(respone);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erorr: {ex.Message} : {ex.StackTrace}");
                respone.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(respone);
            }
        }

    }

}

