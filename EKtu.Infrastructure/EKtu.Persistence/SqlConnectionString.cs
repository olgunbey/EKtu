using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence
{
    internal static class SqlConnectionString
    {
        public static string GetConnectionString{ 
            get
            {
                return "Server=127.0.0.1,1555;Database=ExampleDb;User Id=SA;Password=Password123;TrustServerCertificate=True";
            } 
        }
    }
}
