using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class TourDestination
{
    public long Id { get; set; }

    public ulong IsEnable { get; set; }

    public int DisplayOrder { get; set; }

    public string TourSlug { get; set; } = null!;

    public string LocationSlug { get; set; } = null!;

    public virtual Locations LocationSlugNavigation { get; set; } = null!;

    public virtual Tours TourSlugNavigation { get; set; } = null!;
}
