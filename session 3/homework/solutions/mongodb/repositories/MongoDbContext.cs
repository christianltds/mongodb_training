using mongodb.documents;
using MongoDB.Driver;

namespace mongodb.repositories
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
            where TDocument : class
        {
            return _database.GetCollection<TDocument>(collectionName);
        }
    }
}