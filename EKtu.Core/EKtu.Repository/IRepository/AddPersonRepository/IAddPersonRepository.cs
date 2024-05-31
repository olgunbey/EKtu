using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IRepository.AddPersonRepository
{
    public interface IAddPersonRepository<T> where T : BasePersonEntity , new()
    {
        Task AddPersonAsync(T data);
        Task<bool> ExistUser(string tckKo);
    }
}
