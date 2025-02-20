using System;
using System.Security.Cryptography;
using System.Text;

namespace common.Utils
{
    public static class CryptoUtils
    {
        // Mã hóa một chiều (SHA-256)
        public static string HashWithSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Mã hóa hai chiều (AES)
        public static string EncryptWithAES(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // Giá trị khởi tạo (IV) 16 byte

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string DecryptWithAES(string cipherText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // Giá trị khởi tạo (IV) 16 byte

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        // Mã hóa và giải mã (RSA sử dụng công nghệ block)
        public static string EncryptWithRSA(string plainText, RSAParameters publicKey, int blockSize)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
                int maxBlockSize = blockSize - 42; // Kích thước block tối đa cho RSA

                using (MemoryStream ms = new MemoryStream())
                {
                    for (int i = 0; i < dataToEncrypt.Length; i += maxBlockSize)
                    {
                        byte[] block = new byte[Math.Min(maxBlockSize, dataToEncrypt.Length - i)];
                        Buffer.BlockCopy(dataToEncrypt, i, block, 0, block.Length);
                        byte[] encryptedBlock = rsa.Encrypt(block, false);
                        ms.Write(encryptedBlock, 0, encryptedBlock.Length);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string DecryptWithRSA(string cipherText, RSAParameters privateKey, int blockSize)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                byte[] dataToDecrypt = Convert.FromBase64String(cipherText);
                int maxBlockSize = blockSize; // Kích thước block tối đa cho RSA

                using (MemoryStream ms = new MemoryStream())
                {
                    for (int i = 0; i < dataToDecrypt.Length; i += maxBlockSize)
                    {
                        byte[] block = new byte[Math.Min(maxBlockSize, dataToDecrypt.Length - i)];
                        Buffer.BlockCopy(dataToDecrypt, i, block, 0, block.Length);
                        byte[] decryptedBlock = rsa.Decrypt(block, false);
                        ms.Write(decryptedBlock, 0, decryptedBlock.Length);
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        public static void GenerateRSAKeys(out RSAParameters publicKey, out RSAParameters privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                publicKey = rsa.ExportParameters(false);
                privateKey = rsa.ExportParameters(true);
            }
        }
        
    }
}
