using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class StudentAbsenceDto
    {
        public string LessonName{ get; set; }
        public List<DateTime> AbsenceDateTimes{ get; set; }
        public int AbsenceCount { get; set; }
    }
}
