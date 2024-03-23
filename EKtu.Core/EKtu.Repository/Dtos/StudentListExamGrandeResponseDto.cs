using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class StudentListExamGrandeResponseDto
    {
        public string FirstName{ get; set; }
        public string LastName { get; set; }

        public List<StudentChooseLessonExamGrandeResponseDto> StudentChooseExamGrande{ get; set; }
    }
}
