namespace WorkWithData.Domain.models
{
    public class CoffeeTransaction 
    {
        // hour_of_day,cash_type,money,coffee_name,Time_of_Day,Weekday,Month_name,Weekdaysort,Monthsort,Date,Time
        public int HourOfDay { get; set; }
        public string Coffee { get; set; }
        public Payment Payment { get; set; }
        public DateInfo DateInfo { get; set; }
        public TimeSpan Time { get; set; }

        public CoffeeTransaction(int hourOfDay, Payment payment, string coffee, DateInfo dateInfo, TimeSpan time)
        {
            HourOfDay = hourOfDay;
            Payment = payment;
            Coffee = coffee;
            DateInfo = dateInfo;
            Time = time;
        }
    }
}
