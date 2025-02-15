using BankomatClassLibrary;

public class AccountService
{
    private readonly ATMContext _context;
    public Account CurrentAccount { get; private set; }

    public AccountService(ATMContext context)
    {
        _context = context;
    }

    public bool Login(string cardNumber, string pinCode)
    {
        var account = _context.Accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return false;
        }

        account.LoginSucceeded += OnAuthenticationSucceeded;
        account.LoginFailed += OnAuthenticationFailed;

        if (account.Login(cardNumber, pinCode))
        {
            CurrentAccount = account;
            return true;
        }

        return false;
    }

    private void OnAuthenticationSucceeded(object sender, Account account)
    {
        Console.WriteLine($"Authentication successful! Welcome, {account.FirstName} {account.LastName}!");
    }

    private void OnAuthenticationFailed(object sender, EventArgs e)
    {
        Console.WriteLine("Incorrect card number or PIN. Please try again.");
    }
}
