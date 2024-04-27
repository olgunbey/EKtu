using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
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
        Task<Response<int>> EmailAndPassword(string tckno, string password);
    }
}
