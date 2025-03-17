using mongodb.documents;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongodb.repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository<Account>
    {
        public AccountRepository(MongoDbContext context): base(context, "accounts")
        {
        }

        public async Task<Account> CreateAccount(Account document)
        {
            var accountid = await GetNextAccountId();
            document.AccountId = accountid.ToString();
            await Collection.InsertOneAsync(document);
            return document;
        }

        public async Task<bool> DeleteAccount(string accountid)
        {
            var filter = Builders<Account>.Filter.Eq(acc => acc.AccountId, accountid);
            var update = Builders<Account>.Update.Set(acc => acc.IsActive, false);
            var result = await Collection.UpdateOneAsync(filter, update);
            return result.IsAcknowledged;
        }

        public async Task<Account> GetAccountById(string id)
        {
            var filter = Builders<Account>.Filter.Eq(acc => acc.AccountId, id);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IList<Account>> GetAccounts()
        {
            var filter = Builders<Account>.Filter.Empty;
            return await Collection
                .Find(filter)
                .ToListAsync();
        }

        public async Task<Account> TransferFunds(string fromAccount, string toAccount, double amount)
        {
            var fromFilter = Builders<Account>.Filter.Eq(acc => acc.AccountId, fromAccount);
            var toFilter = Builders<Account>.Filter.Eq(acc => acc.AccountId, toAccount);
            var fromAccountDoc = await Collection.Find(fromFilter).FirstOrDefaultAsync();
            var toAccountDoc = await Collection.Find(toFilter).FirstOrDefaultAsync();

            if (fromAccountDoc == null || toAccountDoc == null)
            {
                throw new Exception("Invalid account");
            }

            if (fromAccountDoc.Balance < amount)
            {
                throw new Exception("Insufficient funds");
            }

            var fromUpdate = Builders<Account>.Update.Inc(acc => acc.Balance, -amount);
            var toUpdate = Builders<Account>.Update.Inc(acc => acc.Balance, amount);

            await Collection.UpdateOneAsync(fromFilter, fromUpdate);
            await Collection.UpdateOneAsync(toFilter, toUpdate);

            return await GetAccountById(fromAccountDoc.AccountId);
        }

        public async Task<Account> UpdateAccountBalance(string accountid, double amount)
        {
            var filter = Builders<Account>.Filter.Eq(acc => acc.AccountId, accountid);
            var update = Builders<Account>.Update.Inc(acc => acc.Balance, amount);
            await Collection.FindOneAndUpdateAsync(
                filter,
                update,
                options: new FindOneAndUpdateOptions<Account> { ReturnDocument = ReturnDocument.After });
            return await GetAccountById(accountid);
        }

        private async Task<int> GetNextAccountId()
        {
            var sort = Builders<Account>.Sort.Descending(acc => acc.AccountId);
            var projection = Builders<Account>.Projection.Include(acc => acc.AccountId).Exclude(acc => acc.Id);
            var account = await Collection.Find(Builders<Account>.Filter.Empty)
                .Sort(sort)
                .Project<Account>(projection)
                .FirstOrDefaultAsync();

            return int.Parse(account == null ? "0" : account.AccountId) + 1;
        }
    }
}