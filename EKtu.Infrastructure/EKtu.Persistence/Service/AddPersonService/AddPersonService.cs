using Azure;
using EKtu.Domain.Entities;
using EKtu.Infrastructure.HASH;
using EKtu.Repository.Dtos;
using EKtu.Repository.Exceptions;
using EKtu.Repository.IRepository.AddPersonRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.AddPersonService;

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
            data.Password=HashTransaction.HashPassword(data.Password);

         bool hasUserTckNo= await repository.ExistUser(data.Password);
            if(hasUserTckNo)
            {
                return EKtu.Repository.Dtos.Response<NoContent>.Fail("bu tckno'ya sahip kullanıcı var", 400);
            }
            try
            {
               await repository.AddPersonAsync(data);
               await saves.SaveChangesAsync();
                return EKtu.Repository.Dtos.Response<NoContent>.Success(204);
            }
            catch (Exception)
            {
                throw new AddPersonErrorException("hata peronel eklenemedi");
            }
        }
    }
}
