using System;
using System.Collections.Generic;

namespace WatchShop.DAL.DbModels;

public partial class ItemsInCart
{
    public int Id { get; set; }

    public int? CartId { get; set; }

    public int? ItemId { get; set; }

    public int? ItemAmount { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Item? Item { get; set; }
}
