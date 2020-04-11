using System.Threading.Tasks;
using GroupExpenseManager.API.Database.Context;
using GroupExpenseManager.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupExpenseManager.API.Database.Repository
{
    public class AccountRepository : BaseRepository,IAccountRepository
    {
        private readonly DataContext context;
        private readonly ILogger logger;
        public AccountRepository(DataContext context, ILogger<AccountRepository> logger):base(context,logger)
        {
            this.logger = logger;
            this.context = context;

        }

        public async Task<Account> GetAccount(int accountId)
        {
            logger.LogDebug($"Get account for id {accountId}");
            var account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            return account;
        }

        public async Task<Account> CreateNewAccount(Account account)
        {
            logger.LogDebug("Creating new Account for Group");
            LogProperties(account);

            Add(account);

            if (await SaveAll())
            {
                return account;
            }

            return null;
        }
    }
}