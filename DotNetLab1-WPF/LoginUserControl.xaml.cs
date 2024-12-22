using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BankomatClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace DotNetLab1_WPF
{
    public partial class LoginUserControl : UserControl
    {
        public event EventHandler<Account> LoginSucceeded;

        private ATMContext context = new ATMContext();

        public LoginUserControl()
        {
            InitializeComponent();
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            string cardNumber = CardNumberInput.Text;
            string pinCode = PinInput.Password;

            var account = context.Accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
            if (account != null)
            {
                account.LoginSucceeded -= OnAuthenticationSucceeded;
                account.LoginFailed -= OnAuthenticationFailed;

                account.LoginSucceeded += OnAuthenticationSucceeded;
                account.LoginFailed += OnAuthenticationFailed;

                account.Login(cardNumber, pinCode);
            }
            else {
                MessageBox.Show("Account access error");
            }
        }

        private void OnAuthenticationSucceeded(object sender, Account account)
        {
            LoginSucceeded?.Invoke(this, account);
        }

        private void OnAuthenticationFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Incorrect card number or PIN.", "Authorization error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
