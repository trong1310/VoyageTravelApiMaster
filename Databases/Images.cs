using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class Images
{
    public long Id { get; set; }

    public string Path { get; set; } = null!;

    public string? OwnerUuid { get; set; }

    public string Uuid { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public ulong IsEnable { get; set; }
}
