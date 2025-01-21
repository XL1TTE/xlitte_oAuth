using System.Security.Cryptography;
using System.Text;

namespace XLITTE_AuthorizationService.Services
{
    public class PasswordSecurityService
    {
        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, 1000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                // Сравниваем хеши
                for (int i = 0; i < hash.Length; i++)
                {
                    if (hash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static (byte[] hash, byte[] salt) GetPasswordHash(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                return (hash, salt);
            }
        }

        private static byte[] StringToBytes(string String)
        {
            return Encoding.ASCII.GetBytes(String);
        } 
    }
}
