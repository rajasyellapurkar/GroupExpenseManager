using System.ComponentModel.DataAnnotations;

namespace GroupExpenseManager.API.Dtos
{
    public class AccountForCreationDto
    {
        [Required]
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountDescription { get; set; }
        public byte[] AccountIcon { get; set; }
    }
}