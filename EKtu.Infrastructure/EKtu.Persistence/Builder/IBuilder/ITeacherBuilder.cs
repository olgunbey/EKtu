using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.IBuilder
{
    public interface ITeacherBuilder
    {
        ITeacherBuilder FirstName(string firstname);
        ITeacherBuilder LastName(string lastname);
        ITeacherBuilder Password(string password);
        ITeacherBuilder TckNo(string TckNo);
        Teacher GetPerson();
    }
}
