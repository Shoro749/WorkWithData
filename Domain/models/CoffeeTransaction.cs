namespace WorkWithData.Domain.models
{
    public class CoffeeTransaction 
    {
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

        public CoffeeTransaction()
        {
            HourOfDay = 0;
            Payment = new Payment();
            Coffee = "None";
            DateInfo = new DateInfo();
            Time = new TimeSpan();
        }
    }
}
