using System;
using System.Security.Cryptography;
using System.Text;

namespace ZBlog.Common
{
    public class Util
    {
        /// <summary>
        /// Get the md5 of the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetMd5(string value)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }
    }
}