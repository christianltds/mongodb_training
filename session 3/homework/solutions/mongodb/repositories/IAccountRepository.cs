namespace mongodb.repositories
{
    public interface IAccountRepository<TDocument> where TDocument : class
    {
        public Task<IList<TDocument>> GetAccounts();

        public Task<TDocument> GetAccountById(string id);

        public Task<TDocument> CreateAccount(TDocument document);

        public Task<TDocument> UpdateAccountBalance(string accountid, double amount);

        public Task<bool> DeleteAccount(string accountid);

        public Task<TDocument> TransferFunds(string fromAccount, string toAccount, double amount);
    }
}