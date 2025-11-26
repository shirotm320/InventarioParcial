    using System.Security.Cryptography;
    using System.Text;

    namespace InventarioParcial.Utilities
    {
        public static class PasswordHelper
        {
            // Método para crear el Hash (encriptar)
            public static void CreatePasswordHash(string password, out string passwordHash, out string salt)
            {
                using (var hmac = new HMACSHA512())
                {
                    salt = Convert.ToBase64String(hmac.Key);
                    passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                }
            }

            // Método para verificar si la contraseña ingresada coincide con la guardada
            public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
            {
                using (var hmac = new HMACSHA512(Convert.FromBase64String(storedSalt)))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    var computedHashString = Convert.ToBase64String(computedHash);

                    return computedHashString == storedHash;
                }
            }
        }
    }



