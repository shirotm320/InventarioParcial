using System.ComponentModel.DataAnnotations;

namespace InventarioParcial.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } 
    }
}