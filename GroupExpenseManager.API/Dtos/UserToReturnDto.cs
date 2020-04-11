using System;
using System.Collections.Generic;
using GroupExpenseManager.API.Models;

namespace GroupExpenseManager.API.Dtos
{
    public class UserToReturnDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastActive { get; set; }
        public string PhotoUrl { get; set; }
        public string Age { get; set; }
        public Gender Gender { get; set; }
    }
}