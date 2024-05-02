using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Domain.Entities
{
    public class Attendance:BaseEntity
    {
        public StudentChooseLesson StudentChooseLesson{ get; set; }
        public int StudentChooseLessonId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string ReasonForAbsence { get; set; }
        public bool PermissionCheck{ get; set; }

    }
}
