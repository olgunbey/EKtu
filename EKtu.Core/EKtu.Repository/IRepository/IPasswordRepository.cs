using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IRepository
{
    public interface IPasswordRepository<T> where T : BasePersonEntity, new()
    {
        Task<T> EmailAndPassword(Expression<Func<T,bool>> expression);
    }
}
