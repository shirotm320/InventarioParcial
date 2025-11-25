   using System.ComponentModel.DataAnnotations;

    namespace InventarioParcial.Models
    {
        public class Role
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(50)]
            public string Name { get; set; } 

            // Relación Muchos a Muchos con Users
            public ICollection<UserRole>? UserRoles { get; set; }
        }
    }
