using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository;
using EKtu.Repository.IService;

namespace EKtu.Persistence.Service
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected IBaseRepository<T> repository;
        protected ISaves _saves;
        public BaseService(IBaseRepository<T> baseRepository, ISaves saves)
        {
            repository = baseRepository;
            _saves = saves;
        }
        public async Task<Response<bool>> AddAsync(T entity)
        {
            try
            {
                await repository.AddAsync(entity);
               await _saves.SaveChangesAsync();
                return Response<bool>.Success(true, 200);
            }
            catch (Exception)
            {
                return Response<bool>.Fail("eklenmedi", 200);
            }
        }

        public async Task<Response<bool>> DeleteAsync(T entity)
        {
            try
            {
                await repository.DeleteAsync(entity);
                await _saves.SaveChangesAsync();
                return Response<bool>.Success(true, 200);
            }
            catch (Exception)
            {
                return Response<bool>.Fail("silinemedi", 200);
            }
        }

        public async Task<Response<IEnumerable<T>>> FindAllAsync(Expression<Func<T, bool>> expression)
        {
         var Datas= (await  repository.FindAllAsync(expression)).ToList();

            return DatasControl(Datas);
        }

        public async Task<Response<IEnumerable<T>>> GetAllAsync()
        {
          var Datas= (await repository.GetAllAsync()).ToList();

            return DatasControl(Datas);
        }

        public async Task<Response<T>> GetByIdAsync(int id)
        {
            var Datas = (await repository.GetByIdAsync(id));
            if (Datas == null)
                return Response<T>.Fail("bulunamadı", 200);
            return Response<T>.Success(Datas, 200);
        }

        public async Task<Response<bool>> UpdateAsync(T entity)
        {
            try
            {
                repository.UpdateAsync(entity);
                await _saves.SaveChangesAsync();
                return Response<bool>.Success(true, 200);
            }
            catch (Exception)
            {
                return Response<bool>.Fail("güncelleme gerçekleşmedi", 200);
            }
        }

        private Response<IEnumerable<T>> DatasControl(IList<T> Datas)
        {
            if (!Datas.Any())
            {
                return Response<IEnumerable<T>>.Fail("boş", 200);
            }
            return Response<IEnumerable<T>>.Success(Datas, 200);

        }
    }
}
