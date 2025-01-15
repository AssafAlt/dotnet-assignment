using System.ComponentModel.DataAnnotations;

namespace ColorsManagement.Models
{
    public class Color
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Color name is required.")]
        public string ColorName { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Display Order is required.")]
        public int DisplayOrder { get; set; }
        [Required(ErrorMessage = "In Stock is required.")]
        public bool InStock { get; set; }

        
    }
}
