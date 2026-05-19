using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class Cars
{
    /// <summary>
    /// id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ten xe
    /// </summary>
    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    /// <summary>
    /// bien so
    /// </summary>
    public string LicensePlate { get; set; } = null!;

    /// <summary>
    /// so ghe
    /// </summary>
    public int SeatCount { get; set; }

    /// <summary>
    /// hang xe
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// mau sac
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// nam san xuat
    /// </summary>
    public int? ManufactureYear { get; set; }

    /// <summary>
    /// mo ta
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// anh xe
    /// </summary>
    public string? ThumbNail { get; set; }

    /// <summary>
    /// trang thai
    /// </summary>
    public ulong IsEnable { get; set; }

    /// <summary>
    /// ngay tao
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// ngay cap nhat
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public ulong? IsHot { get; set; }

    public virtual ICollection<CarRoute> CarRoute { get; set; } = new List<CarRoute>();
}
