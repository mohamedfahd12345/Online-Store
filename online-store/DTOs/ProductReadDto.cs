using System.ComponentModel.DataAnnotations;
using online_store.Models;
namespace online_store.DTOs;

public class ProductsReadDto 
{

    public string? Description { get; set; }

    
    public decimal? Price { get; set; }

    
    public int? Quantity { get; set; }

   
    public int? CategoryId { get; set; }

    public string? MainImageUrl { get; set; }

    public string? ProductName { get; set; }

    public int? VendorId { get; set; } 
    public int ProductId { get; set; }
    public string? Category { get; set; }
    

    
      

}
