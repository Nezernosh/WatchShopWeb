using System;
using System.Collections.Generic;

namespace WatchShop.DAL.DbModels;

public partial class Item
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Price { get; set; }

    public string? Picture { get; set; }

    public decimal? Rating { get; set; }

    public virtual ICollection<ItemsInCart> ItemsInCarts { get; set; } = new List<ItemsInCart>();
}
