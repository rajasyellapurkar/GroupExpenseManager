using System.Collections.Generic;
using System.Threading.Tasks;
using GroupExpenseManager.API.Helper;
using GroupExpenseManager.API.Models;

namespace GroupExpenseManager.API.Database.Repository
{
    public interface IGroupRepository
    {
         Task<Group> CreateNewExpenseSheet(Group group);
         Task<Group> GetGroupById(int groupId);
         Task<Group> UpdateGroup(Group group);
         Task<PagedList<Expense>> GetGroupExpenses(int groupId,int pageNumber);
         Task<ICollection<User>> GetGroupApprovalList(int groupId);
         Task<ICollection<User>> GetGroupAdmins(int groupId);
         Task<IEnumerable<User>> GetGroupUsers(int groupId);
         Task<ICollection<Category>> GetGroupCategories(int groupId);
         Task<ICollection<Account>> GetGroupAccounts(int groupId);
    }
}