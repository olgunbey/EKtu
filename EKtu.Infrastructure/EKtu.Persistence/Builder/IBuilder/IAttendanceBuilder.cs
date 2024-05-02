using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.IBuilder
{
    public interface IAttendanceBuilder
    {

        IAttendanceBuilder StudentChooseLessonId(int id);
        IAttendanceBuilder AttendanceDate(DateTime dateTime);
        IAttendanceBuilder ReasonForAbsence(string reasonforabsence);
        IAttendanceBuilder PermissionCheck(bool permissionCheck);
        Attendance GetAttendance();
    }
}
