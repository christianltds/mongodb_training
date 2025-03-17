using mongodb.documents;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace mongodb.repositories
{
  public class MongoDbContext
  {
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName, bool logToConsole = false)
    {
      var settings = MongoClientSettings.FromConnectionString(connectionString);
      if(logToConsole)
      {
        settings.ClusterConfigurator = builder =>
          builder
          .Subscribe<CommandStartedEvent>(e => Console.WriteLine($"MongoDB Command: {e.CommandName} - {e.Command.ToJson()}"));
      }

      var client = new MongoClient(settings);
      _database = client.GetDatabase(databaseName);
      CreateIndexes().GetAwaiter().GetResult();
    }

    public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
        where TDocument : class
    {
      return _database.GetCollection<TDocument>(collectionName);
    }

    public async Task CreateIndexes()
    {

      var indexKeys = Builders<Account>.IndexKeys.Ascending(acc => acc.AccountId);
      var indexOptions = new CreateIndexOptions { Unique = true };
      var indexModel = new CreateIndexModel<Account>(indexKeys, indexOptions);

      await _database.GetCollection<Account>("accounts").Indexes.CreateOneAsync(indexModel);

      var indexModels = new List<CreateIndexModel<Transaction>>
      {
          new(Builders<Transaction>.IndexKeys.Ascending(t => t.AccountId).Ascending(t => t.TargetAccountId)),
          new(Builders<Transaction>.IndexKeys.Ascending(t => t.TransactionDate))
      };

      await _database.GetCollection<Transaction>("transactions").Indexes.CreateManyAsync(indexModels);
    }
  }
}