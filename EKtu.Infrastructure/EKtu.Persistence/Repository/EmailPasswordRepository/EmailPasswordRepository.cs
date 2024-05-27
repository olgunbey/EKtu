using EKtu.Domain.Entities;
using EKtu.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EKtu.Persistence.Repository.EmailPasswordRepository
{
    public class EmailPasswordRepository<T> : IPasswordRepository<T> where T : BasePersonEntity,new()
    {
        private DatabaseContext db;
        public EmailPasswordRepository(DatabaseContext databaseContext)
        {
            db = databaseContext;
        }
        public async Task<T> EmailAndPassword(Expression<Func<T, bool>> expression)
        {
          return await db.Set<T>().FirstAsync(expression);
        }
    }
}
