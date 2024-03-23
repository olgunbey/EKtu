using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Infrastructure.HASH
{
    public class HashTransaction
    {
        public static string HashPassword(string password)
        {
            using (var create=SHA256.Create())
            {
               byte[] bytesArray= create.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder stringBuilder = new StringBuilder();
                for(int i = 0; i < bytesArray.Length; i++)
                {
                    stringBuilder.Append(bytesArray[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }

        }
    }
}
