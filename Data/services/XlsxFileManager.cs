using ClosedXML.Excel;
using WorkWithData.Domain.models;

namespace Data.services
{
    public static class XlsxFileManager
    {
        public static void Save(List<CoffeeTransaction> transactions, string path)
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

            workbook.SaveAs("transactions.xlsx");
        }
    }
}
