using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class Tours
{
    public long Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public ulong IsEnable { get; set; }

    public string Introduce { get; set; } = null!;

    public string? Highlight { get; set; }

    public double? OriginalPrices { get; set; }

    public long CategoryId { get; set; }

    public string Thumbnail { get; set; } = null!;

    public ulong? IsHot { get; set; }

    public int Ranking { get; set; }

    /// <summary>
    /// điểm khởi hành
    /// </summary>
    public string DepartureSlug { get; set; } = null!;

    public int? DurationDays { get; set; }

    public int? DurationNights { get; set; }

    public string? Description { get; set; }

    public double? SalePrices { get; set; }

    public virtual Categories Category { get; set; } = null!;

    public virtual Locations DepartureSlugNavigation { get; set; } = null!;

    public virtual ICollection<TourDestination> TourDestination { get; set; } = new List<TourDestination>();
}
