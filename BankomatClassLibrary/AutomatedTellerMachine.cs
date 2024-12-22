using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankomatClassLibrary
{
    [Table("ATMs")]
    public class AutomatedTellerMachine
    {
        [Key]
        public string AtmId { get; set; }
        public string Location { get; set; }
        public decimal CashAvailable { get; protected set; }
        public ICollection<BankATM> BankATMs { get; set; }

        public event EventHandler<decimal> CashWithdrawnSucceded;
        public event EventHandler CashWithdrawnFailed;
        public event EventHandler<decimal> CashReplenished;

        public AutomatedTellerMachine(string atmId, string location, decimal cashAvailable)
        {
            AtmId = atmId;
            Location = location;
            CashAvailable = cashAvailable;
            BankATMs = new List<BankATM>();
        }

        public bool WithdrawFromAtm(decimal amount, ATMContext context)
        {
            if (amount <= CashAvailable)
            {
                CashAvailable -= amount;
                CashWithdrawnSucceded?.Invoke(this, amount);
                context.SaveChanges();
                return true;
            }
            CashWithdrawnFailed?.Invoke(this, EventArgs.Empty);
            return false;
        }

        public void ReplenishCash(decimal amount, ATMContext context)
        {
            CashAvailable += amount;
            CashReplenished?.Invoke(this, amount);
            context.SaveChanges();
        }

        public static List<(string AtmId, string Location, decimal CashAvailable, string BankName)> GetAtmsWithBanks(ATMContext context)
        {
            var atmsWithBanks = context.ATMs.Include(atm => atm.BankATMs).ThenInclude(bankAtm => bankAtm.Bank).Select(atm => new
                {
                    atm.AtmId,
                    atm.Location,
                    atm.CashAvailable,
                    BankName = atm.BankATMs.FirstOrDefault().Bank.BankName
                })
                .ToList().Select(atm => (atm.AtmId, atm.Location, atm.CashAvailable, atm.BankName)).ToList();

            return atmsWithBanks;
        }
    }
}
