using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class BookingDetail
{
    public long Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string BookingUuid { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public int Adult { get; set; }

    public int Children { get; set; }

    public int Baby { get; set; }

    public string? YourName { get; set; }

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public string? SpecialRequirements { get; set; }

    public DateTime CreatedAt { get; set; }

    public ulong IsEnable { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual Bookings BookingUu { get; set; } = null!;
}
