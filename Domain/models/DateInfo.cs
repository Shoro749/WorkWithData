namespace WorkWithData.Domain.models
{
    public class DateInfo
    {
        public string TimeOfDay { get; set; }
        public string Weekday { get; set; }
        public string MonthName { get; set; }
        public int WeekdaySort { get; set; }
        public int MonthSort { get; set; }
        public DateTime Date { get; set; }

        public DateInfo(string timeOfDay, string weekday, string monthName, int weekdaySort, int monthSort, DateTime date)
        {
            TimeOfDay = timeOfDay;
            Weekday = weekday;
            MonthName = monthName;
            WeekdaySort = weekdaySort;
            MonthSort = monthSort;
            Date = date;
        }

        public DateInfo()
        {
            TimeOfDay = "";
            Weekday = "";
            MonthName = "";
            WeekdaySort = 0;
            MonthSort = 0;
            Date = new DateTime();
        }

        public override string ToString() => $"{TimeOfDay}, {Weekday}, {MonthName}, {/*WeekdaySort}, {MonthSort*/null}{Date.ToString("dd.MM.yyyy")}";
    }
}
