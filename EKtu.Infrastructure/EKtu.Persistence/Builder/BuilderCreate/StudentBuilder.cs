using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.IBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.BuilderCreate
{
    public class StudentBuilder : IStudentBuilder
    {
        Student student;
        public StudentBuilder(Student _student)
        {
            student = _student;
        }

        public IStudentBuilder ClassId(int classId)
        {
            student.ClassId = classId;
            return this;

        }

        public IStudentBuilder Email(string email)
        {
            student.Email = email;
            return this;
        }

        public IStudentBuilder FirstName(string firstname)
        {
            student.FirstName = firstname;
            return this;
        }

        public Student GetPerson()
        {
            return student;
        }
        public IStudentBuilder LastName(string lastname)
        {
            student.LastName = lastname;
            return this;
        }

        public IStudentBuilder Password(string password)
        {
            student.Password = password;
            return this;
        }

        public IStudentBuilder TckNo(string TckNo)
        {
            student.TckNo = TckNo;
            return this;
        }
    }
}
