using BankomatClassLibrary;
using Microsoft.EntityFrameworkCore;

public class ATMService
{
    private readonly ATMContext _context;
    public AutomatedTellerMachine CurrentATM { get; private set; }

    public ATMService(ATMContext context)
    {
        _context = context;
    }

    public void SelectATM()
    {
        var atms = AutomatedTellerMachine.GetAtmsWithBanks(_context);
        Console.WriteLine("Select an ATM from the list:");
        for (int i = 0; i < atms.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {atms[i].AtmId} - {atms[i].Location} ({atms[i].BankName})");
        }

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int atmIndex) && atmIndex > 0 && atmIndex <= atms.Count)
            {
                CurrentATM = _context.ATMs.Include(atm => atm.BankATMs).FirstOrDefault(atm => atm.AtmId == atms[atmIndex - 1].AtmId);
                break;
            }
            else
            {
                Console.WriteLine("Enter the correct ATM number");
            }
        }
    }
}
