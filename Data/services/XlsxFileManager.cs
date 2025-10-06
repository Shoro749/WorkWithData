using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualBasic;
using System.Globalization;
using WorkWithData.Domain.models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data.services
{
    public static class XlsxFileManager
    {
        public static void Save(List<CoffeeTransaction> transactions, string path)
        {
            try
            {
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Transactions");

                worksheet.Cell(1, 1).Value = "hour_of_day";
                worksheet.Cell(1, 2).Value = "cash_type";
                worksheet.Cell(1, 3).Value = "money";
                worksheet.Cell(1, 4).Value = "coffee_name";
                worksheet.Cell(1, 5).Value = "Time_of_Day";
                worksheet.Cell(1, 6).Value = "Weekday";
                worksheet.Cell(1, 7).Value = "Month_name";
                worksheet.Cell(1, 8).Value = "Weekdaysort";
                worksheet.Cell(1, 9).Value = "Monthsort";
                worksheet.Cell(1, 10).Value = "Date";
                worksheet.Cell(1, 11).Value = "Time";

                int row = 2;
                foreach (var t in transactions)
                {
                    worksheet.Cell(row, 1).Value = t.HourOfDay;
                    worksheet.Cell(row, 2).Value = t.Payment.CashType;
                    worksheet.Cell(row, 3).Value = t.Payment.Money;
                    worksheet.Cell(row, 4).Value = t.Coffee;
                    worksheet.Cell(row, 5).Value = t.DateInfo.TimeOfDay;
                    worksheet.Cell(row, 6).Value = t.DateInfo.Weekday;
                    worksheet.Cell(row, 7).Value = t.DateInfo.MonthName;
                    worksheet.Cell(row, 8).Value = t.DateInfo.WeekdaySort;
                    worksheet.Cell(row, 9).Value = t.DateInfo.MonthSort;
                    worksheet.Cell(row, 10).Value = t.DateInfo.Date;
                    worksheet.Cell(row, 11).Value = t.Time;
                    row++;
                }

                workbook.SaveAs("$transactions_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public static List<CoffeeTransaction> Load(string path)
        {
            var workbook = new XLWorkbook(path);
            var worksheet = workbook.Worksheet("Transactions");
            List<CoffeeTransaction> transactions = new List<CoffeeTransaction>();

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                int hourOfDay = row.Cell(1).GetValue<int>();
                string cashType = row.Cell(2).GetString();
                double money = row.Cell(3).GetValue<double>();
                string coffee = row.Cell(4).GetString();
                string timeOfDay = row.Cell(5).GetString();
                string weekday = row.Cell(6).GetString();
                string monthName = row.Cell(7).GetString();
                int weekdaySort = row.Cell(8).GetValue<int>();
                int monthSort = row.Cell(9).GetValue<int>();
                DateTime date = row.Cell(10).GetValue<DateTime>();
                TimeSpan time = row.Cell(11).GetValue<TimeSpan>();

                var payment = new Payment(cashType, money);
                var dateInfo = new DateInfo(timeOfDay, weekday, monthName, weekdaySort, monthSort, date);
                var transaction = new CoffeeTransaction(hourOfDay, payment, coffee, dateInfo, time);

                transactions.Add(transaction);
            }

            return transactions;
        }
    }
}
