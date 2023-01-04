using System;
using System.Collections.Generic;

namespace online_store.Models;

public partial class OrderProduct
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? Quantity { get; set; }

    public int? ProductId { get; set; }

    public decimal? PricePerItem { get; set; }

    public decimal? Price { get; set; }

    public virtual Order? Order { get; set; }
}
