using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class ClassExamNoteAverageResponseDto
    {
        public int ClassId{ get; set; }
        public int LessonId { get; set; }
        public decimal Avg { get; set; }
    }
}
