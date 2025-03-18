using mongodb.documents;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace mongodb.repositories
{
  public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
  {
    private readonly IMongoCollection<Account> _accountCollection;

    public TransactionRepository(MongoDbContext context) : base(context, "transactions")
    {
      _accountCollection = context.GetCollection<Account>("accounts");
    }

    public async Task<Transaction> CreateTransaction(
        string accountId,
        double amount,
        string? targetAccountId = null)
    {
      var type = targetAccountId != null
          ? TransactionType.Transfer
          : amount < 0
              ? TransactionType.Withdrawal
              : TransactionType.Deposit;

      var transaction = new Transaction
      {
        AccountId = accountId,
        Amount = Math.Abs(amount),
        Type = type,
        TargetAccountId = targetAccountId,
        TransactionDate = GenerateRandomDate()
      };

      await Collection.InsertOneAsync(transaction);
      return transaction;
    }

    public async Task<IList<DetailedTransaction>> GetTransactionsByAccountIdAsync(string accountId)
    {
      var transactionQueryable = Collection.AsQueryable();
      var query = transactionQueryable
          .Where(transaction => transaction.AccountId == accountId || transaction.TargetAccountId == accountId)
          .GroupJoin(_accountCollection,
              transaction => transaction.TargetAccountId,
              account => account.AccountId,
              (transaction, accounts) => new
              {
                Transaction = new DetailedTransaction()
                {
                  Id = transaction.Id,
                  TargetAccount = accounts.FirstOrDefault(),
                  Amount = transaction.Amount,
                  Type = transaction.Type,
                  TransactionDate = transaction.TransactionDate,
                },
                transaction.AccountId
              })
          .GroupJoin(_accountCollection,
              transaction => transaction.AccountId,
              account => account.AccountId,
              (transaction, accounts) => new DetailedTransaction()
              {
                Id = transaction.Transaction.Id,
                TargetAccount = transaction.Transaction.TargetAccount,
                Amount = transaction.Transaction.Amount,
                Type = transaction.Transaction.Type,
                TransactionDate = transaction.Transaction.TransactionDate,
                Account = accounts.First()
              });
      return await query.ToListAsync();
    }

    public async Task<IList<TransactionSummary>> GetTransactionSummary(string accountid, TransactionSummaryUnit unit)
    {
      var unitValue = unit.ToString().ToLower();
      var result = Collection.AsQueryable()
          .Where(t => t.AccountId == accountid || t.TargetAccountId == accountid)
          .AppendStage<Transaction, Transaction>(
              new BsonDocument("$addFields",
                  new BsonDocument("transactiondate",
                      new BsonDocument("$dateTrunc",
                          new BsonDocument
                          {
                                    { "date", "$transactiondate" },
                                    { "unit", unitValue }
                          }
                      )
                  )
              )
          )
          .GroupBy(t => t.TransactionDate)
          .Select(g => new TransactionSummary()
          {
            Id = g.Key.Date,
            AccountId = g.First().AccountId,
            TotalCredit = g.Where(t =>
                      t.Type == TransactionType.Deposit ||
                      (t.Type == TransactionType.Transfer && t.TargetAccountId == accountid))
                  .Sum(t => t.Amount),
            TotalDebit = g.Where(t =>
                      t.Type == TransactionType.Withdrawal ||
                      (t.Type == TransactionType.Transfer && t.AccountId == accountid))
                  .Sum(t => Math.Abs(t.Amount)),
            TotalTransactions = g.Count()
          })
          .OrderByDescending(t => t.Id);

      return await result.ToListAsync();
    }

    private static DateTime GenerateRandomDate()
    {
      var random = new Random();
      var today = DateTime.Today;

      int randomYear = random.Next(today.Year - 5, today.Year + 1);
      int randomMonth = random.Next(1, 13);
      int day = Math.Min(today.Day, DateTime.DaysInMonth(randomYear, randomMonth));

      var randomDate = new DateTime(randomYear, randomMonth, day);

      return randomDate > today ? today : randomDate;
    }
  }
}