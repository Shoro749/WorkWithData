using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using WorkWithData.models;

namespace WorkWithData.services
{
    public static class CsvFileManager
    {
        public static List<CoffeeTransaction> Load(string path)
        {
            List<CoffeeTransaction> transactions = new List<CoffeeTransaction>();
            try
            {
                if (!File.Exists(path)) return transactions;

                var config = new CsvConfiguration(CultureInfo.InvariantCulture);

                using (var reader = new StreamReader(path, Encoding.UTF8))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        int hourofday = Convert.ToInt32(csv.GetField<string>("hour_of_day"));
                        string cashtype = csv.GetField<string>("cash_type");
                        double money = double.Parse(csv.GetField<string>("money"), NumberStyles.Any, CultureInfo.InvariantCulture);
                        string coffeename = csv.GetField<string>("coffee_name");
                        string timeofday = csv.GetField<string>("Time_of_Day");
                        string weekday = csv.GetField<string>("Weekday");
                        string monthday = csv.GetField<string>("Month_name");
                        int weekdaysort = Convert.ToInt32(csv.GetField<string>("Weekdaysort"));
                        int monthsort = Convert.ToInt32(csv.GetField<string>("Monthsort"));
                        DateTime date = DateTime.ParseExact(csv.GetField<string>("Date"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        string[] parts = csv.GetField<string>("Time").Split(':');
                        int minutes = int.Parse(parts[0], CultureInfo.InvariantCulture);
                        double seconds = double.Parse(parts[1], CultureInfo.InvariantCulture);
                        TimeSpan time = TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);

                        var transaction = new CoffeeTransaction(hourofday, cashtype, money, coffeename, timeofday,
                            weekday, monthday, weekdaysort, monthsort, date, time);
                        transactions.Add(transaction);
                    }
                }
                return transactions;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                throw;
            }
        }
    }
}
