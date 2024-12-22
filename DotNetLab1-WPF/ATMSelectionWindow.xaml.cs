using System;
using System.Text;
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
    public partial class ATMSelectionWindow : Window
    {
        static public ATMContext context = new ATMContext();
        public ATMSelectionWindow()
        {
            InitializeComponent();
            LoadATMs();
        }

        private void LoadATMs()
        {
            var atms = AutomatedTellerMachine.GetAtmsWithBanks(context);
            ATMList.ItemsSource = atms.Select(atm => $"{atm.AtmId} - {atm.Location} ({atm.BankName})").ToList();
        }

        private void ATMButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string selectedATMInfo = clickedButton.Content.ToString();

            var selectedAtm = context.ATMs.Include(atm => atm.BankATMs)
                              .FirstOrDefault(atm => selectedATMInfo.Contains(atm.AtmId.ToString()));

            if (selectedAtm != null)
            {
                ATMOperationWindow operationWindow = new ATMOperationWindow(selectedAtm, context);
                operationWindow.Show();
                this.Close();
            }
        }
    }
}