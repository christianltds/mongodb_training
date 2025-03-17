namespace mongodb.dtos
{
    public class TransactionSummaryDto
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public double TotalCredit { get; set; }
        public double TotalDebit { get; set; }
        public double TotalTransactions { get; set; }
    }
}