using TravelMasterApi.Models.Admin.Response;

namespace TravelMasterApi.Models.Admin.Response
{
    public class TourTimeRespose
    {
        public string Uuid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int DurationDays { get; set; }
        public int DurationNights { get; set; }

    }
    public class GetTourResponeModels
    {
        public string Slug { get; set; } = null!;

        public string Title { get; set; } = null!;

        /// <summary>
        /// 0: chưa xác định 1: trong nước 2: nước ngoài
        /// </summary>
        public sbyte Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public double? OriginalPrices { get; set; }
        public double? SalePrices { get; set; }
        public string Thumbnail { get; set; } = null!;

        public ulong? IsHot { get; set; }

        public string Ranking { get; set; }

        /// <summary>
        /// điểm khởi hành
        /// </summary>
        public string? Departure { get; set; } = null!;
        public string? SlugDeparture { get; set; } = null!;
        public int? DurationDays { get; set; }
        public int? DurationNights { get; set; }
    }
    public class TourDetailResponeModels
    {
        public double? OriginalPrices { get; set; }
        public double? SalePrices { get; set; }
        internal string Uuid { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Introduce { get; set; }
        public string? Thumbnail { get; set; }
        public List<string>? Images { get; set; }
        public List<LocationObject>? TourDestinations { get; set; }
        //public List<DateTime>? DepartureSchedule { get; set; }
        public int? DurationDays { get; set; }
        public sbyte? Type{ get; set; }
        public int? DurationNights { get; set; }
        public ulong? IsHot { get; set; }
        /// <summary>
        /// điểm khởi hành
        /// </summary>
        public string? Departure { get; set; } = null!;
        public string? SlugDeparture { get; set; } = null!;
        public double? Ranking { get; set; } = 0;
    }
    public class LocationObject
    {
        public string? Slug { get; set; }
        public string? Name { get; set; } = string.Empty;
    }
}
