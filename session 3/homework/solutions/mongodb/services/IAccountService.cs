using mongodb.documents;
using mongodb.dtos;

namespace mongodb.services
{
  public interface IAccountService
  {
    public Task<IList<AccountDto>> GetAccounts(int size, int offset);

    public Task<AccountDto> GetAccountById(string id);

    public Task<AccountDto> CreateAccount(CreateAccountDto account);

    public Task<AccountDto> UpdateAccountBalance(string accountid, double amount);

    public Task<bool> DeleteAccount(string accountid);

    public Task<AccountDto> TransferFunds(string fromAccount, string toAccount, double amount);

    public Task<IList<TransactionDto>> GetAccountTransactions(string accountid);

    public Task<IList<TransactionSummaryDto>> GetAccountSummary(string accountid, TransactionSummaryUnit unit);
  }
}