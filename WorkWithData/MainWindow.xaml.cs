using System.Transactions;
using System.Windows;
using WorkWithData.models;
using WorkWithData.services;

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
        }
    }
}