using System.Collections.Generic;
using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.Admin.Request
{
    public class CreateComboManagerRequestModels
    {
        public string? Name { get; set; }
        public string? Introduce { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public string? SlugDeparture { get; set; }
        public string? SlugDestination { get; set; }
        public double Ranking { get; set; } = 5.0;
        public int NumberDays { get; set; }
        public int NumberNights { get; set; }
        public double OriginalPrice { get; set; }
        public double? SalePrice { get; set; }
        public long CategoryId { get; set; }
        public string? Includes { get; set; }
        public ulong? IsHot { get; set; }
        public List<string>? ImagesUrl { get; set; }
    }

    public class UpdateComboManagerRequestModels
    {
        public string? Description { get; set; }

        public string Slug { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Introduce { get; set; }
        public string? Thumbnail { get; set; }
        public string? SlugDeparture { get; set; }
        public string? SlugDestination { get; set; }
        public double Ranking { get; set; } = 5.0;
        public int NumberDays { get; set; }
        public int NumberNights { get; set; }
        public double? OriginalPrice { get; set; }
        public double? SalePrice { get; set; }
        public long CategoryId { get; set; }
        public string? Includes { get; set; }
        public ulong? IsHot { get; set; }
        public List<string>? ImagesUrl { get; set; }
    }
    public class GetComboManagerRequestModel : BaseRequestMessageKeyword
    {
        public int? Ranking { get; set; }
    }
}
