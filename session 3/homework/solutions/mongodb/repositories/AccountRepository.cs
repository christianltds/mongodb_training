using mongodb.documents;
using MongoDB.Driver;

namespace mongodb.repositories
{
  public class AccountRepository : BaseRepository<Account>, IAccountRepository<Account>
  {
    public AccountRepository(MongoDbContext context) : base(context, "accounts")
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
      return await Collection.Find(GetAccountFilter(id)).FirstOrDefaultAsync();
    }

    public async Task<IList<Account>> GetAccounts(int size, int offset)
    {
      var filter = Builders<Account>.Filter.Empty;
      return await Collection
          .Find(filter)
          .Skip(offset)
          .Limit(size)
          .ToListAsync();
    }

    public async Task<Account> TransferFunds(string fromAccount, string toAccount, double amount)
    {
      const int maxRetries = 3;
      for(int attempt = 1; attempt <= maxRetries; attempt++)
      {
        using var session = await Collection.Database.Client.StartSessionAsync();
        session.StartTransaction();

        try
        {
          var fromAccountDoc = await Collection.Find(session, GetAccountFilter(fromAccount)).FirstOrDefaultAsync();
          var toAccountDoc = await Collection.Find(session, GetAccountFilter(toAccount)).FirstOrDefaultAsync();

          if(fromAccountDoc == null || toAccountDoc == null)
          {
            throw new Exception("Invalid account");
          }

          if(fromAccountDoc.Balance < amount)
          {
            throw new Exception("Insufficient funds");
          }

          var fromFilter = Builders<Account>.Filter.Eq(acc => acc.AccountId, fromAccount);
          var toFilter = Builders<Account>.Filter.Eq(acc => acc.AccountId, toAccount);
          var fromUpdate = Builders<Account>.Update.Inc(acc => acc.Balance, -amount);
          var toUpdate = Builders<Account>.Update.Inc(acc => acc.Balance, amount);

          await Collection.UpdateOneAsync(session, fromFilter, fromUpdate);
          await Collection.UpdateOneAsync(session, toFilter, toUpdate);

          await session.CommitTransactionAsync();
          break;
        }
        catch(Exception)
        {
          await session.AbortTransactionAsync();
          if(attempt == maxRetries)
            throw;
        }
      }

      return await GetAccountById(fromAccount);
    }

    public async Task<Account> UpdateAccountBalance(string accountid, double amount)
    {
      var account = await GetAccountById(accountid);

      if(account == null)
      {
        throw new Exception("Invalid account");
      }
      else if(amount < 0 && account.Balance < Math.Abs(amount))
      {
        throw new Exception("Insufficient funds");
      }

      var filter = Builders<Account>.Filter.Eq(acc => acc.AccountId, accountid);
      var update = Builders<Account>.Update.Inc(acc => acc.Balance, amount);
      var updatedDoc = await Collection.FindOneAndUpdateAsync(
          filter,
          update,
          options: new FindOneAndUpdateOptions<Account> { ReturnDocument = ReturnDocument.After });

      return updatedDoc;
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

    private static FilterDefinition<Account> GetAccountFilter(string accountId)
    {
      return Builders<Account>.Filter.And(
        Builders<Account>.Filter.Eq(acc => acc.AccountId, accountId),
        Builders<Account>.Filter.Eq(acc => acc.IsActive, true));
    }
  }
}