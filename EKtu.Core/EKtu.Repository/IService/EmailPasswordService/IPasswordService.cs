using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.EmailPasswordService
{
    public interface IPasswordService<T> where T : BasePersonEntity, new()
    {
        Task<int> EmailAndPassword(string tckno, string password);
    }
}
