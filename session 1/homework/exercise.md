# Session 1 Exercises  

Set up a [MongoDB local server](https://www.mongodb.com/try/download/community) or create/use a free **playground cluster (M0)** in **MongoDB Atlas** (recommended). Connect to it using either [MongoDB Compass](https://www.mongodb.com/try/download/compass) or [mongosh (Mongo Shell)](https://www.mongodb.com/try/download/shell).  

If using a MongoDB **cluster**, it is recommended to load sample data into the cluster for training. To do this:  
1. Go to your **MongoDB Atlas** project.  
2. In the **Clusters** tab, find your **playground cluster**.  
3. Click on the **ellipsis button** (\u22EE) to the right of the **Connect** button.  
4. Select **Load Sample Dataset** and wait for the process to complete.  

Once loaded, you should have several **sample databases and collections** available for testing your queries.  

> **Note:** If using a MongoDB cluster, ensure you have a user with **read/write permissions** to complete the exercises successfully.  

---

## Exercise 1  
### Connect to a MongoDB server  

- Connect to a MongoDB server using **MongoDB Compass**.  
- Connect to a MongoDB server using **mongosh**.    

---

## Exercise 2  
### Perform CRUD operations  

- The goal of this exercise is to practice **CRUD operations** in MongoDB, including **insert, find, update, and delete**.  
- You do not need to follow a strict document schema—**MongoDB is a flexible NoSQL database**, so try inserting documents with different structures and data types.  
- The MongoDB ecosystem provides a **reserved object `db`** for accessing a given database.  

### Useful Commands  

- **List all databases:** `show dbs`  
- **Select a database:** `use {db_name}` or `db.getSiblingDB("dbname")`
- **List collections:** `show collections`  
- **Access a collection:** `db.collectionName.method()`  

#### CRUD Operations  

- **Insert documents:**  
  - `db.collectionName.insertOne(document)`  
  - `db.collectionName.insertMany(documents)`  

- **Find documents:**  
  - `db.collectionName.find(<filter>, <projection>, <options>)`  
  - `db.collectionName.findOne(<filter>, <projection>, <options>)`  

- **Update documents:**  
  - `db.collectionName.updateOne(filter, update, <options>)`  
  - `db.collectionName.updateMany(filter, update, <options>)`  
  - `db.collectionName.replaceOne(filter, replacement, <options>)`  
  - `db.collectionName.findOneAndReplace(filter, replacement, <options>)`  
  - `db.collectionName.findOneAndUpdate(filter, update, <options>)`  

- **Delete documents:**  
  - `db.collectionName.deleteOne(filter, <options>)`  
  - `db.collectionName.deleteMany(filter, <options>)`  
  - `db.collectionName.findOneAndDelete(filter, <options>)`  

---

## Exercise 3  
### Perform CRUD operations on documents with nested objects and arrays  

- Perform CRUD operations on **nested objects**.  
- Perform CRUD operations on **elements within an array**.  
- Perform CRUD operations on **elements within an array of objects**.  

---

## Extra Exercise 1  
### Create a hierarchical data model to represent an e-commerce platform and perform advanced queries  

- Design a **products collection** with **nested documents** for categories and subcategories.  
- Insert sample data, including **arrays of nested documents** to represent reviews for each product.  
- Write advanced queries to:  
  - Retrieve all products under a specific **category or subcategory**.  
  - Filter products based on the number of reviews (`$size`) and review ratings using `$elemMatch`.  
  - Use projections to return **only product names and reviews** for a specific date range.  

---
