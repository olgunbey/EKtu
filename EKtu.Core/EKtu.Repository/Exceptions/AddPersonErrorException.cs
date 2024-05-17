using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Exceptions
{
    public class AddPersonErrorException:Exception
    {
        public AddPersonErrorException(string msj):base(msj) { }
    }
}
