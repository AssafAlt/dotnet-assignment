using System.ComponentModel.DataAnnotations;

namespace ColorsManagement.Dtos
{
    public class OperationColorDto
    {
        
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
