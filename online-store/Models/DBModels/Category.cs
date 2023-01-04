using System;
using System.Collections.Generic;

namespace online_store.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
