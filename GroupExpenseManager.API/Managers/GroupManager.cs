using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupExpenseManager.API.Database.Repository;
using GroupExpenseManager.API.Models;
using Microsoft.Extensions.Logging;

namespace GroupExpenseManager.API.Managers
{
    public class GroupManager : IGroupManager
    {
        private readonly IGroupRepository groupRepo;
        private readonly ILogger<GroupManager> logger;
        private readonly IAccountRepository accountRepo;
        private readonly IUserGroupRepository userGroupRepo;
        public GroupManager(IGroupRepository groupRepo, ILogger<GroupManager> logger, 
        IAccountRepository accountRepo, IUserGroupRepository userGroupRepo)
        {
            this.userGroupRepo = userGroupRepo;
            this.accountRepo = accountRepo;
            this.logger = logger;
            this.groupRepo = groupRepo;
        }
        public async Task<Group> CreateNewGroup(Group group, int userId)
        {
            logger.LogDebug("Create a new group");
            var newGroup = await groupRepo.CreateNewExpenseSheet(group);
            if (newGroup.Id > 0 && await AddNewUserGroup(userId, newGroup))
            {
                return newGroup;
            }
            return null;
        }

        private async Task<bool> AddNewUserGroup(int userId, Group newGroup)
        {
            UserGroup userGroup = new UserGroup
            {
                UserId = userId,
                GroupId = newGroup.Id,
                IsAdmin = true,
                IsApproval = true,
                GroupRegistrationDate = DateTime.Now
            };

            return await userGroupRepo.AddNewUserGroup(userGroup) != null;
        }

        public async Task<Account> CreateNewAccount(Account account, int groupId, int userId)
        {
            logger.LogDebug($"Create a new account for group {groupId} and user {userId}");

            if (await isUserValid(groupId, userId))
            {
                account.GroupId = groupId;
                return await accountRepo.CreateNewAccount(account);
            }

            return null;
        }

        

        public async Task<Group> GetGroup(int groupId, int userId)
        {
            logger.LogDebug($"Get group with group id {groupId} and user id {userId}");
            if (await isUserValid(groupId, userId))
            {
                var group = await groupRepo.GetGroupById(groupId);
                return group;
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetGroupUsers(int groupId, int userId)
        {
            logger.LogDebug($"Get group users for group id {groupId} and user id {userId}");
            var users = await groupRepo.GetGroupUsers(groupId);
            if (users != null && users.Any(u => u.Id == userId))
            {
                return users;
            }
            return null;
        }

        public async Task<IEnumerable<Account>> GetGroupAccounts(int groupId, int userId)
        {
            logger.LogDebug($"Get group accounts for group id {groupId} and user id {userId}");
            if (await isUserValid(groupId, userId))
            {
                return await groupRepo.GetGroupAccounts(groupId);
            }
            return null;
        }

        public async Task<Account> GetGroupAccount(int groupId, int userId, int accountId)
        {
            logger.LogDebug($"Get group account for group id {groupId}, user id {userId} and account id {accountId}");
            if (await isUserValid(groupId, userId))
            {
                return await accountRepo.GetAccount(accountId);
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetGroupCategories(int groupId, int userId)
        {
            logger.LogDebug($"Get group categories for group id {groupId} and user id {userId}");
            if (await isUserValid(groupId, userId))
            {
                return await groupRepo.GetGroupCategories(groupId);
            }
            return null;
        }

        private async Task<bool> isUserValid(int groupId, int userId)
        {
            var users = await groupRepo.GetGroupUsers(groupId);
            return users != null && users.Any(u => u.Id == userId);
        }
    }
}