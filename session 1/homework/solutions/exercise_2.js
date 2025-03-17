// Connect to a MongoDB Server using mongosh (from command prompt or in MongoDB Compass open the shell)
// As part of this solution we will be using MongoDB Atlas sample data that can be migrated to your own cluster.

var dbName = "sample_supplies";
var collectionName = "sales";

db = db.getSiblingDB(dbName); // You can also list the dbs with 'show dbs' and select a db with 'use dbName'
var collection = db.getCollection(collectionName);

// list the first 5 documents
collection.find().limit(5)

// Find all documents where sale date is between 2019-01-01 and 2020-01-01 and project only customer, saleDate and purchaseMethod (exclude the id)
collection.find({saleDate: {$gte: new Date("2019-01-01"), $lt: new Date("2020-01-01")}}, {saleDate: 1, customer: 1, purchaseMethod: 1, _id: 0})

// Now return the total count
collection.countDocuments({saleDate: {$gte: new Date("2015-01-01"), $lt: new Date("2016-01-01")}})


var dbName = "exercises";
var collectionName = "exercise_2";

db = db.getSiblingDB(dbName); // You can also list the dbs with 'show dbs' and select a db with 'use dbName'
var collection = db.getCollection(collectionName);


// Inserting a single document into the collection
collection.insertOne({
    name: "John",
    age: 30,
    email: "john@email.com"
})

// Inserting multiples documents in a single write operation
collection.insertMany([
  {
    name: "book1",
    pages: 324,
    publisheddate: new Date("2024-10-02"),
    genre: ""
  },
  {
    name: "book2",
    pages: 245,
    publisheddate: new Date("2021-04-12"),
    author: "Andre Smith"
  },
  {
    id: 123,
    name: "Bob",
    age: 3,
    specie: "Dog",
    owner: "Alice",
    nickname: "Bobby"
  },
  {
    id: "123",
    name: "Charlotte",
    age: 3,
    specie: "Cat",
    owner: "John",
    nickname: "Charlie"
  }
])

collection.find()

// List all documents where age are greater than 3
collection.find({age: {$gt: 3}})

// List all documents where age are greater than 3 or pages are greater than 300
collection.find({$or: [{age: {$gt: 3}}, {pages: {$gt: 300}}]})


// Updating a single document where age is equal to 3. We have two records that matched this filter, but just the first ocurrence should be updated
collection.updateOne({age: 3}, {$set: {name: "Billy"}})

// Both documents are return, but just one got updated
collection.find({age: 3}).toArray()

// Updating multiple documents where age is equal to 3. Both records should be updated, as both matches the filter.
collection.updateMany({age: 3}, {$set: {name: "Max"}})

// Both documents are return, an both got updated
collection.find({age: 3})

// We can replace one document completly using the replaceOne command
collection.replaceOne({name: "book2"}, {name: "book3", author: "Alice"})

// won't return any document
collection.find({name: "book2"})

// will return the new replaced document
collection.find({name: "book3"})

// Replace one document and return it as a result. returnNewDocument set to false will return the original document, if true will return te new version. Returns null if no match
collection.findOneAndReplace({name: "book3"}, {name: "book4", author: "John"}, {returnNewDocument: false})

// Update one document and return it as a result. returnNewDocument set to false will return the original document, if true will return te new version. Returns null if no match
collection.findOneAndUpdate({name: "John"}, {$set: {lastname: "Smith"}, $inc: {age: 1}}, {returnDocument: "after"})

// Deletes one document
collection.deleteOne({name: "book4"})

// Deletes many documents
collection.deleteMany({age: 3})

// Deletes a document and returns it
collection.findOneAndDelete({lastname: "Smith"})
