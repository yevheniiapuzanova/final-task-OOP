using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordCrack
{
    public class PasswordManager
    {
        private static readonly string Salt = "crackSalt";
        public string EncryptedPassword { get; private set; } = string.Empty;

        public void CreatePassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var saltedPassword = password + Salt;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                EncryptedPassword = Convert.ToBase64String(bytes);
            }
        }
    }
}
