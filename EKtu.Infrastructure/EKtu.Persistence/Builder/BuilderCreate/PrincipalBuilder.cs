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
        public IPersonBuilder Email(string email)
        {
            principal.Email = email;
            return this;
        }

        public IPersonBuilder FirstName(string firstname)
        {
            principal.FirstName = firstname;
            return this;
        }

        public BasePersonEntity GetPerson()
        {
            return principal;
        }

        public IPersonBuilder LastName(string lastname)
        {
            principal.LastName = lastname;
            return this;
        }

        public IPersonBuilder Password(string password)
        {
            principal.Password = password;
            return this;
        }

        public IPersonBuilder TckNo(string TckNo)
        {
            principal.TckNo = TckNo;
            return this;
        }
    }
}
