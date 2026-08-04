using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.Publishing
{
    public class SignatureHelper
    {
        /// <summary>
        /// Generate HMAC-SHA256 signature.
        /// </summary>
        /// <param name="payload">The raw JSON or string payload.</param>
        /// <param name="secretKey">The secret key for hashing.</param>
        /// <returns>Base64 encoded signature string.</returns>
        public static string GenerateSignature(string payload, string secretKey)
        {
            if (string.IsNullOrEmpty(payload)) return string.Empty;
            if (string.IsNullOrEmpty(secretKey)) return string.Empty;

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
