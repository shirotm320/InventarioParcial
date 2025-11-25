    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace InventarioParcial.Models
    {
        public class Product
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(100)]
            public string Name { get; set; }

            [Required]
            [Column(TypeName = "decimal(18,2)")] // Define precisión para dinero
            public decimal Price { get; set; }

            [Required]
            public int Stock { get; set; }

            // --- Claves Foráneas ---

            [Required]
            public int CategoryId { get; set; } // La columna en la BD

            [ForeignKey("CategoryId")]
            public Category? Category { get; set; } // El objeto de navegación
        }
    }


