using Azure;
using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository.AddPersonRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.AddPersonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Service.AddPersonService
{
    public class AddPersonService<T> : IAddPersonService<T> where T : BasePersonEntity, new()
    {
        private readonly IAddPersonRepository<T> repository;
        private readonly ISaves saves;
        public AddPersonService(IAddPersonRepository<T> addPersonRepository, ISaves saves)
        {
            repository = addPersonRepository;
            this.saves = saves;
        }

        public async Task<EKtu.Repository.Dtos.Response<NoContent>> AddAsync(T data)
        {
            try
            {
               await repository.AddPersonAsync(data);
               await saves.SaveChangesAsync();
                return EKtu.Repository.Dtos.Response<NoContent>.Success(204);
            }
            catch (Exception)
            {
                throw new Exception("değer eklenemedi"); //midleware ile hatayı yönet

                throw;
            }
        }
    }
}
