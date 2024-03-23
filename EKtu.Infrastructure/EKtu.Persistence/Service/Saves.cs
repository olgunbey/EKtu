using EKtu.Repository.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Service
{
    public class Saves : ISaves
    {
        private DatabaseContext _dbContext;
        public Saves(DatabaseContext database)
        {
            _dbContext = database;
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
           await _dbContext.SaveChangesAsync();
        }
    }
}
