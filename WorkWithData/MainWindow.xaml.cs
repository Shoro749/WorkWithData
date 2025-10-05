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
            transactions = CsvFileManager.Load("coffe.csv");
            dg_transactions.ItemsSource = transactions;

            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"transactions_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            //CsvFileManager.Save(transactions, path);
        }
    }
}