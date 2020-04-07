namespace GroupExpenseManager.API.Models
{
    public class Approval
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ApprovalStatus Status { get; set; }
        public int ExpenseId { get; set; }
        public Expense Expense { get; set; }
        public string Comments { get; set; }
    }
}