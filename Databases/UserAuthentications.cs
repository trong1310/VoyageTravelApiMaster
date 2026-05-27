using System;
using System.Collections.Generic;

namespace TravelMasterApi.Databases;

public partial class UserAuthentications
{
    public long Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string UserUuid { get; set; } = null!;

    /// <summary>
    /// 1: local,2: google,3: facebook
    /// </summary>
    public sbyte Provider { get; set; }

    public string ProviderId { get; set; } = null!;

    public string? PassWord { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Users UserUu { get; set; } = null!;
}
