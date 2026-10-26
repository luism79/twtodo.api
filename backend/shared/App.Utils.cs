using System.Security.Cryptography;
using System.Text;

namespace backend.shared
{
    public static class AppUtils
    {
        public static string ConvertTextToHash(string value)
        {

            byte[] inputBytes = Encoding.UTF8.GetBytes(value);
            byte[] hashBytes = SHA256.Create().ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes).ToLower();
        }
    }
}
