using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class Locations
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public ulong IsEnable { get; set; }

    public string Slug { get; set; } = null!;

    public string? Images { get; set; }

    public virtual ICollection<CarRoute> CarRoute { get; set; } = new List<CarRoute>();

    public virtual ICollection<Hotels> Hotels { get; set; } = new List<Hotels>();

    public virtual ICollection<TourDestination> TourDestination { get; set; } = new List<TourDestination>();

    public virtual ICollection<Tours> Tours { get; set; } = new List<Tours>();
}
