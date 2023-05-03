using System;
using System.Collections.Generic;

namespace online_store.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? AdderssId { get; set; }

    public string? OrderStatus { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public decimal? ShippingCost { get; set; }

    public DateTime? DeliveredDate { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual Address? Adderss { get; set; }

    public virtual User? Customer { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();
}
