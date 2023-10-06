using System.Security.Cryptography;

namespace ApiPersons.Utilities
{
    public class TokenGenerator
    {
        public static int LENGTH_TOKEN = 5;
        public static string generateRandomToken()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var tokenBytes = new byte[LENGTH_TOKEN];
                rngCryptoServiceProvider.GetBytes(tokenBytes);
                return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
            }
        }
    }
}
