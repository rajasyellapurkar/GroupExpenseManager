using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupExpenseManager.API.Database.Context;
using GroupExpenseManager.API.Helper;
using GroupExpenseManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupExpenseManager.API.Database.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private const int PageSize = 25;
        private readonly DataContext _context;

        public GroupRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Group> CreateNewExpenseSheet(Group group)
        {
            await _context.Groups.AddAsync(group);
            
            if(await SaveAll())
            {
                return group;
            }

            return null;            
        }

        public async Task<ICollection<Account>> GetGroupAccouts(int groupId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g=>g.Id == groupId);
            if(group != null)
            {
                return group.Accounts;
            }
            return null;
        }

        public async Task<Group> GetGroupById(int groupId)
        {
           return await _context.Groups.FirstOrDefaultAsync(g=>g.Id == groupId);
        }

        public async Task<ICollection<Category>> GetGroupCategories(int groupId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g=>g.Id == groupId);
            if(group != null)
            {
                return group.Categories;
            }
            return null;
        }

        public async Task<PagedList<Expense>> GetGroupExpenses(int groupId,int pageNumber)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g=>g.Id == groupId);
            if(group != null)
            {
                return await PagedList<Expense>.CreateAsync(group.Expenses.AsQueryable(),
                                                            pageNumber, PageSize);
            }
            return null;
        }

        public async Task<ICollection<User>> GetGroupUsers(int groupId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g=>g.Id == groupId);
            if(group != null)
            {
                return group.UserGroups.Select(u=>u.User).ToList();                
            }
            return null;
        }

        public async Task<Group> UpdateGroup(Group group)
        {
           var groupFromRepo = await _context.Groups.FirstOrDefaultAsync(g=>g.Id == group.Id);
           if(groupFromRepo !=null)
           {
               groupFromRepo = group;
               if(await SaveAll())
               {
                   return groupFromRepo;
               }
           }
           return null;
        }

        public async Task<ICollection<User>> GetGroupApprovalList(int groupId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g=>g.Id == groupId);
            if(group != null)
            {
                return group.UserGroups.Where(u=>u.IsApproval).Select(u=>u.User).ToList();                
            }
            return null;
        }

        public async Task<ICollection<User>> GetGroupAdmins(int groupId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g=>g.Id == groupId);
            if(group != null)
            {
                return group.UserGroups.Where(u=>u.IsAdmin).Select(u=>u.User).ToList();                
            }
            return null;
        } 
        private async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}