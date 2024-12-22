using System.ComponentModel.DataAnnotations;

namespace BankomatClassLibrary
{
    public class Bank
    {
        [Key]
        public string BankName { get; set; }
        public ICollection<BankATM> BankATMs { get; set; }

        public Bank(string bankName)
        {
            BankName = bankName;
            BankATMs = new List<BankATM>();
        }
    }
}
