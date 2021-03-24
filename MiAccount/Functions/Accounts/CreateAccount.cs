using System.IO;
using System.Threading.Tasks;
using MiAccount.Models.Request;
using MiAccount.Services.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MiAccount
{
    public class CreateAccount
    {
        private readonly IAccountService _accountService;

        public CreateAccount(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [FunctionName("CreateAccount")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "accounts")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateAccountRequest>(requestBody);

            var newAccountId = await _accountService.CreateAccount(data);

            log.LogInformation("Account created.");
            return new OkObjectResult(newAccountId);
        }
    }
}
