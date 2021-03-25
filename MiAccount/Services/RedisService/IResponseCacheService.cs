using System;
using System.Threading.Tasks;

namespace MiAccount.Services.ResponseCacheService
{
    public interface IResponseCacheService
    {
        Task<bool> CacheResponseAsync(string key, object response, TimeSpan duration);
        Task<string> GetCachedResponseAsync(string key);
        void RemoveResponseCachesByPattern(string pattern = "");
        void RemoveResponseCaches(long account, string host, string pathPrefix = "");
    }
}
