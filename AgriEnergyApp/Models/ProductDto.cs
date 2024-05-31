using System.ComponentModel.DataAnnotations;

namespace AgriEnergyApp.Models
{
    public class ProductDto
    {
        [MaxLength(100), Required]
        public string Name { get; set; } = "";

        [MaxLength(100), Required]
        public string Price { get; set; } = "";

        [MaxLength(100), Required]
        public string Category { get; set; } = "";

        public IFormFile? ImageFile { get; set; }

        [MaxLength(100), Required]
        public string Availability { get; set; } = "";
    }
}
