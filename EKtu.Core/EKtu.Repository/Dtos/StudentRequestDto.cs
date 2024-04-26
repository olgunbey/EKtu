using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class StudentRequestDto
    {
        public string StudentName{ get; set; }
        public string StudentLastName{ get; set; }
        public string Email{ get; set; }
        public string StudentPassword { get; set; }
        public string StudentTckNo{ get; set; }
        public int ClassId { get; set; }
    }
}
