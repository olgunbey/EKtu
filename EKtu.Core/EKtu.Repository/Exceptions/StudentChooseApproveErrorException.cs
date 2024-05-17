using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Exceptions
{
    public class StudentChooseApproveErrorException:Exception
    {
        public StudentChooseApproveErrorException(string msj):base(msj) { }
    }
}
