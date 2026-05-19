using System;
using System.Collections.Generic;

namespace TravelMasterApi.Models.Admin.Response
{
    public class GetCarManagerRespones
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string LicensePlate { get; set; } = null!;
        public int SeatCount { get; set; }
        public string? Brand { get; set; }
        public string? Color { get; set; }
        public int? ManufactureYear { get; set; }
        public string? ThumbNail { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CarRouteResponseObject>? Routes { get; set; }
    }

    public class DetailCarResponeModels
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string LicensePlate { get; set; } = null!;
        public int SeatCount { get; set; }
        public string? Brand { get; set; }
        public string? Color { get; set; }
        public int? ManufactureYear { get; set; }
        public string? Description { get; set; }
        public string? ThumbNail { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CarRouteResponseObject>? Routes { get; set; }
    }

    public class CarRouteResponseObject
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
}
