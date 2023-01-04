using System;
using System.Collections.Generic;

namespace online_store.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? CustomerId { get; set; }

    public int? Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? ProductNavigation { get; set; }
}
