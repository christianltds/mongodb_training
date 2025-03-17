using mongodb.documents;
using mongodb.dtos;
using mongodb.services;

namespace mongodb.apis
{
  public static class AccountsApis
  {
    public static void MapAccountsApis(this WebApplication app)
    {
      var accounts = app.MapGroup("bank/accounts");

      accounts.MapGet("/", GetAllAccounts).WithName("GetAccounts");

      accounts.MapGet("/{accountid}", GetAccountByAccountId).WithName("GetAccountByAccountId");

      accounts.MapPost("/", CreateAccount).WithName("CreateAccount");

      accounts.MapDelete("/{accountid}", DeleteAccount).WithName("DeleteAccount");

      accounts.MapPost("/transfer", TransferFunds).WithName("TransferFunds");

      accounts.MapGet("/{accountid}/summary", GetAccountSummary).WithName("GetAccountSummary");

      accounts.MapGet("/{accountid}/transactions", GetAccountTransactions).WithName("GetAccountTransactions");

      accounts.MapPut("/{accountid}/deposit", DepositAccountBalance).WithName("DepositAccountBalance");

      accounts.MapPut("/{accountid}/withdrawal", WithdrawalAccountBalance).WithName("WithdrawalAccountBalance");
    }

    public static async Task<IResult> GetAllAccounts(int size, int offset, IAccountService service)
    {
      return TypedResults.Ok(await service.GetAccounts(size, offset));
    }

    public static async Task<IResult> GetAccountByAccountId(string accountid, IAccountService service)
    {
      var account = await service.GetAccountById(accountid);

      if(account == null)
      {
        return TypedResults.NotFound();
      }

      return TypedResults.Ok(account);
    }

    public static async Task<IResult> CreateAccount(CreateAccountDto account, IAccountService service)
    {
      return TypedResults.Ok(await service.CreateAccount(account));
    }

    public static async Task<IResult> DeleteAccount(string accountid, IAccountService service)
    {
      return TypedResults.Ok(await service.DeleteAccount(accountid));
    }

    public static async Task<IResult> TransferFunds(TransferFundsDto transfer, IAccountService service)
    {
      return TypedResults.Ok(await service.TransferFunds(transfer.FromAccount, transfer.ToAccount, transfer.Amount));
    }

    public static async Task<IResult> GetAccountSummary(
        string accountid,
        TransactionSummaryUnit unit,
        IAccountService service)
    {
      return TypedResults.Ok(await service.GetAccountSummary(accountid, unit));
    }

    public static async Task<IResult> GetAccountTransactions(string accountid, IAccountService service)
    {
      return TypedResults.Ok(await service.GetAccountTransactions(accountid));
    }

    public static async Task<IResult> DepositAccountBalance(string accountid, double amount, IAccountService service)
    {
      var correctedAmount = Math.Abs(amount);
      var account = await service.UpdateAccountBalance(accountid, correctedAmount);

      if(account == null)
      {
        return TypedResults.NotFound();
      }

      return TypedResults.Ok(account);
    }

    public static async Task<IResult> WithdrawalAccountBalance(string accountid, double amount, IAccountService service)
    {
      var correctedAmount = Math.Abs(amount);

      var account = await service.UpdateAccountBalance(accountid, -correctedAmount);

      if(account == null)
      {
        return TypedResults.NotFound();
      }

      return TypedResults.Ok(account);
    }
  }
}