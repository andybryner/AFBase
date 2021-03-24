using System.Collections.Generic;
using System.Threading.Tasks;
using MiAccount.Models.Request;
using MiAccount.Models.Response;

namespace MiAccount.Services.AccountService
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountResponse>> GetAccounts();
        Task<long> CreateAccount(CreateAccountRequest account);
    }
}
