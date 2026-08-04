using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.App
{
    public class Cryptography
    {


        /// <summary>
        /// This method retrieve the string to encrypt from the Presentation Layer
        /// And return the Encrypted String
        /// </summary>
        /// <param name="str">
        /// <returns>
        public static string EncryptData(string strText, byte[] userId)
        {
            return Encrypt(strText, new PFAID(userId).ToString());
        }
        public static string EncryptStringData(string strText, string key)
        {
            return Encrypt(strText, key);
        }
        /// <summary>
        /// This method retrieve the encrypted string to decrypt from the Presentation Layer
        /// And return the decrypted string
        /// </summary>
        /// <param name="str">
        /// <returns>
        public static string DecryptData(string str, byte[] userId)
        {
            return Decrypt(str, new PFAID(userId).ToString());
        }
        public static string DecryptStringData(string str, string key)
        {
            return Decrypt(str, key);
        }

        /// <summary>
        /// This method has been used to get the Encrypetd string for the
        /// passed string
        /// </summary>
        /// <param name="strText">
        /// <param name="strEncrypt">
        /// <returns>
        private static string Encrypt(string strText, string strEncrypt)
        {
            byte[] byKey = new byte[20];
            byte[] dv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
                //DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                using DES des = DES.Create();
                des.Padding = PaddingMode.PKCS7;
                //des.Mode = CipherMode.CBC;
                byte[] inputArray = Encoding.UTF8.GetBytes(strText);
              // des.Padding = PaddingMode.None;
                using MemoryStream ms = new MemoryStream();
                using CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, dv), CryptoStreamMode.Write);
   
                cs.Write(inputArray, 0, inputArray.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return Cryptography.GetRandomPassword();
            }
        }
    
        /// <summary>
        /// This method has been used to Decrypt the Encrypted String
        /// </summary>
        /// <param name="strText">
        /// <param name="strEncrypt">
        /// <returns>
        private static string Decrypt(string strText, string strEncrypt)
        {
            byte[] bKey = new byte[20];
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            try
            {
                bKey = Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
                using DES des = DES.Create();
                des.Padding = PaddingMode.PKCS7;
                //des.Mode = CipherMode.CBC;
                byte[] inputByteArray = inputByteArray = Convert.FromBase64String(strText);
                using MemoryStream ms = new MemoryStream();
                using CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(bKey, IV), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;

                return encoding.GetString(ms.ToArray());
            }

            catch (Exception ex)
            {
                return  Cryptography.GetRandomPassword();
            }

        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        /// <summary>
        /// used to get 6 characters random Password
        /// </summary>
        /// <returns></returns>
        public static string GetRandomPassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 6)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result.ToString();
        }

    }
}
