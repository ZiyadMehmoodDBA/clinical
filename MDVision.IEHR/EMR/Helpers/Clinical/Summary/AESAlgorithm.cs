using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Summary
{
    public class AESAlgorithm
    {
        private AesManaged aes;
        private string salt;

        public AESAlgorithm()
        {
            aes = new AesManaged();
            aes.Padding = PaddingMode.Zeros;
            salt = "Clinical Document Architecture";
        }

        public string Encrypt(string str, string password)
        {
            string encryptedText = string.Empty;

            if (!String.IsNullOrEmpty(str) && !String.IsNullOrEmpty(password))
            {
                GenerateKey(password);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(str);
                        }
                        encryptedText = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            return encryptedText;
        }

        public string Decrypt(string str, string password)
        {
            string plainText = string.Empty;

            if (!String.IsNullOrEmpty(str) && !String.IsNullOrEmpty(password))
            {
                this.GenerateKey(password);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] ct = Convert.FromBase64String(str);

                using (MemoryStream msDecrypt = new MemoryStream(ct))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }

        private void GenerateKey(string password)
        {
            if (!String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(salt))
            {
                byte[] saltArray = Encoding.ASCII.GetBytes(salt);
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, saltArray);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);
            }
        }

        public string GenerateHash(string str)
        {
            string hashCode = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                Encoding enc = new UTF8Encoding();
                byte[] buffer = enc.GetBytes(str);

                SHA1CryptoServiceProvider cryptoTransformSha1 = new SHA1CryptoServiceProvider();

                hashCode = BitConverter.ToString(
                    cryptoTransformSha1.ComputeHash(buffer)).Replace("-", "");
            }
            return hashCode;
        }
    }
}