using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class ManagerAccounts
{
    public long Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public sbyte State { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ulong IsEnable { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }
}
