using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MiAccount.Services.RedisService
{
    public interface IRedisService
    {
        IDatabase Database();
        Task<bool> LockReleaseAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None);
        Task<bool> LockTakeAsync(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None);
        ConnectionMultiplexer Multiplexer();
    }
}