using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class Hotels
{
    public long Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Introduce { get; set; } = null!;

    public sbyte Type { get; set; }

    public double Ranking { get; set; }

    public DateTime CreatedAt { get; set; }

    public ulong IsEnable { get; set; }

    public double? RelativePrice { get; set; }

    public string Slug { get; set; } = null!;

    public string Thumbnail { get; set; } = null!;

    public string? Regulations { get; set; }

    public string SlugLocations { get; set; } = null!;

    public string? Address { get; set; }

    /// <summary>
    /// lưu tạm khi có giảm giá để query không cần tính , xóa khi voucher hết hạnx
    /// </summary>
    public double? SalePrice { get; set; }

    public ulong? IsHot { get; set; }

    public string? Description { get; set; }

    public virtual Locations SlugLocationsNavigation { get; set; } = null!;
}
