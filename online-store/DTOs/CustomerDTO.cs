using System.ComponentModel.DataAnnotations;

namespace online_store.DTOs
{
    public class CustomerDTO
    {
        
        [Required]
        public string? Username { get; set; }

        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }


        [Required]
        public string? Password { get; set; }
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


    }
}
