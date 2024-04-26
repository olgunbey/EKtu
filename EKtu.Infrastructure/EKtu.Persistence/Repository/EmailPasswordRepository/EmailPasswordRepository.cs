using EKtu.Domain.Entities;
using EKtu.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
          return await db.Set<T>().FirstOrDefaultAsync(expression);
        }
    }
}
