using System.ComponentModel.DataAnnotations;

namespace ColorsManagement.Dtos
{
    public class UpdateDisplayOrderDto
    {
        [Required(ErrorMessage = "Color Id is required.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Display Order is required.")]
        public int DisplayOrder { get; set; }
    }
}
