using BankomatClassLibrary;
using System.Text;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        using (var context = new ATMContext())
        {
            var atmService = new ATMService(context);
            var accountService = new AccountService(context);
            var transactionService = new TransactionService(context, atmService, accountService);

            atmService.SelectATM();
            AccountSelectionMenu(accountService, transactionService);
        }
    }

    static void AccountSelectionMenu(AccountService accountService, TransactionService transactionService)
    {
        while (true)
        {
            Console.WriteLine("Enter your card number:");
            string cardNumber = Console.ReadLine();

            if (!AuthenticationService.ValidateCardNumber(cardNumber))
            {
                Console.WriteLine("Invalid card number. It must be a 16-digit number.");
                continue;
            }

            Console.WriteLine("Enter your PIN:");
            string pinCode = Console.ReadLine();

            if (!AuthenticationService.ValidatePinCode(pinCode))
            {
                Console.WriteLine("Invalid PIN code. It must be a 4-digit number.");
                continue;
            }

            if (accountService.Login(cardNumber, pinCode))
            {
                AccountOperationMenu(accountService, transactionService);
                return;
            }
        }
    }

    static void AccountOperationMenu(AccountService accountService, TransactionService transactionService)
    {
        while (true)
        {
            Console.WriteLine("Account menu:");
            Console.WriteLine("1. Withdraw cash");
            Console.WriteLine("2. Deposit cash");
            Console.WriteLine("3. Transfer money");
            Console.WriteLine("4. Log out");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        transactionService.WithdrawCash();
                        break;
                    case 2:
                        transactionService.DepositCash();
                        break;
                    case 3:
                        transactionService.TransferMoney();
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("You are logged out.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
    }
}
