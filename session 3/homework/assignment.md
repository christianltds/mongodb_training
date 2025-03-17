# Session 3 Exercises

Set up a [MongoDB local server](https://www.mongodb.com/try/download/community) or create/use a free **playground cluster (M0)** in **MongoDB Atlas** (recommended). Connect to it using either [MongoDB Compass](https://www.mongodb.com/try/download/compass) or [mongosh (Mongo Shell)](https://www.mongodb.com/try/download/shell).

---

> **Note:** The following exercises can be completed sequentially within the same solution.  
> It is recommended to create a Web API for managing bank accounts and transactions between them.  
> In these examples, two resources are considered: **account** and **transaction**. Each will be represented as a collection in the **banks** database.

> **Note:** For the following exercises, consider the following POCOs:

```csharp
 public class Account
 {
    string Id;
    string AccountId;
    string FirstName;
    string LastName;
    double Balance;
    DateTime CreatedAt;
    bool IsActive;
 }
```
```csharp
 public class Transaction
 {
    string Id;
    string AccountId;
    TransactionType Type
    double Amount;
    DateTime TransactionDate;
    string TargetAccountId;
 }
```
---

## Exercise 8  
### Create a Web API Application in C#

- Create a Web API application in C#.  
- Install the **MongoDB.Driver** NuGet package.  
- Connect to a MongoDB server using `IMongoClient` and inject it into your application.  
- Use the `IMongoClient` instance to access a MongoDB database and collection from the server.  
- Implement CRUD operations using the collection object and expose them through the API:  
  - **Get all accounts**  
  - **Create an account** – Make the initial balance an optional field.  
    - For any new account, increment the account id field with the next value, starting from 1
  - **Get an account by account ID**  
  - **Deactivate an account** – Implement soft delete.  
  - **Transfer funds between two accounts**
    - Send the source account and targe account id and the amount 
    - Return the updated source account document
  - **Update account balance**:  
    - Create a method for **depositing** funds.  
    - Create a method for **withdrawing** funds.  
    - Return the updated source account document

> **Note:** For each get operation, project your data to return only the required fields. Do not return the MongoDB auto generated id.

> **Note:** For each operation where the balance is updated (deposit, Withdrawal, and transfer), create a transaction document and insert it into the **transactions** collection.  
> - Include a reference to the **account ID** from the accounts collection.  
> - In the case of a transfer, also add a reference to the **target account ID**.  

---

## Exercise 9  
### Add Pagination Logic to Your Application  

- Extend your API and update the **Get all accounts** endpoint to support pagination.  

---

## Exercise 10  
### Implement a Transaction for Updating Two or More Resources  

- Add a method that starts and commits or rolls back a MongoDB transaction when transferring funds between two accounts.  
- Return the updated source account’s basic information.  

---

## Exercise 11  
### Write Aggregation Pipelines  

- Create an endpoint that returns all transactions of an account:  
  - Add a `$match` stage to filter transactions where the **transaction account ID** or **target account ID** matches the provided account ID.  
  - Add a `$lookup` stage to join the transactions with the **accounts** collection and return account objects. Exclude the balance from the response; only return the **ID** and **person name**.  

- Create another endpoint to return a summary of transactions based on a timestamp for a specific account ID:  
  - Use `$dateTrunc` to truncate the transaction date based on a unit (e.g., daily, monthly, or yearly) passed to the API.  
  - Group transactions based on the `$dateTrunc` result.  
  - Add an accumulator expression to compute the following:  
    - **totalCredit** – Sum of credit transactions.  
    - **totalDebit** – Sum of debit transactions.  
    - **totalTransaction** – Total number of transactions.  

---

## Extra Exercise 3  
### Improve Your Web API Application  

- Create indexes on the **account ID** in both the **accounts** and **transactions** collections.  
- Create an index on the **transaction date** field in the **transactions** collection.  
- Add a retry mechanism for MongoDB transactions.  
- Enable MongoDB logging to monitor the generated MQL expressions.  
- Add a balance check before processing any Withdrawal or transfer operation.  
- Add a check to ensure that the account is active before performing any operations.  
- **Bonus:** Create a repository using `BsonDocument` instead of a POCO class.  

---
