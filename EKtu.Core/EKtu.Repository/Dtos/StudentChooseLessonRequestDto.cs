using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class StudentChooseLessonRequestDto
    {
        public int StudentId{ get; set; }
        public List<int> LessonId { get; set; }
    }
}
