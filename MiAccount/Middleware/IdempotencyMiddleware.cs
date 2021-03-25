using System;
using System.Threading.Tasks;
using MiAccount.Services.RedisService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;

namespace MiView.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class IdempotencyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string IdempotencyKey = "idempotency-key";
        private IdempotencyChecker checker;

        public IdempotencyMiddleware(RequestDelegate next) //may need to move redis service injection back here.
        {
            _next = next;
            checker = IdempotencyChecker.GetInstance;
        }

        public async Task Invoke(HttpContext httpContext, IRedisService redisService)
        {
            if (httpContext.Request.Headers.ContainsKey(this.IdempotencyKey) && (httpContext.Request.Method == "POST" || httpContext.Request.Method == "PATCH"))
            {
                var value = "idempotency:" + httpContext.Request.Headers[this.IdempotencyKey].ToString();
                if (checker.Check(value, redisService.Database()))
                {
                    httpContext.Response.StatusCode = 409;
                    return;
                }

                try
                {
                    await _next(httpContext);

                    if (httpContext.Response.StatusCode >= 400)
                    {
                        redisService.Database().KeyDelete(value);
                    }
                }
                catch (Exception e)
                {
                    redisService.Database().KeyDelete(value);
                    throw e;
                }
            }
            else
            {
                await _next(httpContext);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static partial class MiddlewareExtensions
    {
        public static IApplicationBuilder UseIdempotencyProtection(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IdempotencyMiddleware>();
        }
    }

    public sealed class IdempotencyChecker
    {
        private static IdempotencyChecker _instance;
        private static readonly object padlock = new object();
        private IdempotencyChecker() { }
        public static IdempotencyChecker GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new IdempotencyChecker();
                    }
                    return _instance;
                }
            }
        }
        public bool Check(string value, IDatabase redis)
        {
            lock (padlock)
            {
                var exists = redis.KeyExists(value);
                if (exists == true)
                {
                    return true;
                }

                redis.StringSet(value, DateTime.UtcNow.ToString(), TimeSpan.FromDays(1));

                return false;
            }
        }
    }
}
