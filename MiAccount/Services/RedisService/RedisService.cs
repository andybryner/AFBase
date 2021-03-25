using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MiAccount.Services.RedisService
{
    public class RedisService : IRedisService
    {
        private static string _connectionString { get; set; }

        public RedisService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private static readonly Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(_connectionString);
        });

        public async Task<bool> LockTakeAsync(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            return await lazyConnection.Value.GetDatabase().LockTakeAsync(key, value, expiry, flags);
        }

        public async Task<bool> LockReleaseAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await lazyConnection.Value.GetDatabase().LockReleaseAsync(key, value, flags);
        }

        public IDatabase Database()
        {
            return lazyConnection.Value.GetDatabase();
        }

        public ConnectionMultiplexer Multiplexer()
        {
            return lazyConnection.Value;
        }
    }
}
