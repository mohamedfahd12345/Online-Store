using System;
using System.Collections.Generic;

namespace online_store.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public int? CategoryId { get; set; }

    public string? ImageUrl { get; set; }

    public string? ProductName { get; set; }

    public int? VendorId { get; set; }

    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    public virtual Category? Category { get; set; }

    public virtual User? Vendor { get; set; }
}
