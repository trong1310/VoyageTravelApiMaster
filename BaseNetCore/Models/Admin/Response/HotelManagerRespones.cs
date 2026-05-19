namespace TravelMasterApi.Models.Admin.Response
{
    public class GetHotelManagerRespones
    {
        public string? Name { get; set; } = null!;
        /// <summary>
        /// VinPearl = 1,
        /// Resort = 2,
        /// Villa = 3
        /// </summary>
        public sbyte? Type { get; set; }

        public string? Ranking { get; set; }

        public DateTime? CreatedAt { get; set; }

        public double? RelativePrice { get; set; }

        public string? Slug { get; set; } = null!;

        public string? Thumbnail { get; set; } = null!;

        public string? Locations { get; set; } = null!;

        public string? Address { get; set; }

        public ulong? IsHot { get; set; }
    }
    public class DetailHotelResponeModels
    {
        internal string Uuid { get; set; }
        public string? Name { get; set; } = null!;
        /// <summary>
        /// VinPearl = 1,
        /// Resort = 2,
        /// Villa = 3
        /// </summary>
        public sbyte? Type { get; set; }
        public string? Ranking { get; set; }
        public double? RelativePrice { get; set; }
        public string? Slug { get; set; } = null!;
        public string? Thumbnail { get; set; } = null!;
        public string? SlugLocations { get; set; } = null!;
        public string? Locations { get; set; } = null!;
        public string? Address { get; set; }
        public string? Introduce { get; set; }
        public string? Regulations { get; set; }
        public ulong? IsHot { get; set; }
        public List<string>? Images { get; set; }
        public List<LocationsObject>? HotelsNearby { get; set; }
        public List<LocationsObject>? TouristAttraction { get; set; }
        public List<LocationsObject>? Topic { get; set; }

    }
    public class LocationsObject
    {
        public string? Name { get; set; }
        public string? Slug { get; set; }
    }
    public class GetTopicHotelResponeModels
    {
        public string? Slug { get; set; }
        public string? Title { set; get; }
        public string? Thumbnail { set; get; }
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<GetTopicHotelItem>? Items { get; set; }
    }
     public class GetTopicHotelItem
    {
        public string? Slug { get; set; }
        internal string? SlugTopic { get; set; }
        public string? Name { set; get; }
    }
}
