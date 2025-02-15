using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ICommand
{
    void Execute();
}

public class WithdrawCashCommand : ICommand
{
    private readonly TransactionService _transactionService;

    public WithdrawCashCommand(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public void Execute()
    {
        _transactionService.WithdrawCash();
    }
}

public class DepositCashCommand : ICommand
{
    private readonly TransactionService _transactionService;

    public DepositCashCommand(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public void Execute()
    {
        _transactionService.DepositCash();
    }
}

public class TransferMoneyCommand : ICommand
{
    private readonly TransactionService _transactionService;

    public TransferMoneyCommand(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public void Execute()
    {
        _transactionService.TransferMoney();
    }
}

public class LogoutCommand : ICommand
{
    public void Execute()
    {
        Console.Clear();
        Console.WriteLine("You are logged out.");
    }
}
