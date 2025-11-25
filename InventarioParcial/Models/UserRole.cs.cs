 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

    namespace InventarioParcial.Models
    {
        // Esta tabla rompe la relación Muchos-a-Muchos
        public class UserRole
        {
            [Key, Column(Order = 0)]
            public int UserId { get; set; }

            [Key, Column(Order = 1)]
            public int RoleId { get; set; }

            // Navegación
            [ForeignKey("UserId")]
            public User? User { get; set; }

            [ForeignKey("RoleId")]
            public Role? Role { get; set; }
        }
    }

