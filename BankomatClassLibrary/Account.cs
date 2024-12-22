using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankomatClassLibrary
{
    public class Account
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Key]
        public string CardNumber { get; set; }
        public string PinCode { get; set; }
        public decimal Balance { get; protected set; }

        public event EventHandler<decimal> BalanceChecked;
        public event EventHandler<decimal> FundsWithdrawnSucceded;
        public event EventHandler FundsWithdrawnFailed;
        public event EventHandler<decimal> FundsDeposited;
        public event EventHandler<Account> LoginSucceeded;
        public event EventHandler LoginFailed;

        public Account(string cardNumber, string firstName, string lastName, decimal balance, string pinCode)
        {
            CardNumber = cardNumber;
            FirstName = firstName;
            LastName = lastName;
            Balance = balance;
            PinCode = pinCode;
        }

        public bool Login(string cardNumber, string pinCode)
        {
            if (cardNumber == CardNumber && pinCode == PinCode)
            {
               LoginSucceeded?.Invoke(this, this);
                return true;
            }
            else
            {
                LoginFailed?.Invoke(this, EventArgs.Empty);
                return false;
            }
        }

        public decimal Check()
        {
            BalanceChecked?.Invoke(this, Balance);
            return Balance;
        }

        public bool FundsWithdraw(decimal amount, ATMContext context)
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                context.Entry(this).State = EntityState.Modified;
                context.SaveChanges();
                FundsWithdrawnSucceded?.Invoke(this, amount);
                return true;
            }
            else
            {
                FundsWithdrawnFailed?.Invoke(this, EventArgs.Empty);
                return false;
            }
        }

        public bool Trans(decimal amount, Account recipient, ATMContext context)
        {
            if (FundsWithdraw(amount, context))
            {
                recipient.Deposit(amount, context);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Deposit(decimal amount, ATMContext context)
        {
            Balance += amount;
            FundsDeposited?.Invoke(this, amount);

            context.SaveChanges();
        }

        public static List<(string CardNumber, string FirstName, string LastName, decimal Balance)> GetAccounts(ATMContext context)
        {
            var accounts = context.Accounts.Select(account => new
                {
                    account.CardNumber,
                    account.FirstName,
                    account.LastName,
                    account.Balance
                })
                .ToList().Select(account => (account.CardNumber, account.FirstName, account.LastName, account.Balance)).ToList();

            return accounts;
        }
    }
}
