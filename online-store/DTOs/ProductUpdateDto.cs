namespace online_store.DTOs;

public class ProductUpdateDto
{
    public string? Description { get; set; }


    public decimal? Price { get; set; }


    public int? Quantity { get; set; }


    public int? CategoryId { get; set; }

    public string? MainImageUrl { get; set; }

    public string? ProductName { get; set; }

    public int ProductId { get; set; }
    public List<string> imagesUrl { get; set; } = new List<string>();
}
