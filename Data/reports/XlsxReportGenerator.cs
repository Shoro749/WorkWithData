using ClosedXML.Excel;
using WorkWithData.Domain.models;
using System.Linq;

namespace Data.reports
{
    public static class XlsxReportGenerator
    {
        public static void GenerateReport(string path, List<CoffeeTransaction> transactions)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Summary");
            
            int row = 1;

            ws.Cell(row, 1).Value = "📊 Coffee Sales Summary";
            ws.Range(row, 1, row, 3).Merge().Style
                .Font.SetBold().Font.FontSize = 14;
            row += 2;

            ws.Cell(row, 1).Value = "Sales by Month";
            ws.Cell(row, 1).Style.Font.SetBold();
            row++;

            var salesByMonth = transactions
                .GroupBy(t => t.DateInfo.MonthName)
                .Select(g => new { Month = g.Key, Total = g.Sum(t => t.Payment.Money) })
                .OrderBy(g => g.Month)
                .ToList();

            ws.Cell(row, 1).InsertTable(salesByMonth, "tblSalesByMonth", true);
            row += salesByMonth.Count + 3;

            ws.Cell(row, 1).Value = "Sales by Coffee Type";
            ws.Cell(row, 1).Style.Font.SetBold();
            row++;

            var salesByCoffee = transactions
                .GroupBy(t => t.Coffee)
                .Select(g => new { Coffee = g.Key, Total = g.Sum(t => t.Payment.Money) })
                .OrderByDescending(g => g.Total)
                .ToList();

            ws.Cell(row, 1).InsertTable(salesByCoffee, "tblSalesByCoffee", true);
            int mainTableStart = row + 1;
            int mainTableEnd = mainTableStart + salesByCoffee.Count - 1;
            row += salesByCoffee.Count + 2;

            ws.Cell(row, 1).Value = "🏆 Top 3 Coffees";
            ws.Cell(row, 1).Style.Font.SetBold();
            row++;

            var top3 = salesByCoffee.OrderByDescending(x => x.Total).Take(3).ToList();
            ws.Cell(row, 1).InsertTable(top3, "tblTop3", true);
            ws.Range(row + 1, 1, row + top3.Count, 2).Style.Fill.SetBackgroundColor(XLColor.LightGreen);
            row += top3.Count + 2;

            ws.Cell(row, 1).Value = "📉📈 Min/Max Sales";
            ws.Cell(row, 1).Style.Font.SetBold();
            row++;

            var minSale = salesByCoffee.MinBy(x => x.Total);
            var maxSale = salesByCoffee.MaxBy(x => x.Total);

            var minMaxList = new[]
            {
            new { Type = "Min", Coffee = minSale.Coffee, Total = minSale.Total },
            new { Type = "Max", Coffee = maxSale.Coffee, Total = maxSale.Total }
        };

            ws.Cell(row, 1).InsertTable(minMaxList, "tblMinMax", true);

            ws.Range(row + 1, 1, row + 1, 3).Style.Fill.SetBackgroundColor(XLColor.LightSalmon); // Min
            ws.Range(row + 2, 1, row + 2, 3).Style.Fill.SetBackgroundColor(XLColor.LightBlue);   // Max

            ws.Columns().AdjustToContents();

            wb.SaveAs(path);
        }
    }
}
