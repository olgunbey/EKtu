using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class EnteringStudentGradesRequestDto
    {
        public int StudentId{ get; set; }
        public int LessonId{ get; set; }
        public int Exam_1{ get; set; }
        public int Exam_2{ get; set; }
    }
}
