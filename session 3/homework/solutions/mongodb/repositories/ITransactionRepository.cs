using mongodb.documents;

namespace mongodb.repositories
{
    public interface ITransactionRepository
    {
        public Task<IList<DetailedTransaction>> GetTransactionsByAccountIdAsync(string accountId);
        public Task<Transaction> CreateTransaction(
            string accountId,
            double amount,
            string? targetAccountId = null);
        public Task<IList<TransactionSummary>> GetTransactionSummary(string accountid, TransactionSummaryUnit unit);
    }
}