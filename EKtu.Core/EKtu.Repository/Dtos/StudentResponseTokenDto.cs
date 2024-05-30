using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class StudentResponseTokenDto
    {
        public int UserId{ get; set; }
        public int ClassId{ get; set; }
        public int Grade{ get; set; }
    }
}
