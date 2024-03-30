using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.CacheService
{
    public interface ICache<T> where T : class,new()
    {
        Task SetCache(T data, string cacheKey);
        Task<List<T>> GetCache(string cacheKey);
    }
}
