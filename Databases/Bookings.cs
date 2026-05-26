using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class Bookings
{
    public long Id { get; set; }

    public string Uuid { get; set; } = null!;

    /// <summary>
    /// 1: Fixed Tours , 2: Custom Tours, != tour = 0
    /// </summary>
    public sbyte Type { get; set; }

    /// <summary>
    /// PendingProcessing = 1,
    /// Processed = 2,
    /// Cancelled = 3,
    /// </summary>
    public sbyte State { get; set; }

    public DateTime CreatedAt { get; set; }

    public ulong IsEnable { get; set; }

    public long CategoryId { get; set; }

    public double? TotalPrice { get; set; }

    /// <summary>
    /// tour  theo yêu cầu thì không có slug
    /// </summary>
    public string? SlugOwner { get; set; }

    public virtual ICollection<BookingDetail> BookingDetail { get; set; } = new List<BookingDetail>();

    public virtual Categories Category { get; set; } = null!;
}
