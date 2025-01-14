
using System.Security.Cryptography;
using System.Text;

namespace XLITTE_AuthorizationService.Services
{
    public static class ClientIDGenerator
    {
        public static string Generate(string app_name)
        {

            byte[] app_name_bytes = UTF8Encoding.UTF8.GetBytes(app_name);
            string client_id = BitConverter.ToString(app_name_bytes).Replace("-", "").ToLower();

            return client_id;
        }
    }
}
