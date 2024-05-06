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
        public int LessonConfirmationId{ get; set; }
        public LessonConfirmation LessonConfirmation{ get; set; }

    }
}
