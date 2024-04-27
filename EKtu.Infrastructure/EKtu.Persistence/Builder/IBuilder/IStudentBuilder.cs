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
        IStudentBuilder FirstName(string firstname);
        IStudentBuilder LastName(string lastname);

        IStudentBuilder Email(string email);
        IStudentBuilder Password(string password);
        IStudentBuilder TckNo(string TckNo);
        IStudentBuilder ClassId(int ClassId);

        Student GetPerson();
    }
}
