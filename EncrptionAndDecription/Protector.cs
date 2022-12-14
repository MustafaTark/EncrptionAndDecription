using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncrptionAndDecription
{
    internal class Protector //AES Advanced Encription Standard(to symtric keys )
    {
        // salt size must be at least 8 bytes, we will use 16 bytes
        private static readonly byte[] salt =
        Encoding.Unicode.GetBytes("7BANANAS");
        // iterations must be at least 1000, we will use 2000
        private static readonly int iterations = 2000;
        public static string Encrypt(
        string plainText, string password)
        {
            byte[] encryptedBytes;
            byte[] plainBytes = Encoding.Unicode
            .GetBytes(plainText);
            var aes = Aes.Create(); // abstract class factory method
            var pbkdf2 = new Rfc2898DeriveBytes(
            password, salt, iterations);
            aes.Key = pbkdf2.GetBytes(32); // set a 256-bit key
            aes.IV = pbkdf2.GetBytes(16); // set a 128-bit IV
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(
                ms, aes.CreateEncryptor(),
                CryptoStreamMode.Write))
                {
                    cs.Write(plainBytes, 0, plainBytes.Length);
                }
                encryptedBytes = ms.ToArray();
            }
            return Convert.ToBase64String(encryptedBytes);
        }
        public static string Decrypt(
        string cryptoText, string password)
        {
            byte[] plainBytes;
            byte[] cryptoBytes = Convert
            .FromBase64String(cryptoText);
            var aes = Aes.Create();
            var pbkdf2 = new Rfc2898DeriveBytes(
            password, salt, iterations);
            aes.Key = pbkdf2.GetBytes(32);
            aes.IV = pbkdf2.GetBytes(16);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(
                ms, aes.CreateDecryptor(),
                CryptoStreamMode.Write))
                {
                    cs.Write(cryptoBytes, 0, cryptoBytes.Length);
                }
                plainBytes = ms.ToArray();
            }
            return Encoding.Unicode.GetString(plainBytes);
        }
        private static Dictionary<string, User> Users =
                                                new Dictionary<string, User>();
        public static User Register(
        string username, string password)
        {
            // generate a random salt
            var rng = RandomNumberGenerator.Create();
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            var saltText = Convert.ToBase64String(saltBytes);
            // generate the salted and hashed password
            var saltedhashedPassword = SaltAndHashPassword(
            password, saltText);
            var user = new User
            {
                Name = username,
                Salt = saltText,
                SaltedHashedPassword = saltedhashedPassword
            };
            Users.Add(user.Name, user);
            return user;
        }
        public static bool CheckPassword(
        string username, string password)
        {
            if (!Users.ContainsKey(username))
            {
                return false;
            }
            var user = Users[username];
            // re-generate the salted and hashed password
            var saltedhashedPassword = SaltAndHashPassword(
            password, user.Salt!);
            return (saltedhashedPassword == user.SaltedHashedPassword);
        }
        private static string SaltAndHashPassword(
        string password, string salt)
        {
            var sha = SHA256.Create();
            var saltedPassword = password + salt;
            return Convert.ToBase64String(
            sha.ComputeHash(Encoding.Unicode.GetBytes(saltedPassword)));
        }

    }
    }
/*
    • We used double the recommended salt size and iteration count.
     
    • Although the salt and iteration count can be hardcoded, the password must be
    passed at runtime when calling the Encrypt and Decrypt methods.

    • We use a temporary MemoryStream type to store the results of encrypting and
    decrypting, and then call ToArray to turn the stream into a byte array.

    • We convert the encrypted byte arrays to and from a Base64 encoding to make
    them easier to read.
 */
