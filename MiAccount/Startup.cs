using System;
using MiAccount.Data;
using MiAccount.Services.AccountService;
using MiAccount.Services.RedisService;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
[assembly: FunctionsStartup(typeof(MiAccount.Startup))]

namespace MiAccount
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));

            builder.Services.AddScoped<IAccountService, AccountService>();

            var redisConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString");
            builder.Services.AddTransient(redis => new RedisService(redisConnectionString));
        }
    }
}
