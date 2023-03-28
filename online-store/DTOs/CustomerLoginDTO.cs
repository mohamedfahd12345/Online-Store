using System.ComponentModel.DataAnnotations;

namespace online_store.DTOs
{
    public class CustomerLoginDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; init; }
        [Required]
        public string? Password { get; init; }
    }
}
