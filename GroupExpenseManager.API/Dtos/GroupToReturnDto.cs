using System;

namespace GroupExpenseManager.API.Dtos
{
    public class GroupToReturnDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string GroupPhotoUrl { get; set; }
        public DateTime CreationDate { get; set; }
    }
}