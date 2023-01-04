namespace online_store.DTOs
{
    public class ProductReadDto : ProductWriteDto
    {
          public int? VendorId { get; set; } 
          public int ProductId { get; set; }
          public string? Category { get; set; }
        


    }
}
