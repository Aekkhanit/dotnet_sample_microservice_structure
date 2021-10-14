using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Extensions
{
    public static class StringExtension
    {      
        public static string EncodeTo64(this string toEncode)

        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;

        }
        public static string DecodeTo64(this string toEncode)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(toEncode));

        }
    }
}
