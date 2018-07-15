using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace EasySendler.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string inputString, int symbolCount)
        {
            return $"{inputString.Substring(0, Math.Min(inputString.Length, symbolCount))}...";
        }

        public static string Encrypt(this string toEncrypt, bool useHashing = true)
        {
            if (string.IsNullOrWhiteSpace(toEncrypt))
            {
                return toEncrypt;
            }

            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            var settingsReader = new AppSettingsReader();

            var key = (string)settingsReader.GetValue("SecurityKey", typeof(string));
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tdes.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(this string cipherString, bool useHashing = true)
        {
            if (string.IsNullOrWhiteSpace(cipherString))
            {
                return cipherString;
            }

            byte[] keyArray;
            var toEncryptArray = Convert.FromBase64String(cipherString);

            AppSettingsReader settingsReader = new AppSettingsReader();
            var key = (string)settingsReader.GetValue("SecurityKey", typeof(string));

            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}