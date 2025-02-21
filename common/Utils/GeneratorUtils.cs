using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace common.Utils
{
	public class GeneratorUtils
	{
        /// <summary>
        /// Tạo key từ object bằng cách serialize thành JSON.
        /// </summary>
        public static string GenerateKey(object data, params string[] ignoreFields)
        {
            if (data == null) return "null";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            // Serialize object thành JSON
            string json = JsonSerializer.Serialize(data, options);

            // Nếu có field cần ignore
            foreach (var field in ignoreFields)
            {
                json = json.Replace($"\"{field}\":", $"\"{field}\":\"IGNORED\",");
            }

            return ComputeHash(json);
        }

        /// <summary>
        /// Tạo key từ dictionary (trường hợp tham số động)
        /// </summary>
        public static string GenerateKeyFromDictionary(Dictionary<string, object> parameters)
        {
            var sortedParams = parameters.OrderBy(p => p.Key)
                                         .Select(p => $"{p.Key}={p.Value}")
                                         .ToList();

            string rawKey = string.Join("&", sortedParams);
            return ComputeHash(rawKey);
        }

        /// <summary>
        /// Tạo hash SHA256 để rút gọn key
        /// </summary>
        private static string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Hash bằng FNV-1a (Nhanh nhất)
        /// </summary>
        public static ulong GenerateFnv1aKey(object obj)
        {
            string input = JsonUtils.SerializeObject(obj);
            const ulong fnvOffsetBasis = 14695981039346656037;
            const ulong fnvPrime = 1099511628211;

            ulong hash = fnvOffsetBasis;
            foreach (byte b in Encoding.UTF8.GetBytes(input))
            {
                hash ^= b;
                hash *= fnvPrime;
            }
            return hash;
        }
    }
}

