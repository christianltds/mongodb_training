# Session 3 Exercises

Set up a [MongoDB local server](https://www.mongodb.com/try/download/community) or create/use a free **playground cluster (M0)** in **MongoDB Atlas** (recommended). Connect to it using either [MongoDB Compass](https://www.mongodb.com/try/download/compass) or [mongosh (Mongo Shell)](https://www.mongodb.com/try/download/shell).

If using a MongoDB **cluster**, it is recommended to load sample data into the cluster for training. To do this:

1. Go to your **MongoDB Atlas** project.
2. In the **Clusters** tab, find your **playground cluster**.
3. Click on the **ellipsis button** (⋮) to the right of the **Connect** button.
4. Select **Load Sample Dataset** and wait for the process to complete.

Once loaded, you should have several **sample databases and collections** available for testing your queries.

> **Note:** If using a MongoDB cluster, ensure you have a user with **read/write permissions** to complete the exercises successfully.

---

## Exercise 8
### Work with MongoDB Indexes

> **Note:** For each of the following exercises, run the explain report and check if the correct index is being used.

- Find or create a collection with many documents to compare the performance of queries with and without indexes (**COLLSCAN** vs **IXSCAN**).
- Create a simple single-field index in the desired order and test the query performance.
  - Sort the documents in ascending and descending order.

---

## Extra Exercise 3
### Build a Comprehensive Analytics Report Using the Aggregation Framework

- Use a `sample_supplies.sales` collection which contains sales transaction data (use 2 decimal numbers).
- Create an aggregation pipeline to:
  - Group sales data by storeLocation and calculate total sales, average quantity and revenue per transaction and total revenue
  - Use $project to format the final report, including the location, average values (quantity and revenue) and total revenue. Use 2 decimal numbers.
  - Sort results by total sales in descending order.
  - Use $dateTrunc to group sales by month and calculate total revenue.
  - Use $unwind on the items array to analyze product-level sales performance.

---
