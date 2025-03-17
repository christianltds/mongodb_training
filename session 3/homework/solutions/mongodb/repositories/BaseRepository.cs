using MongoDB.Driver;

namespace mongodb.repositories
{
  public abstract class BaseRepository<TDocument> where TDocument : class
  {
    protected IMongoCollection<TDocument> Collection { get; }

    protected BaseRepository(MongoDbContext context, string collectionName)
    {
      Collection = context.GetCollection<TDocument>(collectionName);
    }
  }
}