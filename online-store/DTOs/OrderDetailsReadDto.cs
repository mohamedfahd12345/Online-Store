namespace online_store.DTOs;

public class OrderDetailsReadDto
{
    public int OrderId { get; set; }

    public string? OrderStatus { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? PhoneNumber { get; set; }

    public decimal? TotalAmount { get; set; }
    public decimal? ShippingCost { get; set; }

    public DateTime? ShippedDate { get; set; }


    public DateTime? DeliveredDate { get; set; }


}
