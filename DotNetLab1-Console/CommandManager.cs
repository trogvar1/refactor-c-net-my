using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CommandManager
{
    private readonly Dictionary<int, ICommand> _commands;

    public CommandManager(TransactionService transactionService, AccountService accountService)
    {
        _commands = new Dictionary<int, ICommand>
        {
            { 1, new WithdrawCashCommand(transactionService) },
            { 2, new DepositCashCommand(transactionService) },
            { 3, new TransferMoneyCommand(transactionService) },
            { 4, new LogoutCommand() }
            // Якщо додаєте перевірку балансу або інші операції, додайте нові команди сюди
        };
    }

    public void ExecuteCommand(int choice)
    {
        if (_commands.ContainsKey(choice))
        {
            _commands[choice].Execute();
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}
