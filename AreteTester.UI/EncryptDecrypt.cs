using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;

namespace AreteTester.UI
{
    public class EncryptDecrypt
    {
        private const string desKey = "mwadjzra";

        public static string Encrypt(string text)
        {
            return Encrypt(text, desKey);
        }

        public static string Encrypt(string text, string key)
        {
            string encryptedText = string.Empty;

            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
                using (StreamWriter writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(text);
                    writer.Flush();
                    cryptoStream.FlushFinalBlock();

                    encryptedText = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
            }

            return encryptedText;
        }

        public static string Decrypt(string text)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(desKey);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(text)))
            {
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);
                return reader.ReadToEnd();
            }
        }

        public static void EncryptFile(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = ASCIIEncoding.ASCII.GetBytes(desKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(desKey);

                ICryptoTransform desEncrypt = des.CreateEncryptor();
                using (CryptoStream cryptostream = new CryptoStream(stream, desEncrypt, CryptoStreamMode.Write))
                {
                    byte[] bytearrayinput = new byte[stream.Length];
                    stream.Read(bytearrayinput, 0, bytearrayinput.Length);
                    // Clear content
                    stream.SetLength(0);
                    // Write encrypted content
                    cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                }
            }
        }

        public static void DecryptFile(string filename)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes(desKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(desKey);

            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                ICryptoTransform desdecrypt = des.CreateDecryptor();
                CryptoStream cryptostreamDecr = new CryptoStream(stream, desdecrypt, CryptoStreamMode.Read);
                string content = new StreamReader(cryptostreamDecr).ReadToEnd();

                File.WriteAllText(filename, content);
            }
        }

        public static string GenerateKey()
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            string newKey = string.Empty;

            for (; ; )
            {
                des.GenerateKey();
                byte[] keyBytes = des.Key;

                bool validKeyFound = true;
                foreach (byte b in keyBytes)
                {
                    // This is used to get readable characters and not '\' because it is an escape sequence.
                    if ((b < 32 || b > 126) && b != 92 && b != 34)
                    {
                        validKeyFound = false;
                        continue;
                    }
                }

                if (validKeyFound)
                {
                    newKey = ASCIIEncoding.ASCII.GetString(keyBytes);
                    if (newKey.Contains(@""""))
                    {
                        continue;
                    }
                    break;
                }
            }

            return newKey;
        }
    }
}
