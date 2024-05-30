using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class GetLessonResponseDto
    {
        public List<AnaDers> AnaDers{ get; set; }
        public List<SecmeliDers> SecmeliDers{ get; set; }
    }

    public class SecmeliDers
    {
        public int OptionalLessonId { get; set; }
        public int LessonId { get; set; }
        public string LessonName { get; set; }
    }
    public class AnaDers
    {
        public int LessonId{ get; set; }
        public string LessonName{ get; set; }
    }
}
