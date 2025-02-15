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

    public interface ICommand
    {
        void Execute();
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
        var commandManager = new CommandManager(transactionService, accountService);

        while (true)
        {
            Console.WriteLine("Account menu:");
            Console.WriteLine("1. Withdraw cash");
            Console.WriteLine("2. Deposit cash");
            Console.WriteLine("3. Transfer money");
            Console.WriteLine("4. Log out");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 4) // If the user chooses to logout
                {
                    commandManager.ExecuteCommand(choice);
                    return;
                }
                commandManager.ExecuteCommand(choice);
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
    }

}
