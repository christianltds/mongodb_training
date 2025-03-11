var dbName = "sample_supplies";
var collectionName = "sales";

db = db.getSiblingDB(dbName); 
var collection = db.getCollection(collectionName);

// Group sales data by storeLocation and calculate total sales and average sales per transaction.
var pipeline = [
    {
      $addFields: {
        totalQuantity: {
          $sum: "$items.quantity" // adding a fielf for the total quantity of all items in the items array
        },
        totalRevenue: {
          $multiply: [{$sum: "$items.quantity"}, {$sum: "$items.price"}] // adding a field for the total price of all items in the items array
        }
      }
    },
    {
      $group: {
        _id: "$storeLocation", // group by location
        totalSales: {
          $sum: 1 // total sales for each location
        },
        totalRevenue: {
          $sum: "$totalRevenue" // total revenue for each location
        },
        avgQtd: {
          $avg: "$totalQuantity" // average quantity of items sold
        },
        avgRevenue: {
          $avg: "$totalRevenue" // average revenue for each location
        }
      }
    },
    {
      $project: {
        totalSales: 1,
        avgQtd: {
          $round: ["$avgQtd", 2] // round valus in 2 decimals
        },
        avgRevenue: {
          $round: ["$avgRevenue", 2]
        },
        totalRevenue: {
          $round: ["$totalRevenue", 2]
        }
      }
    },
    {
      $sort: {
        totalSales: -1 // sort by sales in descending order
      }
    }
  ];

collection.aggregate(pipeline).toArray();

pipeline = [
    {
      $group: {
        _id: { // group by month. $dateTrunc will get the first date of the month for each year and group the documents
          month: {
            $dateTrunc: {
              date: "$saleDate",
              unit: "month"
            }
          }
        },
        totalRevenue: {
          $sum: {
            $sum: "$items.price" // sum the array of prices in the items array and then sum the total
          }
        }
      }
    },
    {
      $sort: {
        "_id.month": 1 // sort by month in ascending order
      }
    }
  ];

collection.aggregate(pipeline).toArray();

pipeline = [
    { $unwind: "$items" }, // flatten the items array
    {
      $group: {
        _id: "$items.name", // group by item name
        totalQuantitySold: {
          $sum: "$items.quantity" // get the total quantity sold by each item
        },
        totalRevenue: { // get the total revenue for each item
          $sum: {
            $multiply: [
              "$items.price",
              "$items.quantity"
            ]
          }
        }
      }
    },
    { $sort: { totalRevenue: -1 } } // sort by total revenue in descending order
  ];

collection.aggregate(pipeline).toArray();
