using BankomatClassLibrary;
using System.Text;
using Microsoft.EntityFrameworkCore;

class Program
{
    static AutomatedTellerMachine currentATM;
    static Account currentAccount;
    static ATMContext context = new ATMContext();

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        ATMSelectionMenu();
        AccountSelectionMenu();
    }
    static void ATMSelectionMenu()
    {
        var atms = AutomatedTellerMachine.GetAtmsWithBanks(context);
        Console.WriteLine("Select an ATM from the list:");
        for (int i = 0; i < atms.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {atms[i].AtmId} - {atms[i].Location} ({atms[i].BankName})");
        }
        while (true)
        {

            if (int.TryParse(Console.ReadLine(), out int atmIndex) && atmIndex > 0 && atmIndex <= atms.Count)
            {
                var selectedAtm = atms[atmIndex - 1];
                currentATM = context.ATMs.Include(atm => atm.BankATMs).FirstOrDefault(atm => atm.AtmId == selectedAtm.AtmId);
                break;
            }
            else
            {
                Console.WriteLine("Enter the correct ATM number");
            }
        }
    }
    static void AccountSelectionMenu()
    {
        while (true)
        {
            Console.WriteLine("Enter your card number:");
            string cardNumber = Console.ReadLine();

            if (cardNumber.Length == 16 && cardNumber.All(char.IsDigit))
            {
                Console.WriteLine("Enter your PIN:");
                string pinCode = Console.ReadLine();

                if (pinCode.Length == 4 && pinCode.All(char.IsDigit))
                {
                    var account = context.Accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
                    if (account != null)
                    {
                        account.LoginSucceeded += OnAuthenticationSucceeded;
                        account.LoginFailed += OnAuthenticationFailed;

                        if (account.Login(cardNumber, pinCode))
                        {
                            currentAccount = account;
                            AccountOperationMenu();
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Account with this card number was not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect PIN code. It must be a 4-digit number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid card number. It must be a 16-digit number.");
            }
        }
    }

    static void OnAuthenticationSucceeded(object sender, Account currentAccount)
    {
        Console.WriteLine($"Authentication was successful! Congratulations, user {currentAccount.FirstName} {currentAccount.LastName}!");
    }
    static void OnAuthenticationFailed(object sender, EventArgs e)
    {
        Console.WriteLine("The card number or PIN is incorrect. Please try again.");
    }

    static void AccountOperationMenu()
    {
        currentAccount.BalanceChecked += OnBalanceChecked;
        currentAccount.FundsWithdrawnSucceded += OnFundsWithdrawnSucceded;
        currentAccount.FundsWithdrawnFailed += OnFundsWithdrawnFailed;
        currentATM.CashWithdrawnFailed += OnCashWithdrawnFailed;
        currentAccount.FundsDeposited += OnFundsDeposited;

        while (true)
        {
            Console.WriteLine("Account menu:");
            Console.WriteLine("1. Check your balance");
            Console.WriteLine("2. Withdraw cash");
            Console.WriteLine("3. Top up with cash");
            Console.WriteLine("4. Transfer money");
            Console.WriteLine("5. Log out of your account");
            Console.WriteLine("Select an action:");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        currentAccount.Check();
                        break;
                    case 2:
                        WithdrawCash();
                        break;
                    case 3:
                        DepositCash();
                        break;
                    case 4:
                        TransferMoney();
                        break;
                    case 5:
                        currentAccount = null;
                        Console.Clear();
                        Console.WriteLine("You are logged out of your account. Back to the authorization menu.");
                        AccountSelectionMenu();
                        return;
                    default:
                        Console.WriteLine("Wrong choice, try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Incorrect input, please try again.");
            }
        }
    }

    static void OnBalanceChecked(object sender, decimal balance)
    {
        Console.WriteLine($"Your balance: {balance} UAH");
    }
    static void OnFundsWithdrawnFailed(object sender, EventArgs e)
    {
        Console.WriteLine("Insufficient funds in the account.");
    }
    static void OnFundsWithdrawnSucceded(object sender, decimal amount)
    {
        Console.WriteLine($"Success! UAH {amount} was withdrawn from the card balance!");
    }
    static void OnCashWithdrawnFailed(object sender, EventArgs e)
    {
        Console.WriteLine("Insufficient funds in the ATM. The operation is canceled");
    }
    static void OnFundsDeposited(object sender, decimal amount)
    {
        Console.WriteLine($"Success! Your account has been replenished by {amount} UAH!");
    }

    static void WithdrawCash()
    {
        Console.WriteLine("Enter the amount to withdraw:");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            if (currentATM.WithdrawFromAtm(amount, context))
                currentAccount.FundsWithdraw(amount, context);
        }
        else
        {
            Console.WriteLine("Incorrect amount entered.");
        }
    }

    static void DepositCash()
    {
        Console.WriteLine("Enter the amount to deposit:");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            currentAccount.Deposit(amount, context);
            currentATM.ReplenishCash(amount, context);
        }
        else
        {
            Console.WriteLine("The amount entered is incorrect.");
        }
    }

    static void TransferMoney()
    {
        Console.WriteLine("Enter the recipient's card number:");
        string recipientCardNumber = Console.ReadLine();

        var recipientAccount = context.Accounts.FirstOrDefault(a => a.CardNumber == recipientCardNumber);
        if (recipientAccount != null)
        {
            Console.WriteLine("Enter the amount to transfer:");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                recipientAccount.FundsDeposited += OnFundsDeposited;
                currentAccount.Trans(amount, recipientAccount, context);
            }
            else
            {
                Console.WriteLine("Incorrect amount.");
            }
        }
        else
        {
            Console.WriteLine("Recipient account not found.");
        }
    }
}