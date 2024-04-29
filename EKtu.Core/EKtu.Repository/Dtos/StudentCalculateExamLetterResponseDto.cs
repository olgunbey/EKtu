using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class StudentCalculateExamLetterResponseDto
    {
        public int GradeAverage{ get; set; }
        public string LatterGrande{ get; set; }
        public int ExamNoteId{ get; set; }
    }
}
