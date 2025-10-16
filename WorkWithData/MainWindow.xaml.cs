using Data.reports;
using Data.services;
using DocumentFormat.OpenXml.Math;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using WorkWithData.Data.services;
using WorkWithData.Domain.models;
using WorkWithData.windows;


namespace WorkWithData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<ChartPoint> ChartData { get; set; }
        List<CoffeeTransaction> transactions = new List<CoffeeTransaction>();
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                transactions = CsvFileManager.Load("coffe.csv");
                dg_transactions.ItemsSource = transactions;

                GenerateLineChart(transactions);
                GenerateColumnChart(transactions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GenerateLineChart(List<CoffeeTransaction> coffeeTransactions)
        {
            var groupedByCoffee = coffeeTransactions
                .GroupBy(t => t.Coffee)
                .ToDictionary(g => g.Key, g => g.ToList());

            var groupedByDate = coffeeTransactions
                .GroupBy(t => t.DateInfo.Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            var lineSeriesCollection = new SeriesCollection();
            foreach (var coffee in groupedByCoffee.Keys)
            {
                var values = new ChartValues<double>(
                    groupedByCoffee[coffee].Select(t => t.Payment.Money)
                );

                lineSeriesCollection.Add(new LineSeries
                {
                    Title = coffee,
                    Values = values,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 8
                });
            }

            chart_line.Series = lineSeriesCollection;
            chart_line.AxisX.Clear();
            chart_line.AxisX.Add(new Axis
            {
                Title = "Hour",
                Labels = groupedByCoffee.First().Value.Select(t => t.HourOfDay.ToString()).ToArray()
            });
            chart_line.AxisY.Clear();
            chart_line.AxisY.Add(new Axis { Title = "Price" });
        }

        private void GenerateColumnChart(List<CoffeeTransaction> coffeeTransactions)
        {
            var coffeeTypes = coffeeTransactions.Select(t => t.Coffee).Distinct().ToList();

            var seriesCollection = new SeriesCollection();

            foreach (var coffee in coffeeTypes)
            {
                var count = coffeeTransactions.Count(t => t.Coffee == coffee);

                seriesCollection.Add(new ColumnSeries
                {
                    Title = coffee,
                    Values = new ChartValues<int> { count }
                });
            }

            chart_column.Series = seriesCollection;

            chart_column.AxisX.Clear();
            chart_column.AxisY.Clear();
            chart_column.AxisY.Add(new Axis { Title = "Count" });
        }

        private void UpdateTransactions(List<CoffeeTransaction> list)
        {
            dg_transactions.ItemsSource = list;
            dg_transactions.Items.Refresh();
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double? min = null;
                double? max = null;

                if (!string.IsNullOrWhiteSpace(tb_min.Text))
                {
                    if (double.TryParse(tb_min.Text, out double parsedMin)) min = parsedMin;
                    else throw new Exception("Min price is not a valid number.");
                }

                if (!string.IsNullOrWhiteSpace(tb_max.Text))
                {
                    if (double.TryParse(tb_max.Text, out double parsedMax)) max = parsedMax;
                    else  throw new Exception("Max price is not a valid number.");
                }

                DateTime? startDate = dp_start.SelectedDate;
                DateTime? endDate = dp_end.SelectedDate;

                if (startDate.HasValue && endDate.HasValue)
                {
                    if (startDate > endDate) throw new Exception("The start date cannot be later than the end date.");
                    if (startDate > DateTime.Now || endDate > DateTime.Now) throw new Exception("Dates cannot be later than today.");
                }

                var filtered = transactions.Where(t =>
                    (!min.HasValue || t.Payment.Money >= min.Value) &&
                    (!max.HasValue || t.Payment.Money <= max.Value) &&
                    (!startDate.HasValue || t.DateInfo.Date >= startDate.Value.Date) &&
                    (!endDate.HasValue || t.DateInfo.Date <= endDate.Value.Date)
                ).ToList();

                UpdateTransactions(filtered);
                GenerateLineChart(filtered);
                GenerateColumnChart(filtered);
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

        private void ExportCsv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Title = "Save transactions file",
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    FileName = $"transactions_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (dialog.ShowDialog() == true)
                {
                    CsvFileManager.Save(transactions, dialog.FileName);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ExportJson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Title = "Save transactions file",
                    Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
                    FileName = $"transactions_{DateTime.Now:yyyyMMdd_HHmmss}.json"
                };

                if (dialog.ShowDialog() == true)
                {
                    JsonFileManager.Save(transactions, dialog.FileName);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ExportXml_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Title = "Save transactions file",
                    Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*",
                    FileName = $"transactions_{DateTime.Now:yyyyMMdd_HHmmss}.xml"
                };

                if (dialog.ShowDialog() == true)
                {
                    XmlFileManager.Save(transactions, dialog.FileName);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ExportXlsx_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Title = "Save transactions file",
                    Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                    FileName = $"transactions_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (dialog.ShowDialog() == true)
                {
                    XlsxFileManager.Save(transactions, dialog.FileName);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "All Supported Files|*.json;*.xml;*.csv;*.xlsx|" +
                    "JSON Files (*.json)|*.json|" +
                    "XML Files (*.xml)|*.xml|" +
                    "CSV Files (*.csv)|*.csv|" +
                    "Excel Files (*.xlsx)|*.xlsx";
                ofd.Title = "Open...";
                ofd.DefaultExt = "csv";

                if (ofd.ShowDialog() == true)
                {
                    string path = ofd.FileName;
                    string ext = Path.GetExtension(path).ToLower();
                    switch (ext)
                    {
                        case ".csv":
                            transactions = CsvFileManager.Load(path);
                            break;

                        case ".json":
                            transactions = JsonFileManager.Load(path);
                            break;

                        case ".xml":
                            transactions = XmlFileManager.Load(path);
                            break;

                        case ".xlsx":
                            transactions = XlsxFileManager.Load(path);
                            break;
                    }
                    UpdateTransactions(transactions);
                    MessageBox.Show("File opened successfully!");
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error opening file: {ex.Message}"); }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new CreateTransaction
                {
                    Owner = this
                };

                if (dialog.ShowDialog() == true)
                {
                    var transaction = dialog.ResultTransaction;
                    transactions.Add(transaction);
                    UpdateTransactions(transactions);
                    MessageBox.Show("Transaction successfully created");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selected = dg_transactions.SelectedItem as CoffeeTransaction;
                if (selected == null) throw new Exception("Choose a transaction to edit");

                var dialog = new EditTransaction(selected) { Owner = this };

                if (dialog.ShowDialog() == true)
                {
                    var transaction = dialog.EditedTransaction;
                    transactions.Remove(selected);
                    transactions.Add(transaction);
                    UpdateTransactions(transactions);
                    MessageBox.Show("Transaction successfully edited");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selected = dg_transactions.SelectedItem as CoffeeTransaction;
                if (selected == null) throw new Exception("Choose a transaction to delete");

                transactions.Remove(selected);
                UpdateTransactions(transactions);
                MessageBox.Show("Transaction successfully deleted");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DOCXReport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save docx report",
                Filter = "Docx files (*.docs)|*.docs|All files (*.*)|*.*",
                FileName = $"reprot_{DateTime.Now:yyyyMMdd_HHmmss}.docs"
            };

            if (dialog.ShowDialog() == true)
            {
                DocxReportGenerator.GenerateReport(transactions, dialog.FileName, new List<string>());
                MessageBox.Show("Report saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        private void XLSXReport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save docx report",
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FileName = $"reprot_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                XlsxReportGenerator.GenerateReport(dialog.FileName, transactions);
                MessageBox.Show("Report saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}