using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Domain.Entities
{
    public class StudentChooseLesson:BaseEntity
    {
        public int StudentId{ get; set; }
        public int LessonId{ get; set; }
        public Student Student{ get; set; }
        public Lesson Lesson{ get; set; }
        public List<Attendance> Attendance{ get; set; }

        public ExamNote ExamNote{ get; set; }
    }
}
