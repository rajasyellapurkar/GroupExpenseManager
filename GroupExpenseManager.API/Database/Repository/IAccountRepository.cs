using System.Threading.Tasks;
using GroupExpenseManager.API.Models;

namespace GroupExpenseManager.API.Database.Repository
{
    public interface IAccountRepository
    {
         Task<Account> GetAccount(int accountId);
         Task<Account> CreateNewAccount(Account account);
    }
}