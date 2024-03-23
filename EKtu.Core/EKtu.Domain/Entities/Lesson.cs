using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Domain.Entities
{
    public class Lesson:BaseEntity
    {
        public string LessonName{ get; set; }
        public bool HasOptional { get; set; }
        public int? OptionalLessonId { get; set; }
        public int Grade { get; set; }
        public bool Term { get; set; }
        public ICollection<TeacherClassLesson> TeacherClassLessons{ get; set; }

        public ICollection<StudentChooseLesson>  StudentChooseLessons{ get; set; }

        public OptionalLesson? OptionalLesson{ get; set; }
    }
}
