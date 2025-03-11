// Connect to a MongoDB Server using mongosh (from command prompt or in MongoDB Compass open the shell)
// As part of this solution we will be using MongoDB Atlas sample data that can be migrated to your own cluster.

var dbName = "sample_training";
var collectionName = "grades";

db = db.getSiblingDB(dbName); // You can also list the dbs with 'show dbs' and select a db with 'use dbName'
var collection = db.getCollection(collectionName);


// queryPlanner.winningPlan - to check if it was a collscan or ixscan
// executionStats.executionTimeMillis - to check the time taken in the query


// Create a simple single-field index in the desired order and test the query performance
collection.find({ student_id:  9997}).explain("executionStats")

collection.createIndex( { student_id: 1 } )

collection.find({ student_id:  9997}).explain("executionStats")


// Sort the documents in ascending and descending order
collection.find({}).sort({ student_id: 1 }).explain("executionStats")
collection.find({}).sort({ student_id: -1 }).explain("executionStats")

collection.dropIndex("student_id_1") // delete the created index


// Create a compound index and compare the query performance.
collection.find({ student_id:  9997, class_id: 13 }).explain("executionStats")

collection.createIndex( { student_id: 1, class_id: 1 } )

collection.find({ student_id:  9997, class_id: 13 }).explain("executionStats") // should use the index


// Run queries using only part of the indexed fields and check if the index is used (order matters!). Identify cases where the index is not used.
collection.find({ student_id:  9997}).explain("executionStats") // should use the index, as student_id is the first field in the index

collection.find({ class_id:  13}).explain("executionStats") // should not use the index, as class_id is the second field in the index. Order needs to be followed


// Sort the documents in the order the index was created, then in reverse, and finally in random order.
collection.find({ student_id: 9997 }).sort({ class_id: 1}).explain("executionStats") // should use the index
collection.find({}).sort({ student_id: 1, class_id: 1}).explain("executionStats") // should use the index
collection.find({}).sort({ student_id: -1, class_id: -1}).explain("executionStats") // should use the index as it is the transverse order
collection.find({}).sort({ student_id: 1, class_id: -1}).explain("executionStats") // should not use the index as it is not the transverse order or crated order

collection.dropIndex("student_id_1_class_id_1") // delete the created index


// Create a multikey index (for array fields) and compare the query performance.
collection.find({ "scores.type": "exam" }).explain("executionStats")

collection.createIndex( { "scores.type": 1 } )

collection.find({ "scores.type": "exam" }).explain("executionStats") // should use index

collection.find({ "scores.type": { $in: ["exam", "quiz"]} }).explain("executionStats") // should use index

collection.find({ "scores.type": { $all: ["exam", "quiz"]} }).explain("executionStats") // should use index

collection.find({}).sort({ "scores.type": 1 }).explain("executionStats") // should use index

collection.dropIndex("scores.type_1")

// Create a text index and compare the query performance.
dbName = "sample_supplies";
collectionName = "sales";

db = db.getSiblingDB(dbName);
collection = db.getCollection(collectionName);

collection.createIndex( { "storeLocation": "text" } )

collection.find(
  { 
    $text: {
      $search: "Denver"
    }
  }
).explain("executionStats")

// Run text queries to test index functionalities.

collection.find(
  { 
    $text: {
      $search: "DENVER" // case insensitive
    }
  }
)

collection.find(
  { 
    $text: {
      $search: "DENVER"
    }
  },
  {
    score: {
      $meta: "textScore" // project the score
    }
  }
).sort({ score: { $meta: "textScore" } }) // sort by score

collection.dropIndex("storeLocation_text")

// Try a **wildcard index (`$**`)** for dynamically structured documents.

collection.createIndex( { "$**": "text" } )
collection.find(
  { 
    $text: {
      $search: "online" // checks on all the text fields, matches purchaseMethod
    }
  }
)

collection.find(
  { 
    $text: {
      $search: "pens" // checks on all the text fields. matches the name of an item in the items array
    }
  }
)

collection.dropIndex("$**_text")

// Create a geospatial index and try out some location-based queries.
dbName = "sample_geospatial";
collectionName = "shipwrecks";

db = db.getSiblingDB(dbName);
collection = db.getCollection(collectionName);

collection.createIndex( { "coordinates": "2dsphere" } ) // This index is already created by mongdodb sample data. Drop before creating

collection.find(
  { 
    coordinates: {
      $near: {
        $geometry: { type: "Point", coordinates: [ -79.9074173, 9.3560572 ] },
        $maxDistance: 1000
      }
    }
  }
).explain("executionStats")

// all documents that are 1000 meters maximum distance from the point	
collection.find(
  { 
    coordinates: {
      $near: {
        $geometry: { type: "Point", coordinates: [ -79.9074173, 9.3560572 ] },
        $maxDistance: 1000
      }
    }
  }
)

// point inside a polygon
collection.find({
  coordinates: {
    $geoWithin: {
      $geometry: {
        type: "Polygon",
        coordinates: [[
          [-79.9085, 9.3650], // Top-left
          [-79.9060, 9.3650], // Top-right
          [-79.9060, 9.3470], // Bottom-right
          [-79.9085, 9.3470], // Bottom-left
          [-79.9085, 9.3650]  // Closing point (same as first)
        ]]
      }
    }
  }
})
