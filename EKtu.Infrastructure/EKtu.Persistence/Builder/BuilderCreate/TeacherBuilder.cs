using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.IBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.BuilderCreate
{
    public class TeacherBuilder : ITeacherBuilder
    {
        Teacher teacher;
        public TeacherBuilder(Teacher _teacher)
        {
            teacher = _teacher;
        }

        public ITeacherBuilder FirstName(string firstname)
        {
            teacher.FirstName = firstname;
            return this;
        }

        public Teacher GetPerson()
        {
            return teacher;
        }

        public ITeacherBuilder LastName(string lastname)
        {
            teacher.LastName = lastname;
            return this;
        }

        public ITeacherBuilder Password(string password)
        {
            teacher.Password = password;
            return this;
        }

        public ITeacherBuilder TckNo(string TckNo)
        {
            teacher.TckNo = TckNo;
            return this;
        }
    }
}
