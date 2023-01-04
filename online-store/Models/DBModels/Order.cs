using System;
using System.Collections.Generic;

namespace online_store.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public decimal? TotatlPrice { get; set; }

    public int? AdderssId { get; set; }

    public string? OrderStatus { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? ShippedDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public decimal? DeliveryCost { get; set; }

    public decimal? TotatlPriceForProducts { get; set; }

    public virtual Address? Adderss { get; set; }

    public virtual User? Customer { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();
}
