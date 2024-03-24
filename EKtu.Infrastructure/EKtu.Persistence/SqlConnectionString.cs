using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence
{
    public static class SqlConnectionString
    {
        
        public static string GetConnectionString{ 
            get
            {
                return new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build().GetConnectionString("mydb");
            } 
        }
    }
}
