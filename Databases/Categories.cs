using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class Categories
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public ulong IsEnable { get; set; }

    public string Banner { get; set; } = null!;

    public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();

    public virtual ICollection<Tours> Tours { get; set; } = new List<Tours>();
}
