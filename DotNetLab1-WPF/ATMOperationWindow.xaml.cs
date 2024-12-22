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
using System.Windows.Shapes;
using BankomatClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace DotNetLab1_WPF
{
    public partial class ATMOperationWindow : Window
    {
        private AutomatedTellerMachine selectedATM;
        private Account currentAccount;
        private ATMContext context;

        public ATMOperationWindow(AutomatedTellerMachine atm, ATMContext _context)
        {
            InitializeComponent();
            selectedATM = atm;
            context = _context;
            ShowLoginControl();
        }

        private void ShowLoginControl()
        {
            var loginControl = new LoginUserControl();
            loginControl.LoginSucceeded += OnLoginSucceeded;
            UserControlHost.Content = loginControl;
        }

        private void OnLoginSucceeded(object sender, Account account)
        {
            currentAccount = account;
            AuthenticatedUser.Text = $"Greetings, {currentAccount.LastName} {currentAccount.FirstName}!";
            ShowOperationsControl();
        }
        private void ShowOperationsControl()
        {
            var operationsControl = new ATMOperationsControl(currentAccount, selectedATM, context);
            operationsControl.ExitRequested += OnExitRequested;
            UserControlHost.Content = operationsControl;
        }

        private void OnExitRequested(object sender, EventArgs e)
        {
            currentAccount = null;
            AuthenticatedUser.Text = "";
            ShowLoginControl();
        }
    }
}
