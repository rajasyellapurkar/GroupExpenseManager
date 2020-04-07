using System.Collections.Generic;

namespace GroupExpenseManager.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public byte[] CategoryIcon { get; set; }
        public int? GroupId { get; set; }
        public bool IsParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }
    }
}