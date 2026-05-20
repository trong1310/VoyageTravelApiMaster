using System;
using System.Collections.Generic;
using TravelMasterApi.Models.Admin.Response;

namespace TravelMasterApi.Models.ResponseModel
{
    public class HotelClientResponseModels
    {
        public string? Name { get; set; }
        public sbyte? Type { get; set; }
        public string? Ranking { get; set; }
        public double? RelativePrice { get; set; }
        public string? Slug { get; set; }
        public string? Thumbnail { get; set; }
        public string? Locations { get; set; }
        public string? Address { get; set; }
        public ulong? IsHot { get; set; }
        internal DateTime? CreatedAt { get; set; }
    }

    public class DetailHotelClientResponseModels
    {
        public string? Name { get; set; }
        public sbyte? Type { get; set; }
        public string? Ranking { get; set; }
        public double? RelativePrice { get; set; }
        public string? Slug { get; set; }
        public string? Thumbnail { get; set; }
        public string? SlugLocations { get; set; }
        public string? Locations { get; set; }
        public string? Address { get; set; }
        public string? Introduce { get; set; }
        public string? Regulations { get; set; }
        public ulong? IsHot { get; set; }
        public List<string>? Images { get; set; }
        public List<LocationsObject>? HotelsNearby { get; set; }
        public List<LocationsObject>? TouristAttraction { get; set; }
        public List<LocationsObject>? Topic { get; set; }
    }
}
