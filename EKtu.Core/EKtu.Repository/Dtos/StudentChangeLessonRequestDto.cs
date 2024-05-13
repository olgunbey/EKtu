using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class StudentChangeLessonRequestDto
    {
        public int CurrentLessonId{ get; set; }
        public int ChangeLessonId{ get; set; }
    }
}
