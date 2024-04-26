using EKtu.Domain.Entities;
using EKtu.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.EmailPasswordService
{
    public class EmailPasswordService<T> : IPasswordService<T> where T : BasePersonEntity, new()
    {
        private readonly IPasswordRepository<T> _repository;
        public EmailPasswordService(IPasswordRepository<T> passwordRepository)
        {
            _repository = passwordRepository;
        }
        public async Task<int> EmailAndPassword(string tckno, string password)
        {
          var hasDatas= await _repository.EmailAndPassword(y => y.TckNo == tckno && y.Password == password);

            if(hasDatas is null)
            {
                throw new Exception("kullanıcı yok!!"); //burayı middleware ile düzelt
            }
            return hasDatas.Id;
        }
    }
}
