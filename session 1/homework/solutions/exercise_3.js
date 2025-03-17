// Connect to a MongoDB Server using mongosh (from command prompt or in MongoDB Compass open the shell)
// As part of this solution we will be using MongoDB Atlas sample data that can be migrated to your own cluster.

var dbName = "sample_supplies";
var collectionName = "sales";

db = db.getSiblingDB(dbName); // You can also list the dbs with 'show dbs' and select a db with 'use dbName'
var collection = db.getCollection(collectionName);

// Find all documents where customer age is greater than 50 and less than 70 and project only customer, saleDate and purchaseMethod (exclude the id)
collection.find(
  {
    "customer.age": { $gte: 50, $lte: 70 }
  },
  {
    saleDate: 1,
    customer: 1,
    purchaseMethod: 1,
    _id: 0
  }
).toArray()

// returning the total count
collection.find({"customer.age": {$gte: 50}, "customer.age": {$lte: 70}}).count()

// return all sales where customer satisfaction is greater than 3 and purchaseMethod is "Online" or "Phone". Use same projection than before
collection.find({"customer.satisfaction": {$gt: 3}, purchaseMethod: {$in: ["Online", "Phone"]}}, {saleDate: 1, customer: 1, purchaseMethod: 1, _id: 0})

// return all sales where storeLocation is equal to "Denver" and saleDate was in 2015 or storeLocation is equal to "London" and purchaseMethod is "In store"
collection.find(
  {
    $or: [
      { storeLocation: "Denver", saleDate: { $gte: new Date("2015-01-01"), $lt: new Date("2016-01-01") } },
      { storeLocation: "London", purchaseMethod: "In store" }
    ]
  }
).toArray()

collection.insertOne({
  _id: "1",
  saleDate: new Date("2025-02-06"),
  items: [
    {
      name: "laptop",
      tags: ["electronics", "office"],
      price: 699.99,
      quantity: 1
    }
  ],
  storeLocation: "Rio de Janeiro",
  customer: {
    gender: "M",
    age: 45,
    email: "user@email.com",
    satisfaction: 3
  },
  couponUsed: true,
  purchaseMethod: "In Store"
})

collection.updateOne(
  { _id: "1" },
  { 
    $set: { storeLocation: "Denver", "items.0.quantity": 2 },
    $inc: {"customer.age": 2 },
    $unset: { couponUsed: ""}
  }
)

// return all sales where array has at least one item
collection.find({items: {$exists: true, $ne: []}})

// return all sales where items array has only one item
collection.find({items: {$size: 1}})
collection.find({items: {$size: 1}}).count()

// return all sales where any element in items array has more than one element in the tags array property, project only the id and item tags property
collection.find({"items.tags": {$size: 1}}, {"items.tags": 1})

// return all sales where all elements in items array has a quantity propety greater than 5
collection.find({items: {"$not": {"$elemMatch": {"quantity": {"$lte": 5}}}}})

// retun sales where items tags array has "school" and "general" elements. Project only tags
collection.find({"items.tags": {$all: ["school", "general"]}}, {"items.tags": 1})

// return all sales where firs element in items array quantity is equal to 5
collection.find({"items.0.quantity": 5})


collection.insertOne({
  _id: "2",
  saleDate: new Date("2025-02-06"),
  items: [
    {
      name: "laptop",
      tags: ["electronics", "office"],
      price: 699.99,
      quantity: 1
    },
    {
      name: "mouse",
      tags: ["electronics", "office"],
      price: 50.00,
      quantity: 3
    }
  ],
  storeLocation: "Rio de Janeiro",
  customer: {
    gender: "M",
    age: 32,
    email: "user@email.com",
    satisfaction: 5
  },
  couponUsed: false,
  purchaseMethod: "Online"
})

collection.updateOne({_id: "2", "items.name": "mouse"}, {$set: {"items.$.quantity": 5}})
collection.updateOne({_id: "2"}, {$set: {"items.$[].discount": true}})
collection.updateOne(
  {_id: "2"},
  {$set: {"items.$[element].discount": false}},
  {arrayFilters: [{"element.name": "laptop"}]}
)


collection.updateOne({_id: "2"}, {$push: {"items.$[].tags": "school"}})
collection.updateOne({_id: "2"}, {$push: {"items.$[].tags": { $each: ["general", "personal"] }}})
collection.updateOne({_id: "2", "items.name": "mouse"}, {$pop: {"items.$.tags": 1}})
collection.updateOne({_id: "2", "items.name": "mouse"}, {$pop: {"items.$.tags": -1}})
collection.updateOne({_id: "2", "items.name": "laptop"}, {$pull: {"items.$.tags": "general"}})