using System;

namespace GroupExpenseManager.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        public Expense Expense { get; set; }
        public int? ExpenseId { get; set; }
        public Group Group { get; set; }
        public int? GroupId { get; set; }
        public User User { get; set; }
        public int? UserId { get; set; }
    }
}