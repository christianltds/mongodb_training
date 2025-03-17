using mongodb.documents;

namespace mongodb.dtos
{
    public class TransactionDto
    {
        public string Id { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public BasicAccountDto Account { get; set; }
        public BasicAccountDto? TargetAccount { get; set; }
    }
}