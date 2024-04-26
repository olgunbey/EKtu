using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Domain.Entities
{
    public class Principal:BasePersonEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TckNo { get; set; }
        public string Password { get; set; }
        public string Email{ get; set; }
    }
}
