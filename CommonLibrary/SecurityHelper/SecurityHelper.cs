#region 文件和作者信息
/******************************************************************

  * CLR 版本: 4.0.30319.42000
  * 机器名称: SUNXIAN
  * 命名空间: CommonLibrary.SecurityHelper
  * 文 件 名: SecurityHelper
  * 创建人员: SunXian
  * 创建时间: 2017/4/26 10:40:16
  
//******************************************************************
* Copyright： @青岛凯亚研发部@
******************************************************************/
#endregion

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary.SecurityHelper
{
    /// <summary>
    /// DES 8位堆成加密算法
    /// </summary>
    public class SecurityHelper
    {
        public static string EncryptKey = "qd-cares";    //定义密钥
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDes(string encryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(EncryptKey.Substring(0, 8));
                byte[] rgbIv = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCsp = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCsp.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDes(string decryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(EncryptKey);
                byte[] rgbIv = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider dcsp = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dcsp.CreateDecryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        /// <summary>
        /// MD5 32位加密算法
        /// </summary>
        /// <param name="input">待加密的字符串</param>
        /// <param name="code">编码方式</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeByMd532(string input, string code = "UTF-8")
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var t = md5.ComputeHash(Encoding.GetEncoding(code).GetBytes(input));
            var sb = new StringBuilder(32);
            foreach (byte t1 in t)
            {
                sb.Append(t1.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
