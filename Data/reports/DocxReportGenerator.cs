using WorkWithData.Domain.models;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Data.reports
{
    public static class DocxReportGenerator
    {
        public static void GenerateReport(List<CoffeeTransaction> transactions, string filePath, List<string> chartImagePaths)
        {
            try
            {
                using (DocX document = DocX.Create(filePath))
                {
                    document.InsertParagraph("Coffee Sales Report")
                            .FontSize(22)
                            .Bold()
                            .Alignment = Alignment.center;

                    document.InsertParagraph("Student: Ващенко Роман Андрійович")
                            .FontSize(14)
                            .Alignment = Alignment.center;
                    document.InsertParagraph("Group: Група КН-211")
                            .FontSize(14)
                            .Alignment = Alignment.center;
                    document.InsertParagraph("Variant: 51")
                            .FontSize(14)
                            .Alignment = Alignment.center;
                    document.InsertParagraph($"Generation Date: {DateTime.Now:dd.MM.yyyy}")
                            .FontSize(12)
                            .Alignment = Alignment.center;

                    document.InsertParagraph("").InsertPageBreakAfterSelf();

                    document.InsertParagraph("Dataset Description").FontSize(16).Bold();
                    document.InsertParagraph($"Total Records: {transactions.Count}");
                    document.InsertParagraph("Fields:");
                    document.InsertParagraph("- CoffeeTransaction.HourOfDay: Година транзакції (0-23)");
                    document.InsertParagraph("- CoffeeTransaction.Coffee: Тип кави (Latte, Americano, Hot Chocolate...)");
                    document.InsertParagraph("- CoffeeTransaction.Payment: Інформація про оплату");
                    document.InsertParagraph("    - Payment.CashType: Тип оплати (готівка, картка)");
                    document.InsertParagraph("    - Payment.Money: Ціна кави");
                    document.InsertParagraph("- CoffeeTransaction.DateInfo: Інформація про дату/час");
                    document.InsertParagraph("    - DateInfo.TimeOfDay: Час доби (Morning/Day/Evening)");
                    document.InsertParagraph("    - DateInfo.Weekday: День тижня (Monday, Tuesday...)");
                    document.InsertParagraph("    - DateInfo.MonthName: Назва місяця");
                    document.InsertParagraph("    - DateInfo.WeekdaySort: Порядковий номер дня тижня");
                    document.InsertParagraph("    - DateInfo.MonthSort: Порядковий номер місяця");
                    document.InsertParagraph("    - DateInfo.Date: Конкретна дата транзакції");
                    document.InsertParagraph("- CoffeeTransaction.Time: Час транзакції (TimeSpan)");
                    document.InsertParagraph("");

                    document.InsertParagraph("Key Metrics").FontSize(16).Bold();

                    var coffeeGroups = transactions.GroupBy(t => t.Coffee);

                    foreach (var group in coffeeGroups)
                    {
                        document.InsertParagraph($"{group.Key}:")
                                .FontSize(14)
                                .Bold();
                        document.InsertParagraph($"  Count: {group.Count()}");
                        document.InsertParagraph($"  Min Price: {group.Min(t => t.Payment.Money):F2}");
                        document.InsertParagraph($"  Max Price: {group.Max(t => t.Payment.Money):F2}");
                        document.InsertParagraph($"  Avg Price: {group.Average(t => t.Payment.Money):F2}");
                        document.InsertParagraph("");
                    }

                    if (chartImagePaths != null && chartImagePaths.Any())
                    {
                        document.InsertParagraph("Charts").FontSize(16).Bold();
                        foreach (var imgPath in chartImagePaths)
                        {
                            if (File.Exists(imgPath))
                            {
                                var image = document.AddImage(imgPath);
                                var picture = image.CreatePicture();
                                picture.Width = 500;
                                picture.Height = 300;
                                document.InsertParagraph("").AppendPicture(picture).Alignment = Alignment.center;
                                document.InsertParagraph("");
                            }
                        }
                    }

                    document.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating DOCX report: {ex.Message}");
            }
        }
    }
}
