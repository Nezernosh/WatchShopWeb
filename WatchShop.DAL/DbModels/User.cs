using System;
using System.Collections.Generic;

namespace WatchShop.DAL.DbModels;

public partial class User
{
    public User()
    {
        OneTimePasswords = new HashSet<OneTimePassword>();
        Carts = new HashSet<Cart>();
    }

    public int Id { get; set; }

    public string? Login { get; set; }

    public virtual ICollection<Cart> Carts { get; set; }

    public virtual ICollection<OneTimePassword> OneTimePasswords { get; set; }
}
