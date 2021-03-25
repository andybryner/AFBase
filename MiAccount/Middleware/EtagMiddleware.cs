using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MiAccount.Middleware
{
    public class ETagMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string IfNoneMatch = "If-None-Match";
        private readonly string ETag = "ETag";
        private readonly MD5 _MD5 = MD5.Create();

        public ETagMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Method == "GET")
            {
                string value = "";

                //the original response body is a Write-Only Stream, we have to switch it for a read-write one temporarily then copy the data back to the original.
                Stream OriginalBody = httpContext.Response.Body;
                using (var readableStream = new MemoryStream())
                {
                    httpContext.Response.Body = readableStream;

                    if (httpContext.Request.Headers.ContainsKey(this.IfNoneMatch))
                    {
                        value = httpContext.Request.Headers[this.IfNoneMatch].ToString();

                    }
                    //finish the Request Part
                    await _next(httpContext);
                    //Response Starts here

                    //Pepare for read
                    readableStream.Position = 0;
                    var hashedResponseBodyValue = HashToString(_MD5.ComputeHash(readableStream));
                    if (httpContext.Response.Headers.ContainsKey(ETag))
                    {
                        httpContext.Response.Headers[ETag] = hashedResponseBodyValue;
                    }
                    else
                    {
                        httpContext.Response.Headers.Add(ETag, hashedResponseBodyValue);
                    }

                    if (httpContext.Request.Headers.ContainsKey(this.IfNoneMatch) && value == hashedResponseBodyValue)
                    {
                        httpContext.Response.StatusCode = 304;
                        return;
                    }

                    //Write the response we read and hashed earlier to the original body and replace it in the context.
                    readableStream.Position = 0;
                    await readableStream.CopyToAsync(OriginalBody);
                    httpContext.Response.Body = OriginalBody;
                }
            }
            else
            {
                await _next(httpContext);
            }
        }

        private string HashToString(byte[] hash)
        {
            var sb = new StringBuilder();
            hash.ToList().ForEach(x => sb.Append(x.ToString("x2")));
            return sb.ToString();
        }
    }
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static partial class MiddlewareExtensions
    {
        public static IApplicationBuilder UseETagMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ETagMiddleware>();
        }
    }
}
