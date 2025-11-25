namespace InventarioParcial.Data
{
    using global::InventarioParcial.Models;
    
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    namespace InventarioParcial.Models
    {
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

            // Mapeo de Tablas
            public DbSet<Product> Products { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<Role> Roles { get; set; }
            public DbSet<UserRole> UserRoles { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // CONFIGURACIÓN IMPORTANTE: Llave Compuesta para UserRole
                // Esto le dice a EF Core que la PK está formada por UserId + RoleId
                modelBuilder.Entity<UserRole>()
                    .HasKey(ur => new { ur.UserId, ur.RoleId });

                // Configuraciones de relaciones explícitas (opcional, pero recomendado)
                modelBuilder.Entity<UserRole>()
                    .HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                modelBuilder.Entity<UserRole>()
                    .HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);
            }
        }
    }


}
