using System;
using System.Collections.Generic;

namespace GroupExpenseManager.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime LastActive { get; set; }
        public int ActiveLogins { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}