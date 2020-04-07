using System;

namespace GroupExpenseManager.API.Models
{
    public class UserGroup
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsApproval { get; set; }
        public DateTime GroupRegistrationDate { get; set; }
    }
}