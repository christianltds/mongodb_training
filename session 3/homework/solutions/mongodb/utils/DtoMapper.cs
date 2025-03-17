using mongodb.documents;
using mongodb.dtos;
using MongoDB.Bson;

namespace mongodb.utils
{
    public class DtoMapper
    {
        public static AccountDto MapToAccountDto(Account newAccount)
        {
            return new AccountDto()
            {
                AccountId = newAccount.AccountId,
                FirstName = newAccount.FirstName,
                LastName = newAccount.LastName,
                Balance = newAccount.Balance,
            };
        }

        public static BasicAccountDto MapToBasicAccountDto(Account newAccount)
        {
            return new BasicAccountDto()
            {
                AccountId = newAccount.AccountId,
                FirstName = newAccount.FirstName,
                LastName = newAccount.LastName
            };
        }

        public static TransactionDto MapToTransactionDto(DetailedTransaction transaction)
        {
            return new TransactionDto()
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Type = transaction.Type,
                Account = MapToBasicAccountDto(transaction.Account),
                TargetAccount = transaction.TargetAccount != null ? MapToBasicAccountDto(transaction.TargetAccount) : null
            };
        }

        public static AccountDto MapDocumentToAccountDto(BsonDocument document)
        {
            return new AccountDto(){
                AccountId = document["accountid"].AsString,
                FirstName = document["firstname"].AsString,
                LastName = document["lastname"].AsString,
                Balance = document["balance"].AsDouble
            };
        }

        public static TransactionSummaryDto MapToTransactionSummaryDto(TransactionSummary summary)
        {
            return new TransactionSummaryDto()
            { 
                Id = summary.Id.ToString("yyyy-MM-dd"),
                AccountId = summary.AccountId,
                TotalCredit = summary.TotalCredit,
                TotalDebit = summary.TotalDebit,
                TotalTransactions = summary.TotalTransactions
            };
        }
    }
}