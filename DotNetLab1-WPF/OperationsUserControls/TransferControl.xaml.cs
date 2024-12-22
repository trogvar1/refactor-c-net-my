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

namespace DotNetLab1_WPF.OperationsUserControls
{
    public partial class TransferControl : UserControl
    {
        public event EventHandler<(string, decimal)> ConfirmTransfer;

        public TransferControl()
        {
            InitializeComponent();
        }

        private void OnConfirmClick(object sender, RoutedEventArgs e)
        {
            string recipientCardNumber = RecipientCardNumber.Text;
            if (decimal.TryParse(TransferAmount.Text, out decimal amount) && amount > 0 && !string.IsNullOrWhiteSpace(recipientCardNumber))
            {
                ConfirmTransfer?.Invoke(this, (recipientCardNumber, amount));
            }
            else
            {
                MessageBox.Show("Введіть коректні дані.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
