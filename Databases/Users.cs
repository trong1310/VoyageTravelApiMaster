using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class Users
{
    public long Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public ulong IsEnable { get; set; }

    public sbyte State { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<UserAuthentications> UserAuthentications { get; set; } = new List<UserAuthentications>();
}
