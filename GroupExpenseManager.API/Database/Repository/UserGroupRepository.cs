using System.Threading.Tasks;
using GroupExpenseManager.API.Database.Context;
using GroupExpenseManager.API.Models;
using Microsoft.Extensions.Logging;

namespace GroupExpenseManager.API.Database.Repository
{
    public class UserGroupRepository : BaseRepository, IUserGroupRepository
    {
        private readonly DataContext context;
        private readonly ILogger<UserGroupRepository> logger;

        public UserGroupRepository(DataContext context, ILogger<UserGroupRepository> logger):base(context,logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public async Task<UserGroup> AddNewUserGroup(UserGroup userGroup)
        {
            logger.LogDebug("Creating new user group with following details: ");
            LogProperties(userGroup);

            Add(userGroup);

            if (await SaveAll())
            {
                return userGroup;
            }

            return null;
        }
    }
}