using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.Admin.Request
{
    public class CreateHotelManagerRequestModels
    {
        public string? Name { get; set; }
        public string? Introduce { get; set; }
        public sbyte? Type { get; set; }
        public ulong? IsHot { get; set; }
        public double Ranking { get; set; } = 5.0;
        public double? RelativePrice { get; set; }
        public string? Thumbnail { get; set; }
        public string? Regulations { get; set; }
        public string? SlugLocations { get; set; }
        public string? Description { get; set; }
        public List<string>? SlugTopic { get; set; }
        public List<string>? SlugNearbyHotel { get; set; }
        public string? Address { get; set; }
        public List<string>? ImagesUrl { get; set; }
    }

    public class UpdateHotelManagerRequestModels
    {
        public string Slug { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Introduce { get; set; }
        public sbyte? Type { get; set; }
        public double Ranking { get; set; } = 5.0;
        public double? RelativePrice { get; set; }
        public string? Thumbnail { get; set; }
        public string? Regulations { get; set; }
        public string? SlugLocations { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public List<string>? ImagesUrl { get; set; }
        public List<string>? SlugTopic { get; set; }

        /// <summary>
        /// địa điểm ở gần
        /// </summary>
        public List<string>? SlugNearbyHotel { get; set; }
        public ulong? IsHot { get; set; }
    }

    public class GetHotelManagerRequestModel : BaseRequestMessageKeyword
    {
        public int? Ranking { get; set; }
    }
}
