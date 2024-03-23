using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Domain.Entities
{
    public class OptionalLesson:BaseEntity
    {
        public string LessonType { get; set; }

        public ICollection<Lesson> Lessons{ get; set; }
    }
}
