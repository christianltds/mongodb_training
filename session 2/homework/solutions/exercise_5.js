function runTransaction(session, value, collection) {
  try {
      console.log(`Alice is trying to send $${value} to Bob`)
      session.startTransaction();

      // Fetch sender's account
      const sender = collection.findOne({ accountId: 1 });
      if (!sender || sender.balance < value) {
          throw new Error("Insufficient funds for transaction.");
      }

      // Debit
      collection.updateOne(
          { accountId: 1 },
          { $inc: { balance: -value } }
      );

      // Credit
      collection.updateOne(
          { accountId: 2 },
          { $inc: { balance: value } }
      );

      // Commit the transaction
      session.commitTransaction();
      console.log("Transaction committed successfully.");
  } catch (error) {
      console.error("Transaction failed:", error.message);
      session.abortTransaction();
      console.log("Transaction rolled back.");
      throw error
  } finally {
    // Validate balances after transaction
    const finalAccounts = collection.find({}).toArray();
    console.log("Account Balances after transaction:", finalAccounts);
  }
}


function Run(){
  var dbName = "bank";
  var collectionName = "accounts";
  const session = db.getMongo().startSession();

  try {
    var collection = session.getDatabase(dbName).getCollection(collectionName);

    collection.updateOne(
        { _id: 1 },
        { $set: { accountId: 1, name: "Alice", balance: 500 } },
        { upsert: true }
    );
    collection.updateOne(
        { _id: 2 },
        { $set: { accountId: 2, name: "Bob", balance: 200 } },
        { upsert: true }
    );

    let attempt = 0;
    const maxRetries = 3;

    while (attempt < maxRetries) {
        try {
            runTransaction(session, 1500 / (attempt + 1), collection); 
            break; // Exit if transaction succeeds
        } catch (error) {
            console.warn(`Retrying transaction (Attempt ${attempt + 1}/${maxRetries})`);
            attempt++;
            if (attempt === maxRetries) {
                console.error("Transaction failed after multiple retries.");
            }
        }
    }
  } finally {
    session.endSession();
  }
}

Run();