using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AuthenticationService
{
    public static bool ValidateCardNumber(string cardNumber)
    {
        return cardNumber.Length == 16 && cardNumber.All(char.IsDigit);
    }

    public static bool ValidatePinCode(string pinCode)
    {
        return pinCode.Length == 4 && pinCode.All(char.IsDigit);
    }
}
