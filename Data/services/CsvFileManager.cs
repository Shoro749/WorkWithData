using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using WorkWithData.Domain.models;

namespace WorkWithData.Data.services
{
    public static class CsvFileManager
    {
        public static void Save(List<CoffeeTransaction> transactions, string path)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ",",
                };

                using (var writer = new StreamWriter(path, false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteField("hour_of_day");
                    csv.WriteField("cash_type");
                    csv.WriteField("money");
                    csv.WriteField("coffee_name");
                    csv.WriteField("Time_of_Day");
                    csv.WriteField("Weekday");
                    csv.WriteField("Month_name");
                    csv.WriteField("Weekdaysort");
                    csv.WriteField("Monthsort");
                    csv.WriteField("Date");
                    csv.WriteField("Time");
                    csv.NextRecord();

                    foreach (var transaction in transactions)
                    {
                        csv.WriteField(transaction.HourOfDay);
                        csv.WriteField(transaction.Payment.CashType);
                        csv.WriteField(transaction.Payment.Money);
                        csv.WriteField(transaction.Coffee);
                        csv.WriteField(transaction.DateInfo.TimeOfDay);
                        csv.WriteField(transaction.DateInfo.Weekday);
                        csv.WriteField(transaction.DateInfo.MonthName);
                        csv.WriteField(transaction.DateInfo.WeekdaySort);
                        csv.WriteField(transaction.DateInfo.MonthSort);
                        csv.WriteField(transaction.DateInfo.Date.ToString("dd/MM/yyyy"));
                        csv.WriteField("`" + transaction.Time.ToString(@"h\:mm\.f"));

                        csv.NextRecord();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

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
                        string coffeename = csv.GetField<string>("coffee_name");
                        int hourofday = Convert.ToInt32(csv.GetField<string>("hour_of_day"));

                        string cashtype = csv.GetField<string>("cash_type");
                        double money = double.Parse(csv.GetField<string>("money"), NumberStyles.Any, CultureInfo.InvariantCulture);
                        
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

                        var payment = new Payment(cashtype, money);
                        var dateInfo = new DateInfo(timeofday, weekday, monthday, weekdaysort, monthsort, date);
                        var transaction = new CoffeeTransaction(hourofday, payment, coffeename, dateInfo, time);
                        transactions.Add(transaction);
                    }
                }
                return transactions;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
