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
                //transactions = CsvFileManager.Load("coffe.csv");
                transactions = XlsxFileManager.Load("transactions.xlsx");
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
    }
}