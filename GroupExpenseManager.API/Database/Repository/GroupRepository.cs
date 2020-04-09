using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupExpenseManager.API.Database.Context;
using GroupExpenseManager.API.Helper;
using GroupExpenseManager.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupExpenseManager.API.Database.Repository
{
    public class GroupRepository : BaseRepository,IGroupRepository
    {
        private const int PageSize = 25;
        private readonly DataContext _context;
        private readonly ILogger<GroupRepository> _logger;

        public GroupRepository(DataContext context, ILogger<GroupRepository> logger):base(context,logger)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<Group> CreateNewExpenseSheet(Group group)
        {
            _logger.LogDebug("Creating new Expense Sheet");
            LogProperties<Group>(group);

            Add(group);

            if (await SaveAll())
            {
                return group;
            }

            return null;
        }

        public async Task<ICollection<Account>> GetGroupAccouts(int groupId)
        {
            _logger.LogDebug("GetGroupAccouts for group id:",groupId);
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group != null)
            {
                return group.Accounts;
            }
            return null;
        }

        public async Task<Group> GetGroupById(int groupId)
        {
            _logger.LogDebug("Get group details for group id: ", groupId);
            return await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
        }

        public async Task<ICollection<Category>> GetGroupCategories(int groupId)
        {
             _logger.LogDebug("Get Group Categories for group id: ", groupId);
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group != null)
            {
                return group.Categories;
            }
            return null;
        }

        public async Task<PagedList<Expense>> GetGroupExpenses(int groupId, int pageNumber)
        {
             _logger.LogDebug($"Get Group Expenses for group id:{0} and pageNumber {1}", groupId,pageNumber);
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group != null)
            {
                return await PagedList<Expense>.CreateAsync(group.Expenses.AsQueryable(),
                                                            pageNumber, PageSize);
            }
            return null;
        }

        public async Task<ICollection<User>> GetGroupUsers(int groupId)
        {
             _logger.LogDebug($"Get Group Users for group id:", groupId);
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group != null)
            {
                return group.UserGroups.Select(u => u.User).ToList();
            }
            return null;
        }

        public async Task<Group> UpdateGroup(Group group)
        {
             _logger.LogDebug($"Updating the group for following values");
             LogProperties<Group>(group);
            var groupFromRepo = await _context.Groups.FirstOrDefaultAsync(g => g.Id == group.Id);
            if (groupFromRepo != null)
            {
                groupFromRepo = group;
                if (await SaveAll())
                {
                    return groupFromRepo;
                }
            }
            return null;
        }

        public async Task<ICollection<User>> GetGroupApprovalList(int groupId)
        {
             _logger.LogDebug($"Get Group approvals for group id:", groupId);
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group != null)
            {
                return group.UserGroups.Where(u => u.IsApproval).Select(u => u.User).ToList();
            }
            return null;
        }

        public async Task<ICollection<User>> GetGroupAdmins(int groupId)
        {
             _logger.LogDebug($"Get Group admins for group id:", groupId);
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group != null)
            {
                return group.UserGroups.Where(u => u.IsAdmin).Select(u => u.User).ToList();
            }
            return null;
        }
        
    }
}