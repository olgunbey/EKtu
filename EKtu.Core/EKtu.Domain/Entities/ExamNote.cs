using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Domain.Entities
{
    public class ExamNote:BaseEntity
    {
        public int Exam1{ get; set; }
        public int Exam2{ get; set; }
        public string? LetterGrade { get; set; }
        public int StudentChooseLessonId{ get; set; }
        public StudentChooseLesson StudentChooseLesson{ get; set; }

    }
}
