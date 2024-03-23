using EKtu.Domain.Entities;
using EKtu.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Repositorys
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected DatabaseContext _dbContext;
        public BaseRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }
        public async Task AddAsync(T data)
        {
           await _dbContext.AddAsync(data);
        }

        public Task DeleteAsync(T data)
        {
          return Task.FromResult(_dbContext.Remove(data));
        }

        public Task<IQueryable<T>> FindAllAsync(Expression<Func<T, bool>> expression)
        {
         return Task.FromResult(_dbContext.Set<T>().Where(expression));
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
           return await _dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public Task<IQueryable<T>> GetAllAsync()
        {
            return Task.FromResult(_dbContext.Set<T>().AsQueryable());
        }

        public async Task<T> GetByIdAsync(int id)
        {
           return await _dbContext.Set<T>().FindAsync(id);
        }

        public Task UpdateAsync(T data)
        {
            return Task.FromResult(_dbContext.Update(data));
        }
    }
}
