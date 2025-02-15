using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AuthenticationService
{
    public static bool ValidateCardNumber(string cardNumber)
    {
        return IsValidInput(cardNumber, 16);
    }

    public static bool ValidatePinCode(string pinCode)
    {
        return IsValidInput(pinCode, 4);
    }

    private static bool IsValidInput(string input, int length)
    {
        return input.Length == length && input.All(char.IsDigit);
    }
}

