using System.Collections.Generic;
using System.Threading.Tasks;
using GroupExpenseManager.API.Dtos;
using GroupExpenseManager.API.Models;

namespace GroupExpenseManager.API.Managers
{
    public interface IGroupManager
    {
         Task<Group> CreateNewGroup(Group group,int userId);
         Task<Account> CreateNewAccount(Account account, int groupId, int userId);
         Task<Group> GetGroup(int groupId, int userId);
         Task<IEnumerable<User>> GetGroupUsers(int groupId, int userId);
         Task<IEnumerable<Account>> GetGroupAccounts(int groupId, int userId);
         Task<Account> GetGroupAccount(int groupId, int userId, int accountId);
         Task<IEnumerable<Category>> GetGroupCategories(int groupId, int userId);
    }
}