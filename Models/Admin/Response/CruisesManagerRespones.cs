namespace TravelMasterApi.Models.Admin.Response
{
    public class CruisesManagerRespones
    {
        public string? Name { get; set; } = null!;

        public string? Ranking { get; set; }

       // public string? Introduce { get; set; } = null!;

        public double? PricePerPerson { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Slug { get; set; } = null!;

        public string Thumbnail { get; set; } = null!;

        public string? Departure { get; set; }

        public string? Destination { get; set; }
        //public sbyte? Type { get; set; }
        public ulong? IsHot { get; set; }

    }
    public class CruisesDetailManagerRespone
    {
        public string? Name { get; set; } = null!;
        internal string? Uuid { get; set; } = null!;

        public string? Ranking { get; set; }

        public string? Introduce { get; set; } = null!;

        public double? PricePerPerson { get; set; }
        public string Slug { get; set; } = null!;

        public string Thumbnail { get; set; } = null!;

        public string? Departure { get; set; }
        public string? SlugDeparture { get; set; }

        public string? Destination { get; set; }
        public string? SlugDestination { get; set; }
        public sbyte? Type { get; set; }
        public List<string>? Images { get; set; } = null!;
        public ulong? IsHot { get; set; }


    }
}
