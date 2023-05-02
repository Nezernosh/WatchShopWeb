using System;
using System.Collections.Generic;

namespace WatchShop.DAL.DbModels;

public partial class Cart
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? TotalPrice { get; set; }

    public virtual ICollection<ItemsInCart> ItemsInCarts { get; set; } = new List<ItemsInCart>();

    public virtual User? User { get; set; }
}
