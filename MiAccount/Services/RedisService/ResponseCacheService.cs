using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiAccount.Utils;
using Microsoft.Extensions.Logging;
using MiAccount.Services.RedisService;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiAccount.Services.ResponseCacheService
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IRedisService _redis;
        private readonly ILogger _logger;

        public ResponseCacheService(IRedisService redis, ILogger logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public async Task<bool> CacheResponseAsync(string key, object response, TimeSpan duration)
        {
            if (response == null)
            {
                return false;
            }

            var serializedResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });

            try
            {
                return await _redis.Database().StringSetAsync(key, serializedResponse, expiry: duration);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Redis Error");
                return false;
            }
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            try
            {
                var cachedResponse = await _redis.Database().StringGetAsync(key);
                return cachedResponse.HasValue ? cachedResponse.ToString() : null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Redis Error");
            }
            return null;
        }

        public void RemoveResponseCachesByPattern(string pattern = "")
        {
            var db = _redis.Database();
            var mp = _redis.Multiplexer();
            var endpoints = mp.GetEndPoints();

            try
            {
                foreach (var endpoint in endpoints)
                {
                    var s = mp.GetServer(endpoint);
                    var keys = s.Keys(pattern: "RESPONSE:" + pattern);

                    keys.BatchExecute(100, b => BatchRemoveKeys(b, db));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Redis Error");
            }
        }

        public void RemoveResponseCaches(long account, string host, string pathPrefix = "")
        {
            var pattern = $"{account.ToString()}:{host}{pathPrefix}*";
            RemoveResponseCachesByPattern(pattern);
        }


        private void BatchRemoveKeys(IEnumerable<StackExchange.Redis.RedisKey> keys, StackExchange.Redis.IDatabase db)
        {
            var addTasks = new List<Task>();
            var batch = db.CreateBatch();
            foreach (var k in keys)
            {
                var addAsync = batch.KeyDeleteAsync(k);
                addTasks.Add(addAsync);
            }
            batch.Execute();
            var tasks = addTasks.ToArray();
            Task.WaitAll(tasks);
        }
    }
}
