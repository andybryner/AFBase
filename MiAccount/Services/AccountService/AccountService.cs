using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiAccount.Data;
using MiAccount.Models.Request;
using MiAccount.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace MiAccount.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccountResponse>> GetAccounts()
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.Active)
                .Select(a => new AccountResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    TokenTimeout = a.TokenTimeout
                })
                .ToListAsync();
        }

        public async Task<long> CreateAccount(CreateAccountRequest account)
        {
            var now = DateTimeOffset.UtcNow;

            var newAccount = new Account
            {
                Name = account.Name,
                TokenTimeout = account.TokenTimeout,
                Active = true,
                CreatedById = 1,
                UpdatedById = 1,
                CreateTime = now,
                UpdateTime = now,
            };

            _context.Add(newAccount);

            await _context.SaveChangesAsync();

            return newAccount.Id;
        }
    }
}
