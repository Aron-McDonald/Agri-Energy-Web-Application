using System.ComponentModel.DataAnnotations;

namespace AgriEnergyApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string Price { get; set; } = "";

        [MaxLength(100)]
        public string Category { get; set; } = "";
       
        public string ImageFileName { get; set; } = "";

        [MaxLength(100)]
        public string Availability { get; set; } = "";

        public string FarmerId { get; set; } // Add this property
        public ApplicationUser Farmer { get; set; } // Add this navigation property
    }
}
