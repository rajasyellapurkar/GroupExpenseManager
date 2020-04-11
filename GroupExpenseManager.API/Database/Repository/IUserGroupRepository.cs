using System.Threading.Tasks;
using GroupExpenseManager.API.Models;

namespace GroupExpenseManager.API.Database.Repository
{
    public interface IUserGroupRepository
    {
         Task<UserGroup> AddNewUserGroup(UserGroup userGroup);
    }
}