using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Domain.Entities
{
    public class BasePersonEntity:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TckNo { get; set; }
        public string Password { get; set; }

    }
}
