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
}
