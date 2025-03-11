# Session 2 Exercises

Set up a [MongoDB local server](https://www.mongodb.com/try/download/community) or create/use a free **playground cluster (M0)** in **MongoDB Atlas** (recommended). Connect to it using either [MongoDB Compass](https://www.mongodb.com/try/download/compass) or [mongosh (Mongo Shell)](https://www.mongodb.com/try/download/shell).

If using a MongoDB **cluster**, it is recommended to load sample data into the cluster for training. To do this:

1. Go to your **MongoDB Atlas** project.
2. In the **Clusters** tab, find your **playground cluster**.
3. Click on the **ellipsis button** (⋮) to the right of the **Connect** button.
4. Select **Load Sample Dataset** and wait for the process to complete.

Once loaded, you should have several **sample databases and collections** available for testing your queries.

> **Note:** If using a MongoDB cluster, ensure you have a user with **read/write permissions** to complete the exercises successfully.

---

## Exercise 4  
### Work with MongoDB Indexes

> **Note:** For each of the following exercises, run the explain report and check if the correct index is being used.

- Find or create a collection with many documents to compare the performance of queries with and without indexes (**COLLSCAN** vs **IXSCAN**).
- Create a simple single-field index in the desired order and test the query performance.
  - Sort the documents in ascending and descending order.
- Create a compound index and compare the query performance.
  - Run queries using only part of the indexed fields and check if the index is used (order matters!). Identify cases where the index is not used.
  - Sort the documents in the order the index was created, then in reverse, and finally in random order.
- Create a multikey index (for array fields) and compare the query performance.
- Create a text index and compare the query performance.
  - Run text queries to test index functionalities.
  - Try a **wildcard index (`$**`)** for dynamically structured documents.
- Create a geospatial index and try out some location-based queries.

---

## Exercise 5  
### Creating Transactions

- Create a simple transaction, such as a bank transaction, where two documents must be updated in a single transaction.
  - Run a valid transaction and commit it.
  - Run an invalid transaction and roll it back.
- Query the collection to validate if the operations were committed or rolled back.
- Implement a **retry mechanism** to handle transient failures in transactions.
- Add **update conditions** (e.g., check if a balance is sufficient before debiting an account).

---

## Exercise 6  
### Create and Run Aggregation Pipelines

- Create and run aggregation pipelines with single and multiple stages, experimenting with different stage operators.
- Use aggregation operators effectively.
- **Additional Exercises:**
  - Use `$facet` to return multiple grouped results in a single query (e.g., top stores and sales by month).

### Useful and Common Stages:
- `$match`
- `$project`
- `$unwind`
- `$group`
- `$sort`
- `$limit`
- `$addFields`
- `$count`
- `$skip`
- `$lookup`
- `$merge`

---

## Exercise 7  
### Create and Test JSON Schemas

- Define a JSON schema for a specific collection.
  - Create the JSON schema with **strict** and **error** validation levels.
  - Try inserting valid documents.
  - Try inserting invalid documents and observe the outputs.
- Experiment with various validation rules:
  - Required fields
  - Data types
  - Minimum/maximum values
  - Enums
  - Regex validation
- Use `$jsonSchema` queries to filter documents based on schema rules.

---

## Extra Exercise 2  
### Build a Comprehensive Analytics Report Using the Aggregation Framework

- Use a `sample_supplies.sales` collection which contains sales transaction data (use 2 decimal numbers).
- Create an aggregation pipeline to:
  - Group sales data by storeLocation and calculate total sales, average quantity and revenue per transaction and total revenue
  - Use $project to format the final report, including the location, average values (quantity and revenue) and total revenue. Use 2 decimal numbers.
  - Sort results by total sales in descending order.
  - Use $dateTrunc to group sales by month and calculate total revenue.
  - Use $unwind on the items array to analyze product-level sales performance.

---
