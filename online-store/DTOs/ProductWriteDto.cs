using System.ComponentModel.DataAnnotations;

namespace online_store.DTOs
{
    public class ProductWriteDto
    {
       
        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
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
