using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.PrincipalService
{
    public interface IPrincipalService:IBaseService<Principal>,IPasswordService<Principal>
    {
    }
}
