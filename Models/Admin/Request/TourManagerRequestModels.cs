using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.Admin.Request
{
    public class CreateTourManagerRequestModels
    {
        public string? Title { get; set; }
        public string? Introduce { get; set; }
        public sbyte? Type { get; set; }
        public string? Departure { get; set; }
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public List<string>? ImagesUrl { get; set; }
        public int? DurationDays { get; set; }
        public int? DurationNights { get; set; }
        public double? SalePrices { get; set; }
        public int Ranking { get; set; } = 5;
        public double? OriginalPrices { get; set; }
        public ulong? IsHot { get; set; }
        public List<DestinationRequestModels>? Destinations { get; set; }

    }

    public class DestinationRequestModels
    {
        public string Slug { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
    public class UpdateTourManagerRequestModels
    {
        public int Ranking { get; set; } = 5;
        public ulong IsHot { get; set; } = 0;
        public string Slug { get; set; } = string.Empty;
        public string? Title { get; set; }
        public int DurationDays { get; set; } = 0;
        public int DurationNights { get; set; } = 0;
        public string? Introduce { get; set; }
        public string? Description { get; set; }
        public sbyte? Type { get; set; }
        public string? Departure { get; set; }
        public string? Thumbnail { get; set; }
        public double? SalePrices { get; set; }
        public double? OriginalPrices { get; set; }
        public List<string>? ImagesUrl { get; set; }
        public List<UpdateTourDestinationRequestModels>? Destinations { get; set; }
    }

    public class UpdateTourDestinationRequestModels
    {
        public string Slug { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
    }
    public class GetTourManagerRequestModel : BaseRequestMessageKeyword
    {
        public int? Ranking { get; set; }
    }
    public class GetLocationRequestModels : BaseRequestMessageKeyword
    {

    }
    public class GetTourRequestModelForClient : BaseRequestMessage
    {
        public ulong? IsHot { get; set; }
        public int? Ranking { get; set; }
        public List<string>? Departures { get; set; }
        public List<string>? Destinations { get; set; }

    }
}
