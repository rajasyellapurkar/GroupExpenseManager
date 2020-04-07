using System;
using System.Collections.Generic;

namespace GroupExpenseManager.API.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string AdditionalPaymentDetails { get; set; }
        public string Description { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Approval> Approvals { get; set; }
        public DateTime CreationDateTime { get; set; }
        public int CreatorUserId { get; set; }
        public User CreatedBy { get; set; }
        public int UpdatorUserId { get; set; }
        public User UpdatedBy { get; set; }
        public string Payer { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}