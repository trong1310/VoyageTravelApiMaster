using System.Collections.Generic;
using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.Admin.Request
{
    public class CreateCruisesManagerRequestModels
    {
        public string? Name { get; set; }
        public string? Introduce { get; set; }
        /// <summary>
        /// 1 ngày , 2 đêm, 3 > nhiều hơn 1 ngày
        /// </summary>
        public sbyte? Type { get; set; }
        public ulong IsHot { get; set; } = 0;
        public double Ranking { get; set; } = 5.0;
        public double PricePerPerson { get; set; }
        public string? Thumbnail { get; set; }
        public string? SlugDeparture { get; set; }
        public string? SlugDestination { get; set; }
        public string? Description { get; set; }
        public List<string>? ImagesUrl { get; set; }
    }

    public class UpdateCruisesManagerRequestModels
    {
        public string? Description { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Introduce { get; set; }
        public sbyte? Type { get; set; }
        public double Ranking { get; set; } = 5.0;
        public double PricePerPerson { get; set; }
        public string? Thumbnail { get; set; }
        public string? SlugDeparture { get; set; }
        public string? SlugDestination { get; set; }
        public ulong IsHot { get; set; } = 0;
        public List<string>? ImagesUrl { get; set; }
    }
    public class GetCruisesManagerRequestModel : BaseRequestMessageKeyword
    {
        public int? Ranking { get; set; }
    }
    public class CreateTopicHotelRequestModels
    {
        public string? TopicName { get; set; }
        public string? Thumbnail { get; set; }
        public List<string>? SlugHotel { get; set; }
    }
    public class UpdateTopicHotelRequestModels : CreateTopicHotelRequestModels
    {
        public string? Slug { get; set; }
    }
}
