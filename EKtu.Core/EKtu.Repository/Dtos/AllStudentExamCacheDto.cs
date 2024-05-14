using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class AllStudentExamCacheDto
    {
        public int StudentId{ get; set; }
        public string StudentName{ get; set; }
        public ICollection<AllStudentExamCacheDto2> AllStudentExamCacheDtos{ get; set; }
    }
    public class AllStudentExamCacheDto2
    {
        public int Exam1 { get; set; }
        public int Exam2 { get; set; }
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public string LetterGrade { get; set; }
    }

}
