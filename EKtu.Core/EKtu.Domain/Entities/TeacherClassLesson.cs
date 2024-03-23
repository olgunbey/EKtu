using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Domain.Entities
{
    public class TeacherClassLesson:BaseEntity
    {
        public int TeacherId{ get; set; }
        public int ClassId{ get; set; }
        public int LessonId { get; set; }
        public Teacher Teacher{ get; set; }
        public Class Class{ get; set; }
        public Lesson Lesson{ get; set; }
    }
}
