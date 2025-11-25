    using System.ComponentModel.DataAnnotations;

    namespace InventarioParcial.Dtos
    {
        public class ProductCreateDto
        {
            [Required(ErrorMessage = "El nombre es obligatorio")]
            [MinLength(3, ErrorMessage = "Mínimo 3 caracteres")]
            public string Name { get; set; } = string.Empty;

            [Required]
            [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
            public decimal Price { get; set; }

            [Required]
            [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
            public int Stock { get; set; }

            [Required(ErrorMessage = "Debes seleccionar una categoría")]
            public int CategoryId { get; set; }
        }
    }
