using MongoDB.Bson;
using MongoDB.Driver;

namespace mongodb.repositories
{
  public class BsonDocumentRepository : BaseRepository<BsonDocument>, IAccountRepository<BsonDocument>
  {
    public BsonDocumentRepository(MongoDbContext context) : base(context, "accounts")
    {
    }

    public async Task<BsonDocument> CreateAccount(BsonDocument document)
    {
      var accountid = await GetNextAccountId();
      document.Add("accountid", accountid.ToString());

      await Collection.InsertOneAsync(document);
      return document;
    }

    public async Task<bool> DeleteAccount(string accountid)
    {
      var filter = Builders<BsonDocument>.Filter.Eq("accountid", accountid);
      var update = Builders<BsonDocument>.Update.Set("active", false);
      var result = await Collection.UpdateOneAsync(filter, update);
      return result.IsAcknowledged;
    }

    public async Task<BsonDocument> GetAccountById(string id)
    {
      return await Collection.Find(GetAccountFilter(id)).FirstOrDefaultAsync();
    }

    public async Task<IList<BsonDocument>> GetAccounts(int size, int offset)
    {
      var filter = Builders<BsonDocument>.Filter.Empty;
      return await Collection
        .Find(filter)
        .Skip(offset)
        .Limit(size)
        .ToListAsync();
    }

    public async Task<BsonDocument> TransferFunds(string fromAccount, string toAccount, double amount)
    {
      const int maxRetries = 3;
      for(int attempt = 1; attempt <= maxRetries; attempt++)
      {
        using var session = await Collection.Database.Client.StartSessionAsync();
        try
        {
          session.StartTransaction();

          var fromFilter = Builders<BsonDocument>.Filter.Eq("accountid", fromAccount);
          var toFilter = Builders<BsonDocument>.Filter.Eq("accountid", toAccount);
          var fromUpdate = Builders<BsonDocument>.Update.Inc("balance", -amount);
          var toUpdate = Builders<BsonDocument>.Update.Inc("balance", amount);

          var fromAccountDoc = await Collection.Find(session, fromFilter).FirstOrDefaultAsync();
          var toAccountDoc = await Collection.Find(session, toFilter).FirstOrDefaultAsync();

          if(fromAccountDoc == null || toAccountDoc == null)
            throw new Exception("Invalid account");

          if(fromAccountDoc["balance"].ToDouble() < amount)
            throw new Exception("Insufficient funds");

          await Collection.FindOneAndUpdateAsync(session, fromFilter, fromUpdate);
          await Collection.FindOneAndUpdateAsync(session, toFilter, toUpdate);

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

    public async Task<BsonDocument> UpdateAccountBalance(string accountid, double amount)
    {
      var account = await GetAccountById(accountid);

      if(account == null)
      {
        throw new Exception("Invalid account");
      }
      else if(amount < 0 && account["balance"].AsDouble < Math.Abs(amount))
      {
        throw new Exception("Insufficient funds");
      }

      var filter = Builders<BsonDocument>.Filter.Eq("accountid", accountid);
      var update = Builders<BsonDocument>.Update.Inc("balance", amount);
      var result = await Collection.FindOneAndUpdateAsync(
          filter,
          update,
          options: new FindOneAndUpdateOptions<BsonDocument> { ReturnDocument = ReturnDocument.After });

      return result;
    }

    private async Task<int> GetNextAccountId()
    {
      var sort = Builders<BsonDocument>.Sort.Descending("accountid");
      var projection = Builders<BsonDocument>.Projection.Include("accountid").Exclude("_id");
      var document = await Collection.Find(Builders<BsonDocument>.Filter.Empty)
          .Sort(sort)
          .Project(projection)
          .FirstOrDefaultAsync();

      return int.Parse(document == null ? "0" : document.GetValue("accountid").AsString) + 1;
    }

    private static FilterDefinition<BsonDocument> GetAccountFilter(string accountId)
    {
      return Builders<BsonDocument>.Filter.And(
        Builders<BsonDocument>.Filter.Eq("accountid", accountId),
        Builders<BsonDocument>.Filter.Eq("active", true));
    }
  }
}