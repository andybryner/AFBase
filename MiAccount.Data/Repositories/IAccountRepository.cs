using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiAccount.Data.Repositories
{
    public interface IAccountRepository : IDisposable
    {
        IEnumerable<Account> GetAccounts();
        Task<Account> GetAccountById(int id);
        void InsertAccount(Account account);
        Task DeleteAccount(int id);
        Task UpdateAccount(Account account);
        Task Save();
    }
}
