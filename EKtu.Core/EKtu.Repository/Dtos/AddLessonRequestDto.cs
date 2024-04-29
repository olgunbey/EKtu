using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class AddLessonRequestDto
    {
        public string LessonName { get; set; }
        public bool HasOptional { get; set; }
        public int OptionalLessonId { get; set; }
        public int Grade{ get; set; }
        public bool Term{ get; set; }
    }
}
