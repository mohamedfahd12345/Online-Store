using System.ComponentModel.DataAnnotations;

namespace online_store.DTOs;

public class CheckoutDto
{
    [Required]
    public string? StreetNumber { get; set; }

    [Required]
    public string? StreetAddress { get; set; }

    [Required]
    public string? City { get; set; }

    [Required]
    public string? ZipCode { get; set; }

    [Required]
    public string? HomeNumber { get; set; }

    [Required]
    public string? ApartmentNumber { get; set; }

    [Required]
    public string? paymentMethod { get; set; }


}
