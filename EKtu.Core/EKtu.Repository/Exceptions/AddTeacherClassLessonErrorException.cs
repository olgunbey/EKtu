using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Exceptions
{
    public class AddTeacherClassLessonErrorException:Exception
    {
        public AddTeacherClassLessonErrorException(string msj):base(msj) { }
    }
}
