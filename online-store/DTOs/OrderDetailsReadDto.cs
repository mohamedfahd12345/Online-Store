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


    //
    public int AddressId { get; set; }

    public string? StreetNumber { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public List<CartReadDto> orderProducts  { get; set; }  = new List<CartReadDto>() ;

}
