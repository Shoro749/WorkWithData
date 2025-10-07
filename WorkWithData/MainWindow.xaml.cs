using Data.services;
using System.IO;
using System.Windows;
using WorkWithData.Data.services;
using WorkWithData.Domain.models;

namespace WorkWithData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<CoffeeTransaction> transactions = new List<CoffeeTransaction>();
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                transactions = CsvFileManager.Load("coffe.csv");
                //transactions = XlsxFileManager.Load("transactions.xlsx");
                dg_transactions.ItemsSource = transactions;

                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"transactions_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                //XlsxFileManager.Save(transactions, path);

                //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"transactions_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                //CsvFileManager.Save(transactions, path);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateTransactions(List<CoffeeTransaction> list) => dg_transactions.ItemsSource = list;

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double? min = null;
                double? max = null;

                if (!string.IsNullOrWhiteSpace(tb_min.Text))
                {
                    if (double.TryParse(tb_min.Text, out double parsedMin))
                        min = parsedMin;
                    else
                        throw new Exception("Min price is not a valid number.");
                }

                if (!string.IsNullOrWhiteSpace(tb_max.Text))
                {
                    if (double.TryParse(tb_max.Text, out double parsedMax))
                        max = parsedMax;
                    else
                        throw new Exception("Max price is not a valid number.");
                }

                DateTime? startDate = dp_start.SelectedDate;
                DateTime? endDate = dp_end.SelectedDate;

                if (startDate.HasValue && endDate.HasValue)
                {
                    if (startDate > endDate)
                        throw new Exception("The start date cannot be later than the end date.");
                    if (startDate > DateTime.Now || endDate > DateTime.Now)
                        throw new Exception("Dates cannot be later than today.");
                }

                var filtered = transactions.Where(t =>
                    (!min.HasValue || t.Payment.Money >= min.Value) &&
                    (!max.HasValue || t.Payment.Money <= max.Value) &&
                    (!startDate.HasValue || t.DateInfo.Date >= startDate.Value.Date) &&
                    (!endDate.HasValue || t.DateInfo.Date <= endDate.Value.Date)
                ).ToList();

                UpdateTransactions(filtered);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string text = tb_search.Text;

            if (string.IsNullOrEmpty(text))
            {
                UpdateTransactions(transactions);
                return;
            }

            var search = transactions.Where(t => t.Coffee == text).ToList();
            UpdateTransactions(search);
        }
    }
}