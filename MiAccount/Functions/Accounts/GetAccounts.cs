using System.Threading.Tasks;
using MiAccount.Services.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace MiAccount
{
    public class GetAccounts
    {
        private readonly IAccountService _accountService;

        public GetAccounts(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [FunctionName("GetAccounts")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "accounts")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetAccounts function processed a request.");

            var accounts = await _accountService.GetAccounts();

            return new OkObjectResult(accounts);
        }
    }
}
