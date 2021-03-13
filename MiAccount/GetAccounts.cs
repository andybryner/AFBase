using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MiAccount.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MiAccount.Models.Response;

namespace MiAccount
{
    public class GetAccounts
    {
        private readonly ApplicationDbContext _context;

        public GetAccounts(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [FunctionName("GetAccounts")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetAccounts function processed a request.");

            var accounts = await _context.Accounts
                .Where(a => a.Active)
                .Select(a => new GetAccountResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    TokenTimeout = a.TokenTimeout
                })
                .ToListAsync();

            return new OkObjectResult(accounts);
        }
    }
}
