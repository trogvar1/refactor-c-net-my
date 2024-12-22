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
    public partial class WithdrawControl : UserControl
    {
        public event EventHandler<decimal> ConfirmWithdraw;

        public WithdrawControl()
        {
            InitializeComponent();
        }

        private void OnConfirmClick(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(WithdrawAmount.Text, out decimal amount) && amount > 0)
            {
                ConfirmWithdraw?.Invoke(this, amount);
            }
            else
            {
                MessageBox.Show("Введіть коректну суму.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
