using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.IBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.BuilderCreate
{
    internal class StudentBuilder : IStudentBuilder
    {
        private Student student;
        public StudentBuilder()
        {
            student = new Student();
        }
        public IStudentBuilder ClassId(int classId)
        {
            student.ClassId = classId;
            return this;
        }

        public IStudentBuilder FirstName(string firstName)
        {
            student.FirstName = firstName;
            return this;
        }

        public IStudentBuilder LastName(string lastName)
        {
            student.LastName = lastName;
            return this;
        }

        public IStudentBuilder Password(string password)
        {
            student.Password = password;
            return this;
        }

        public Student Student()
        {
            return student;
        }

        public IStudentBuilder TckNo(string tckNo)
        {
           student.TckNo = tckNo;
            return this;
        }
    }
}
