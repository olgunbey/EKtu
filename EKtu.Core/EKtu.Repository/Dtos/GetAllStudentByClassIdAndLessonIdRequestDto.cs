using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class GetAllStudentByClassIdAndLessonIdRequestDto
    {
        public int ClassId{ get; set; }
        public int LessonId{ get; set; }
    }
}
