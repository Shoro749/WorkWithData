using System.Windows;
using System.Windows.Controls;
using WorkWithData.Domain.models;

namespace WorkWithData.windows
{
    /// <summary>
    /// Interaction logic for EditTransaction.xaml
    /// </summary>
    public partial class EditTransaction : Window
    {
        public CoffeeTransaction EditedTransaction { get; private set; }
        public EditTransaction(CoffeeTransaction transaction)
        {
            InitializeComponent();
            tb_coffee.Text = transaction.Coffee;
            cb_cashtype.SelectedIndex = 1;
            tb_money.Text = transaction.Payment.Money.ToString();
            dp_date.SelectedDate = transaction.DateInfo.Date;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string coffee = string.IsNullOrWhiteSpace(tb_coffee.Text) ? throw new Exception("Coffee name cannot be empty") : tb_coffee.Text;
                string cashType = (cb_cashtype.SelectedItem as ComboBoxItem)?.Content.ToString();
                var date = dp_date.SelectedDate ?? DateTime.Now;
                if (!double.TryParse(tb_money.Text, out double money))
                    throw new Exception("Invalid amount");

                EditedTransaction = new CoffeeTransaction
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
