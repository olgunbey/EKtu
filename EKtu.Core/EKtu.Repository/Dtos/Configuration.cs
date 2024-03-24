﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class Configuration
    {
        
        public string mail{ get; set; }
        public string mailPassword{ get; set; }
        public string tokenKey { get; set; }
        public string tokenIssuer{ get; set; }
        public string tokenAudience{ get; set; }
    }
}
