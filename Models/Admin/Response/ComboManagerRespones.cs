namespace TravelMasterApi.Models.Admin.Response
{
    public class GetComboManagerRespones
    {

        public string? Name { get; set; }

        public string? Thumbnail { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Slug { get; set; } = null!;

        public ulong IsHot { get; set; }

        public string? Departure { get; set; }

        public double? Ranking { get; set; }

        public int NumberDays { get; set; }
        public int NumberNights { get; set; }
        public double? OriginalPrice { get; set; }

        public string? Categories { get; set; }

        public string? Destination { get; set; }

        public double? SalePrice { get; set; }


    }
    public class DetailComboResponeModel
    {
        internal string? Uuid { get; set; }
        public string? Name { get; set; }

        public string? Thumbnail { get; set; }

        public string Slug { get; set; } = null!;

        public ulong IsHot { get; set; }

        public string? SlugDeparture { get; set; }
        public string? Departure { get; set; }

        public double? Ranking { get; set; }

        public int NumberDays { get; set; }

        public int NumberNights { get; set; }

        public double? OriginalPrice { get; set; }

        public long? CategoryId { get; set; }
        public string? Categories { get; set; }

        public string? Destination { get; set; }
        public string? SlugDestination { get; set; }
        public string? Description { get; set; }

        public double? SalePrice { get; set; }
        public string? Includes { get; set; }
        public string? Introduce { get; set; }
        public List<string>? Images { get; set; }
    }
}
