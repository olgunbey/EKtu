using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IRepository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task AddAsync(T data);
        Task UpdateAsync(T data);
        Task DeleteAsync(T data);
        Task<IQueryable<T>> GetAllAsync();
        Task<IQueryable<T>> FindAllAsync(Expression<Func<T,bool>> expression);
        Task<T> GetByIdAsync(int id);

        Task<T> FirstOrDefaultAsync(Expression<Func<T,bool>> expression);
    }
}
