using EKtu.Domain.Entities;
using EKtu.Repository.IRepository.AddPersonRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Repository.AddPersonRepository
{
    public class AddPersonRepository<T> : IAddPersonRepository<T> where T : BasePersonEntity, new()
    {
        private readonly DatabaseContext _databaseContext;
        public AddPersonRepository(DatabaseContext database)
        {
            _databaseContext=database;
        }

        public async Task AddPersonAsync(T data)
        {
           await _databaseContext.Set<T>().AddAsync(data);
        }

        public async Task<bool> ExistUser(string tckKo)
        {
        return await _databaseContext.Set<T>().AnyAsync(y=>y.TckNo== tckKo);
            
        }
    }
}
