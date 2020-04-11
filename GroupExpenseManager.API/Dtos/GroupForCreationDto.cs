using System;
using System.ComponentModel.DataAnnotations;

namespace GroupExpenseManager.API.Dtos
{
    public class GroupForCreationDto
    {
        [Required]
        public string GroupName { get; set; }
        [Required]
        public string GroupDescription { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public GroupForCreationDto()
        {
            CreationDate = DateTime.Now;
        }
    }
}