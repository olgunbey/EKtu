using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Domain.Entities
{
    public class Student:BasePersonEntity
    {
        public int ClassId { get; set; }
        public Class Class{ get; set; }
        public ICollection<StudentChooseLesson> StudentChooseLessons{ get; set; }
        public string Email{ get; set; }
        public ICollection<LessonConfirmation> LessonConfirmation{ get; set; }

    }
}
