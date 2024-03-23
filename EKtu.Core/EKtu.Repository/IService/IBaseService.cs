using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<Response<IEnumerable<T>>> GetAllAsync();

        Task<Response<bool>> AddAsync(T entity);
        Task<Response<bool>> UpdateAsync(T entity);
        Task<Response<bool>> DeleteAsync(T entity);

        Task<Response<IEnumerable<T>>> FindAllAsync(Expression<Func<T, bool>> expression);

        Task<Response<T>> GetByIdAsync(int id);
    }
}
