var dbName = "sample_supplies";
var collectionName = "sales";

db = db.getSiblingDB(dbName); 
var collection = db.getCollection(collectionName);

var pipeline = [
    { $match: { storeLocation: "Denver" } }, // Filter sales in Denver
    { $unwind: "$items" }, // Flatten array of items in each sale
    { 
        $group: { 
            _id: "$items.name", // group by item
            totalRevenue: { $sum: { $multiply: ["$items.price", "$items.quantity"] } }, // Total value sold
            avgQuantity: { $avg: "$items.quantity" }, // average of quantity in a sale
            totalSales: { $sum: 1 }, // total sales done
            location: { $first: "$storeLocation" } // they are all the same, so just get the value from the first element
        } 
    },
    { $sort: { totalRevenue: -1 } }, // Sort by revenue (highest first)
    { $limit: 5 }, // Get top 5 best-selling products
    { 
        $project: { 
            productName: "$_id", 
            totalRevenue: 1, 
            avgQuantity: 1, 
            totalSales: 1, 
            location: 1,
            _id: 0 
        } 
    }
];

// Run the aggregation pipeline and return the top 5 best-selling products in Denver
collection.aggregate(pipeline).toArray();

pipeline = [
  {
      $facet: {
          topStores: [ // return the top 3 store locations with the most sales
              { $group: { _id: "$storeLocation", totalSales: { $sum: 1 } } },
              { $sort: { totalSales: -1 } },
              { $limit: 3 }
          ],
          salesByMonth: [ // return the total sale count for each month
              { $addFields: { month: { $substr: ["$saleDate", 5, 2] } } },
              { $group: { _id: "$month", totalSales: { $sum: 1 } } },
              { $sort: { _id: 1 } }
          ]
      }
  }
];

console.log(collection.aggregate(pipeline).toArray())