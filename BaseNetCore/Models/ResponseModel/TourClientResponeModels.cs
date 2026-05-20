namespace TravelMasterApi.Models.ResponseModel
{
    public class TourClientResponeModels
    {
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public string Durations { get; set; } = string.Empty;
        public double? OriginalPrices { get; set; }
        public double? SalePrices { get; set; }
        internal DateTime? CreatedAt { get; set; }
    }
    public class DetailTourClientResponeModels
    {
        public string? Slug { get; set; } 
        public string? Title { get; set; } 
        public string? Thumbnail { get; set; } 
        public List<string>? Images { get; set; } 
        public int? DurationDays { get; set; } 
        public int? DurationNights { get; set; } 
        public string Introduce { get; set; } 
        public List<string> Destinations { get; set; }
        public double? OriginalPrices { get; set; }
        public double? SalePrices { get; set; }
    }
    public class BookingTourFixedRequestModels
    {
        public string? Slug { get; set; }
        public DateTime? StartTime { get; set; }
        public int TotalCustomer { get; set; } = 0;
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? SpecialRequirements { get; set; }

    }

}
