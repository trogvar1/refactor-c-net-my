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
using DotNetLab1_WPF.OperationsUserControls;

namespace DotNetLab1_WPF
{
    public partial class ATMOperationsControl : UserControl
    {
        private Account currentAccount;
        private AutomatedTellerMachine currentATM;
        private ATMContext context;

        public event EventHandler ExitRequested;

        public ATMOperationsControl(Account account, AutomatedTellerMachine atm, ATMContext _context)
        {
            InitializeComponent();
            currentAccount = account;
            currentATM = atm;
            context = _context;
        }

        private void OnCheckBalanceClick(object sender, RoutedEventArgs e)
        {
            currentAccount.BalanceChecked -= OnBalanceChecked;
            currentAccount.BalanceChecked += OnBalanceChecked;
            currentAccount.Check();
        }
        private void OnBalanceChecked(object sender, decimal balance)
        {
            MessageBox.Show($"Your balance: {balance} UAH", "Balance", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnWithdrawClick(object sender, RoutedEventArgs e)
        {
            ShowWithdrawControl();
        }

        private void OnDepositClick(object sender, RoutedEventArgs e)
        {
            ShowDepositControl();
        }

        private void OnTransferClick(object sender, RoutedEventArgs e)
        {
            ShowTransferControl();
        }
        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            ExitRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ShowWithdrawControl()
        {
            var withdrawControl = new WithdrawControl();
            withdrawControl.ConfirmWithdraw += OnConfirmWithdraw;
            OperationField.Children.Clear();
            OperationField.Children.Add(withdrawControl);
        }
        private void OnConfirmWithdraw(object sender, decimal amount)
        {
            currentATM.CashWithdrawnFailed -= OnATMCashNoEnough;
            currentATM.CashWithdrawnFailed += OnATMCashNoEnough;
            if (currentATM.WithdrawFromAtm(amount, context))
            {
                currentAccount.FundsWithdrawnSucceded -= OnFundsWithdrawnSucceded;
                currentAccount.FundsWithdrawnFailed -= OnFundsWithdrawnFailed;
                currentAccount.FundsWithdrawnSucceded += OnFundsWithdrawnSucceded;
                currentAccount.FundsWithdrawnFailed += OnFundsWithdrawnFailed;
                currentAccount.FundsWithdraw(amount, context);
            }
        }
        private void OnFundsWithdrawnSucceded(object sender, decimal amount)
        {
            MessageBox.Show($"Withdrawn  {amount}  UAH.", "Withdrawal of funds", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void OnFundsWithdrawnFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Insufficient funds in the account.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void OnATMCashNoEnough(object sender, EventArgs e)
        {
            MessageBox.Show("Insufficient funds in the ATM.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowDepositControl()
        {
            var depositControl = new DepositControl();
            depositControl.ConfirmDeposit += OnConfirmDeposit;
            OperationField.Children.Clear();
            OperationField.Children.Add(depositControl);
        }
        private void OnConfirmDeposit(object sender, decimal amount)
        {
            currentAccount.FundsDeposited -= OnFundDeposited;
            currentAccount.FundsDeposited += OnFundDeposited;
            currentAccount.Deposit(amount, context);
            currentATM.ReplenishCash(amount, context);
        }
        private void OnFundDeposited(object sender, decimal amount)
        {
            MessageBox.Show($"The account has been replenished by {amount} UAH.", "Deposit of funds", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowTransferControl()
        {
            var transferControl = new TransferControl();
            transferControl.ConfirmTransfer += OnConfirmTransfer;
            OperationField.Children.Clear();
            OperationField.Children.Add(transferControl);
        }
        private void OnConfirmTransfer(object sender, (string recipientCardNumber, decimal amount) transferDetails)
        {
            var recipientAccount = context.Accounts.FirstOrDefault(a => a.CardNumber == transferDetails.recipientCardNumber);
            if (recipientAccount != null)
            {
                currentAccount.FundsWithdrawnSucceded -= OnFundsWithdrawnSucceded;
                currentAccount.FundsWithdrawnFailed -= OnFundsWithdrawnFailed;
                currentAccount.FundsWithdrawnSucceded += OnFundsWithdrawnSucceded;
                currentAccount.FundsWithdrawnFailed += OnFundsWithdrawnFailed;
                currentAccount.Trans(transferDetails.amount, recipientAccount, context);
            }
            else
            {
                MessageBox.Show("Recipient not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
