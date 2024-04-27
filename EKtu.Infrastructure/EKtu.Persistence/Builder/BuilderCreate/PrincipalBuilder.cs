using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.IBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.BuilderCreate
{
    public class PrincipalBuilder : IPrincipalBuilder
    {
        Principal principal;
        public PrincipalBuilder()
        {
            principal = new Principal();
        }
        public IPrincipalBuilder Email(string email)
        {
            principal.Email = email;
            return this;
        }

        public IPrincipalBuilder FirstName(string firstname)
        {
            principal.FirstName = firstname;
            return this;
        }

        public Principal GetPerson()
        {
            return principal;
        }

        public IPrincipalBuilder LastName(string lastname)
        {
            principal.LastName = lastname;
            return this;
        }

        public IPrincipalBuilder Password(string password)
        {
            principal.Password = password;
            return this;
        }

        public IPrincipalBuilder TckNo(string TckNo)
        {
            principal.TckNo = TckNo;
            return this;
        }
    }
}
