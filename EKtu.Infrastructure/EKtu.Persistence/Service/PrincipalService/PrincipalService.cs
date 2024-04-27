using EKtu.Domain.Entities;
using EKtu.Infrastructure.HASH;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository;
using EKtu.Repository.IRepository.PrincipalRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.PrincipalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Service.PrincipalService
{
    public class PrincipalService : BaseService<Principal>, IPrincipalService
    {
        private readonly IPrincipalRepository principalRepository;
        private readonly ISaves _saves;
        private readonly IPrincipalBuilder principalBuilder;
        public PrincipalService(IBaseRepository<Principal> baseRepository, ISaves saves, IPrincipalRepository principalRepository, IPrincipalBuilder principalBuilder) : base(baseRepository, saves)
        {
            this.principalRepository = principalRepository;
            this._saves = saves;
            this.principalBuilder = principalBuilder;
        }

        public async Task<Response<NoContent>> StudentChooseApproveAsync()
        {
            try
            {
                await principalRepository.StudentLessonApproveAsync();
                await _saves.SaveChangesAsync();
                return Response<NoContent>.Success(204);
            }
            catch (Exception)
            {
                return Response<NoContent>.Fail("Dersler kaydedilmedi", 400);
            }
        }
    }
}
