using System.ComponentModel.DataAnnotations;

namespace online_store.DTOs;

public class CheckoutDto
{
    
    [Required]
    public string? paymentMethod { get; set; }


    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }

    [Required]
    public string? StreetNumber { get; set; }
    [Required]
    public string? StreetAddress { get; set; }
    [Required]
    public string? City { get; set; }
    [Required]
    public string? ZipCode { get; set; }
    [Required]
    public string? Country { get; set; }
    [Required]
    public string? State { get; set; }
}
