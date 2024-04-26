using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.AddPersonService
{
    public interface IAddPersonService<T> where T : BasePersonEntity,new()
    {
      Task<Response<NoContent>> AddAsync(T data);
    }
}
