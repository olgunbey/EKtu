using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Domain.Entities
{
    public class Teacher:BasePersonEntity
    {
        public ICollection<TeacherClassLesson> TeacherClassLessons{ get; set; }
    }
}
