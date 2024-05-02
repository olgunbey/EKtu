using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.IBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.BuilderCreate
{
    public class AttendanceBuilder : IAttendanceBuilder
    {
        private readonly Attendance attendance;
        public AttendanceBuilder(Attendance attendance)
        {

            this.attendance = attendance;

        }
        public IAttendanceBuilder AttendanceDate(DateTime dateTime)
        {
            this.attendance.AttendanceDate = dateTime;
            return this;
        }

        public Attendance GetAttendance()
        {
           return this.attendance;
        }

        public IAttendanceBuilder PermissionCheck(bool permissionCheck)
        {
            this.attendance.PermissionCheck = permissionCheck;
            return this;
        }

        public IAttendanceBuilder ReasonForAbsence(string reasonforabsence)
        {
            this.attendance.ReasonForAbsence = reasonforabsence;
            return this;
        }

        public IAttendanceBuilder StudentChooseLessonId(int id)
        {
           this.attendance.StudentChooseLessonId=id;
            return this;
        }
    }
}
