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
using WorkWithData.Domain.models;

namespace WorkWithData.windows
{
    /// <summary>
    /// Interaction logic for CreateTransaction.xaml
    /// </summary>
    public partial class CreateTransaction : Window
    {
        public CoffeeTransaction ResultTransaction { get; private set; }
        public CreateTransaction()
        {
            InitializeComponent();
            cb_cashtype.SelectedIndex = 0;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string coffee = string.IsNullOrWhiteSpace(tb_coffee.Text) ? throw new Exception("Coffee name cannot be empty") : tb_coffee.Text;
                string cashType = (cb_cashtype.SelectedItem as ComboBoxItem)?.Content.ToString();
                var date = dp_date.SelectedDate ?? DateTime.Now;
                if (!double.TryParse(tb_money.Text, out double money))
                    throw new Exception("Invalid amount");

                ResultTransaction = new CoffeeTransaction
                {
                    Coffee = coffee,
                    Payment = new Payment { CashType = cashType, Money = money },
                    DateInfo = new DateInfo(date),
                    HourOfDay = date.Hour,
                    Time = new TimeSpan(0, date.Minute, date.Second)
                };

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
