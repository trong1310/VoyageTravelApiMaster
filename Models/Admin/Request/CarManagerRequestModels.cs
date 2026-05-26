using System.Collections.Generic;
using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.Admin.Request
{
    public class CreateCarManagerRequestModels
    {
        public string Name { get; set; } = null!;
        public string LicensePlate { get; set; } = null!;
        public int SeatCount { get; set; }
        public string? Brand { get; set; }
        public string? Color { get; set; }
        public int? ManufactureYear { get; set; }
        public string? Description { get; set; }
        public string? ThumbNail { get; set; }
        public List<CarRouteRequestModels>? Routes { get; set; }
    }

    public class CarRouteRequestModels
    {
        public string Slug { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

    public class UpdateCarManagerRequestModels
    {
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = null!;
        public string LicensePlate { get; set; } = null!;
        public int SeatCount { get; set; }
        public string? Brand { get; set; }
        public string? Color { get; set; }
        public int? ManufactureYear { get; set; }
        public string? Description { get; set; }
        public string? ThumbNail { get; set; }
        public List<CarRouteRequestModels>? Routes { get; set; }
    }

    public class GetCarManagerRequestModel : BaseRequestMessageKeyword
    {
        public int? SeatCount { get; set; }
    }
}
