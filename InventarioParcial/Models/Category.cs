
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace InventarioParcial.Models
    {
        public class Category
        {
            [Key] 
            public int Id { get; set; }

            [Required(ErrorMessage = "El nombre es obligatorio")]
            [MaxLength(100)]
            public string Name { get; set; }

            [MaxLength(255)]
            public string? Description { get; set; } 

            
            public ICollection<Product>? Products { get; set; }
        }
    }


