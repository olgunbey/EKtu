using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.ICacheService
{
    public interface ICache
    {
        Task SetCache<T>(IEnumerable<T> data, string cacheKey);
        Task<T> GetCache<T>(string cacheKey);
    }
}
