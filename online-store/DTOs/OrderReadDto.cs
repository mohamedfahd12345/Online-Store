namespace online_store.DTOs;

public class OrderReadDto
{
    public int OrderId { get; set; }
    public string? OrderStatus { get; set; }
    public DateTime? OrderDate { get; set; }
}
