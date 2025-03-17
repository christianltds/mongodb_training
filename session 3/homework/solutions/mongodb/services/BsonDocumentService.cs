using mongodb.documents;
using mongodb.dtos;
using mongodb.repositories;
using mongodb.utils;
using MongoDB.Bson;

namespace mongodb.services{
    public class BsonDocumentService : IAccountService
    {
        private readonly IAccountRepository<BsonDocument> _repository;
        private readonly ITransactionRepository _transactionRepository;
        
        public BsonDocumentService(
            IAccountRepository<BsonDocument> repository,
            ITransactionRepository transactionRepository)
        {
            _repository = repository;
            _transactionRepository = transactionRepository;
        }

        public async Task<AccountDto> CreateAccount(CreateAccountDto account)
        {
            var document = new BsonDocument(){
                {"_id", ObjectId.GenerateNewId()},
                {"firstname", account.FirstName},
                {"lastname", account.LastName},
                {"balance", account.InitialBalance},
                {"createdate", DateTime.UtcNow},
                {"active", true},
            };
            var createdDocument = await _repository.CreateAccount(document);
            return DtoMapper.MapDocumentToAccountDto(createdDocument);
        }

        public async Task<bool> DeleteAccount(string accountid)
        {
            return await _repository.DeleteAccount(accountid);
        }

        public async Task<AccountDto> GetAccountById(string id)
        {
            var document = await _repository.GetAccountById(id);
            return DtoMapper.MapDocumentToAccountDto(document);
        }

        public async Task<IList<AccountDto>> GetAccounts()
        {
            var documents = await _repository.GetAccounts();
            return [.. documents.Select(DtoMapper.MapDocumentToAccountDto)];
        }

        public async Task<IList<TransactionSummaryDto>> GetAccountSummary(string accountid, TransactionSummaryUnit unit)
        {
            var summaries = await _transactionRepository.GetTransactionSummary(accountid, unit);
            return [.. summaries.Select(DtoMapper.MapToTransactionSummaryDto)];
        }

        public async Task<IList<TransactionDto>> GetAccountTransactions(string accountid)
        {
            var transactions = await _transactionRepository.GetTransactionsByAccountIdAsync(accountid);
            return [.. transactions.Select(DtoMapper.MapToTransactionDto)];
        }

        public async Task<AccountDto> TransferFunds(string fromAccount, string toAccount, double amount)
        {
            var account = await _repository.TransferFunds(fromAccount, toAccount, amount);
            await _transactionRepository.CreateTransaction(fromAccount, amount, toAccount);
            return DtoMapper.MapDocumentToAccountDto(account);
        }

        public async Task<AccountDto> UpdateAccountBalance(string accountid, double amount)
        {
            var account = await _repository.UpdateAccountBalance(accountid, amount);
            await _transactionRepository.CreateTransaction(accountid, amount);
            return DtoMapper.MapDocumentToAccountDto(account);
        }
    }
}