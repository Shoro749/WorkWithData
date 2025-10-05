namespace WorkWithData.models
{
    public class Payment
    {
        public string CashType { get; set; }
        public double Money { get; set; }

        public Payment(string CashType, double Money)
        {
            this.CashType = CashType;
            this.Money = Money;
        }

        public override string ToString() => $"{CashType}, {Money}";
    }
}
