 using System.ComponentModel.DataAnnotations;

    namespace InventarioParcial.Models
    {
        public class User
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(50)]
            public string Username { get; set; }

            [Required]
            public string PasswordHash { get; set; } // Contraseña encriptada

            [Required]
            public string Salt { get; set; } // Para reforzar la encriptación

            // Relación Muchos a Muchos con Roles
            public ICollection<UserRole>? UserRoles { get; set; }
        }
    }
