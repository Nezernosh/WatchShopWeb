using System;
using System.Collections.Generic;

namespace WatchShop.DAL.DbModels;

public partial class OneTimePassword
{
    public int Id { get; set; }

    public string? Password { get; set; }

    public int? UserId { get; set; }

    public bool? IsUsed { get; set; }

    public virtual User? User { get; set; }
}
