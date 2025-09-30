namespace WorkWithData.models
{
    public class CoffeeTransaction 
    {
        // hour_of_day,cash_type,money,coffee_name,Time_of_Day,Weekday,Month_name,Weekdaysort,Monthsort,Date,Time
        public int HourOfDay { get; set; }
        public string CashType { get; set; }
        public double Money { get; set; }
        public string CoffeeName { get; set; }
        public string TimeOfDay { get; set; }
        public string Weekday { get; set; }
        public string MonthName { get; set; }
        public int WeekdaySort { get; set; }
        public int MonthSort { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public CoffeeTransaction(int hourOfDay, string cashType, double money, string coffeeName, string timeOfDay,
                             string weekday, string monthName, int weekdaySort, int monthSort, DateTime date, TimeSpan time)
        {
            HourOfDay = hourOfDay;
            CashType = cashType;
            Money = money;
            CoffeeName = coffeeName;
            TimeOfDay = timeOfDay;
            Weekday = weekday;
            MonthName = monthName;
            WeekdaySort = weekdaySort;
            MonthSort = monthSort;
            Date = date;
            Time = time;
        }
    }
}
