using System;
using System.Collections.Generic;

namespace GroupExpenseManager.API.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string GroupPhotoUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<Category> Categories { get; set; }
        
    }
}