namespace mongodb.documents
{
    public class TransactionSummary
    {
        public DateTime Id { get; set; }
        public string AccountId { get; set; }
        public double TotalCredit { get; set; }
        public double TotalDebit { get; set; }
        public double TotalTransactions { get; set; }
    }

    public enum TransactionSummaryUnit
    {
        Day,
        Month,
        Year
    }
}