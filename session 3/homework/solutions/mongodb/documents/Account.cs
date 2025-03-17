using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongodb.documents
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString(); 
        [BsonElement("accountid")]
        public string AccountId { get; set; }
        [BsonElement("firstname")]
        public string FirstName { get; set; }
        [BsonElement("lastname")]
        public string LastName { get; set; }
        [BsonElement("balance")]
        public double Balance { get; set; }
        [BsonElement("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonElement("active")]
        public bool IsActive { get; set; } = true;
    }
}