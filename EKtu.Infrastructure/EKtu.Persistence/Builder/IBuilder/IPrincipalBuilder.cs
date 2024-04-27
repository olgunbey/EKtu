
using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.IBuilder
{
    public interface IPrincipalBuilder
    {
        IPrincipalBuilder FirstName(string firstname);
        IPrincipalBuilder LastName(string lastname);

        IPrincipalBuilder Email(string email);
        IPrincipalBuilder Password(string password);
        IPrincipalBuilder TckNo(string TckNo);

        Principal GetPerson();
    }
}
