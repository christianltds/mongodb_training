using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongodb.documents
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString(); 
        [BsonElement("accountid")]
        public string AccountId { get; set; }
        [BsonElement("type")]
        public TransactionType Type { get; set; }
        [BsonElement("amount")]
        public double Amount { get; set; }
        [BsonElement("transactiondate")]
        public DateTime TransactionDate { get; set; }
        [BsonElement("targetaccountid")]
        public string? TargetAccountId { get; set; }
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}