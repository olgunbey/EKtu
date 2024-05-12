using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class AllStudentExamCacheDto
    {
        public int Exam1 { get; set; }
        public int Exam2 { get; set; }

        public int LessonId{ get; set; }
        public string LessonName { get; set; }
        public string LetterGrade { get; set; }
        public int StudentId{ get; set; }
        public string StudentName{ get; set; }
    }

}
