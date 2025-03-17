namespace mongodb.documents
{
    public class DetailedTransaction
    {
        public string Id { get; set; }
        public Account Account { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        // For transfers
        public Account? TargetAccount { get; set; }
    }
}