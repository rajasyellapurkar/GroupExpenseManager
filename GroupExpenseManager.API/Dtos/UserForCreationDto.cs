using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GroupExpenseManager.API.Dtos
{
    public class UserForCreationDto
    {
        public UserForCreationDto()
        {
            RegisteredDate = DateTime.Now;
            LastActive = DateTime.Now;
        }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime LastActive { get; set; }
    }
}