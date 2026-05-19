using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class CarRoute
{
    public long Id { get; set; }

    public string SlugCar { get; set; } = null!;

    public string SlugLocations { get; set; } = null!;

    public ulong IsEnable { get; set; }

    public int DisplayOrder { get; set; }

    public virtual Cars SlugCarNavigation { get; set; } = null!;

    public virtual Locations SlugLocationsNavigation { get; set; } = null!;
}
