using EKtu.Domain.Entities;
using EKtu.Repository.ICacheService;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EKtu.CacheService.CacheServices
{
    public class BaseCache : ICache
    {
        private readonly IRedisClientAsync _redisClient;
        public BaseCache(IRedisClientAsync redisClient)
        {
            _redisClient=redisClient;
        }
        public async Task<T> GetCache<T>(string cacheKey)
        {
           return await _redisClient.GetAsync<T>(cacheKey);
        }

        public async Task SetCache<T>(IEnumerable<T> data, string cacheKey) //burada kaldım
        {
          var serializeData=  JsonSerializer.Serialize(data);

         bool hasCheck= await _redisClient.SetAsync(cacheKey, serializeData);
            if (!hasCheck)
                throw new NotImplementedException("");//burada mıddleawareden yakalayıp kodla

        }
    }
}
