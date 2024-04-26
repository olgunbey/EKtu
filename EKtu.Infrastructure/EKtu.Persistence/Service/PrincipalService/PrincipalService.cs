using EKtu.Domain.Entities;
using EKtu.Repository.IRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.PrincipalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Service.PrincipalService
{
    public class PrincipalService : BaseService<Principal>, IPrincipalService
    {
        public PrincipalService(IBaseRepository<Principal> baseRepository, ISaves saves) : base(baseRepository, saves)
        {
        }
    }
}
