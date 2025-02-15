using BankomatClassLibrary;

public class TransactionService
{
    private readonly ATMContext _context;
    private readonly ATMService _atmService;
    private readonly AccountService _accountService;

    public TransactionService(ATMContext context, ATMService atmService, AccountService accountService)
    {
        _context = context;
        _atmService = atmService;
        _accountService = accountService;
    }

    public void WithdrawCash()
    {
        Console.WriteLine("Enter amount to withdraw:");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            if (_atmService.CurrentATM.WithdrawFromAtm(amount, _context))
                _accountService.CurrentAccount.FundsWithdraw(amount, _context);
        }
        else
        {
            Console.WriteLine("Invalid amount.");
        }
    }

    public void DepositCash()
    {
        Console.WriteLine("Enter amount to deposit:");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            _accountService.CurrentAccount.Deposit(amount, _context);
            _atmService.CurrentATM.ReplenishCash(amount, _context);
        }
        else
        {
            Console.WriteLine("Invalid amount.");
        }
    }

    public void TransferMoney()
    {
        Console.WriteLine("Enter recipient's card number:");
        string recipientCardNumber = Console.ReadLine();

        var recipientAccount = _context.Accounts.FirstOrDefault(a => a.CardNumber == recipientCardNumber);
        if (recipientAccount != null)
        {
            Console.WriteLine("Enter amount to transfer:");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                recipientAccount.FundsDeposited += (sender, amt) => Console.WriteLine($"Success! Your account was replenished by {amt} UAH!");
                _accountService.CurrentAccount.Trans(amount, recipientAccount, _context);
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }
        else
        {
            Console.WriteLine("Recipient account not found.");
        }
    }
}
