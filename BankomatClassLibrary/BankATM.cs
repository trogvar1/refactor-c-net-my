using System.ComponentModel.DataAnnotations;

namespace BankomatClassLibrary { 
    public class BankATM
    {
        public string BankName { get; set; }
        public Bank Bank { get; set; }

        public string AtmId { get; set; }
        public AutomatedTellerMachine ATM { get; set; }
    }
}