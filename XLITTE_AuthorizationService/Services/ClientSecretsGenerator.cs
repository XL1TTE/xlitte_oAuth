using System.Security.Cryptography;

namespace XLITTE_AuthorizationService.Services
{
    public static class ClientSecretsGenerator
    {
        public static string Generate()
        {
            byte[] randomBytes = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            string secret = BitConverter.ToString(randomBytes).Replace("-", "").ToLower();

            return secret;
        }
    }
}
