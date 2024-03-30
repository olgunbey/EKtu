using EKtu.Repository.Dtos;
using EKtu.Repository.IService.CacheService;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Infrastructure.CacheServices
{
    public class Cache<T> : ICache<T> where T : BaseDto, new()
    {
        private readonly IRedisClient redisClient;
        public Cache(IRedisClient redisClient)
        {
            this.redisClient = redisClient;
        }
        public Task<List<T>> GetCache(string cacheKey)
        {
           return Task.FromResult(redisClient.Get<List<T>>(cacheKey));
        }

        public Task SetCache(T data, string cacheKey)
        {
           List<T> cacheData = redisClient.Get<List<T>>(cacheKey);
            if(cacheData.Any())
            {
                if(cacheData.Any(y=>y.Equals(data))) //bu kullanıcıya ait refresh token var
                {
                    return Task.CompletedTask;
                }
                cacheData.Add(data);
                redisClient.Set(cacheKey, cacheData);
            }
            else
            {
                redisClient.Set(cacheKey, data);

            }
            return Task.CompletedTask;
        }
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
    }
}
