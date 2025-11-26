using InventarioParcial.Data.InventarioParcial.Models;
using InventarioParcial.Models;
using InventarioParcial.Utilities;
using Microsoft.EntityFrameworkCore;

namespace InventarioParcial.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> Register(User user, string password)
        {
            // 1. Crear Hash y Salt
            PasswordHelper.CreatePasswordHash(password, out string passwordHash, out string salt);

            user.PasswordHash = passwordHash;
            user.Salt = salt;

            // 2. Guardar Usuario
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // 3. Asignar Rol por defecto ("User" = ID 2, asumiendo que Admin es 1)
            // IMPORTANTE: Asegúrate de que en tu BD el Rol "User" tenga ID 2.
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (defaultRole != null)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = defaultRole.Id
                };
                await _context.UserRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();
            }

            return user;
        }

        public async Task<User?> Login(string username, string password)
        {
            // Buscamos el usuario e incluimos sus Roles para saber quién es
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null; // Usuario no existe

            // Verificamos la contraseña
            if (!PasswordHelper.VerifyPasswordHash(password, user.PasswordHash, user.Salt))
                return null; // Contraseña incorrecta

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }
    }
}


