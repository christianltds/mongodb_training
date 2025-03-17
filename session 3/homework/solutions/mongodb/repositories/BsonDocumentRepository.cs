
using MongoDB.Driver;
using MongoDB.Bson;

namespace mongodb.repositories
{
    public class BsonDocumentRepository : BaseRepository<BsonDocument>, IAccountRepository<BsonDocument>
    {    
        public BsonDocumentRepository(MongoDbContext context): base(context, "accounts")
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
            var filter = Builders<BsonDocument>.Filter.Eq("accountid", id);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IList<BsonDocument>> GetAccounts()
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<BsonDocument> TransferFunds(string fromAccount, string toAccount, double amount)
        {
            var fromFilter = Builders<BsonDocument>.Filter.Eq("accountid", fromAccount);
            var toFilter = Builders<BsonDocument>.Filter.Eq("accountid", toAccount);
            var fromUpdate = Builders<BsonDocument>.Update.Inc("balance", -amount);
            var toUpdate = Builders<BsonDocument>.Update.Inc("balance", amount);

            var session = await Collection.Database.Client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                await Collection.FindOneAndUpdateAsync(session, fromFilter, fromUpdate);
                await Collection.FindOneAndUpdateAsync(session, toFilter, toUpdate);
                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
            }
            finally
            {
                session.Dispose();
            }

            return await GetAccountById(fromAccount);
        }

        public async Task<BsonDocument> UpdateAccountBalance(string accountid, double amount)
        {
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
    }
}