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
using MiAccount.Models.Request;
using MiAccount.Data.Repositories;

namespace MiAccount
{
    public class CreateAccount
    {
        private readonly IAccountRepository repo;

        public CreateAccount(IAccountRepository _repo)
        {
            repo = _repo;
        }

        [FunctionName("CreateAccount")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateAccountRequest>(requestBody);

            var now = DateTimeOffset.UtcNow;

            var newAccount = new Account
            {
                Name = data.Name,
                TokenTimeout = 1200,
                Active = true,
                CreatedById = 1,
                UpdatedById = 1,
                CreateTime = now,
                UpdateTime = now,
            };
            repo.InsertAccount(newAccount);
            await repo.Save();

            log.LogInformation("Account created.");
            return new OkObjectResult(newAccount.Id);
        }
    }
}
