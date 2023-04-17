using System.ComponentModel.DataAnnotations;

namespace online_store.DTOs
{
    public class ProductWriteDto
    {
       
        [Required]
        public string? Description { get; set; }

        [Required]
        [Range(1, int.MaxValue , ErrorMessage = "The price must be at least 1.")]
        public decimal? Price { get; set; }

        [Required]
        [Range(1, int.MaxValue , ErrorMessage = "The quantity must be at least 1.")]
        public int? Quantity { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        [Required]
        public string? MainImageUrl { get; set; }

        [Required]
        public string? ProductName { get; set; }

        public List<string> imagesUrl { get; set; } = new List<string>();


    }
}
