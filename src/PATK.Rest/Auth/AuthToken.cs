using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using PATK.Domain;

namespace PATK.Rest.Auth
{
    internal static class AuthToken
    {
        internal static string GenerateAuthToken(int expireInDays = 1)
        {
            var id = Config.Identifier;
            var key = Config.Key;
            var expiry = DateTime.UtcNow.AddDays(expireInDays);

            var token = CreateSharedAccessToken(id, key, expiry);

            return token;
        }

        static private string CreateSharedAccessToken(string id, string key, DateTime expiry)
        {
            using (var encoder = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                var dataToSign = id + "\n" + expiry.ToString("O", CultureInfo.InvariantCulture);
                var hash = encoder.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
                var signature = Convert.ToBase64String(hash);
                var encodedToken = $"uid={id}&ex={expiry:o}&sn={signature}";
                return encodedToken;
            }
        }
    }
}