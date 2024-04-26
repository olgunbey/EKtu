using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.IBuilder
{
    public interface IStudentBuilder
    {
        public IStudentBuilder FirstName(string firstName);
        public IStudentBuilder LastName(string lastName);
        public IStudentBuilder TckNo(string tckNo);
        public IStudentBuilder Password(string password);
        public IStudentBuilder ClassId(int classId);
        public IStudentBuilder Email(string email);
        public Student Student();
    }
}
