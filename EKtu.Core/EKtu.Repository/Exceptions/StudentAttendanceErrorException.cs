using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Exceptions
{
    public class StudentAttendanceErrorException:Exception
    {
        public StudentAttendanceErrorException(string msj):base(msj) { }
    }
}
