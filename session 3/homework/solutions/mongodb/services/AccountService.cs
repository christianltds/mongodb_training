using mongodb.documents;
using mongodb.dtos;
using mongodb.repositories;
using mongodb.utils;

namespace mongodb.services
{
  public class AccountService : IAccountService
  {
    private readonly IAccountRepository<Account> _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public AccountService(
        IAccountRepository<Account> accountRepository,
        ITransactionRepository transactionRepository)
    {
      _accountRepository = accountRepository;
      _transactionRepository = transactionRepository;
    }

    public async Task<AccountDto> CreateAccount(CreateAccountDto account)
    {
      var newAccount = new Account()
      {
        FirstName = account.FirstName,
        LastName = account.LastName,
        Balance = account.InitialBalance,
      };

      await _accountRepository.CreateAccount(newAccount);
      return DtoMapper.MapToAccountDto(newAccount);
    }

    public async Task<bool> DeleteAccount(string accountid)
    {
      return await _accountRepository.DeleteAccount(accountid);
    }

    public async Task<AccountDto> GetAccountById(string id)
    {
      var account = await _accountRepository.GetAccountById(id);
      return DtoMapper.MapToAccountDto(account);
    }

    public async Task<IList<AccountDto>> GetAccounts(int size, int offset)
    {
      var accounts = await _accountRepository.GetAccounts(size, offset);
      return [.. accounts.Select(DtoMapper.MapToAccountDto)];
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
      var account = await _accountRepository.TransferFunds(fromAccount, toAccount, amount);
      await _transactionRepository.CreateTransaction(fromAccount, amount, toAccount);
      return DtoMapper.MapToAccountDto(account);
    }

    public async Task<AccountDto> UpdateAccountBalance(string accountid, double amount)
    {
      var account = await _accountRepository.UpdateAccountBalance(accountid, amount);
      await _transactionRepository.CreateTransaction(accountid, amount);
      return DtoMapper.MapToAccountDto(account);
    }
  }
}