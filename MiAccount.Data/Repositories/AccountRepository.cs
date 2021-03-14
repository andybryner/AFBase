using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiAccount.Data.Repositories
{
    class AccountRepository : IAccountRepository, IDisposable
    {
        private ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _context.Accounts.Where(x => x.Active).AsEnumerable();
        }

        public async Task<Account> GetAccountById(int id)
        {
            return await _context.Accounts.SingleAsync(x => x.Id == id);
        }

        public void InsertAccount(Account account)
        {
            _context.Add(account);
        }

        public async Task DeleteAccount(int id)
        {
            var account = await _context.Accounts.SingleAsync(x => x.Id == id);
            account.Active = false;
        }

        public async Task UpdateAccount(Account account)
        {
            var dbAccount = await _context.Accounts.FindAsync(account.Id);
            _context.Entry(dbAccount).CurrentValues.SetValues(account);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
