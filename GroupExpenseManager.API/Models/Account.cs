namespace GroupExpenseManager.API.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountDescription { get; set; }
        public byte[] AccountIcon { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }

    }
}