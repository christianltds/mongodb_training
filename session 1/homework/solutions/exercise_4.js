// Connect to a MongoDB Server using mongosh (from command prompt or in MongoDB Compass open the shell)

var dbName = "exercise_4";
var collectionName = "products"

db = db.getSiblingDB(dbName); // You can also list the dbs with 'show dbs' and select a db with 'use dbName'
var collection = db.getCollection(collectionName);

const products = [
  {
    name: "laptop",
    price: 600.00,
    quantity: 10,
    category: {
      name: "electronics",
      subcategories: ["office", "home"]
    },
    reviews: [
      {
        rating: 4,
        comment: "Good product",
        createdAt: new Date("2021-10-01")
      },
      {
        rating: 5,
        comment: "Excellent product",
        createdAt: new Date("2021-11-15")
      },
      {
        rating: 3,
        comment: "Average product",
        createdAt: new Date("2021-12-20")
      }
    ]
  },
  {
    name: "smartphone",
    price: 300.00,
    quantity: 50,
    category: {
      name: "electronics",
      subcategories: ["mobile", "gadgets"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Amazing phone",
        createdAt: new Date("2022-01-10")
      },
      {
        rating: 4,
        comment: "Good value",
        createdAt: new Date("2022-02-15")
      }
    ]
  },
  {
    name: "headphones",
    price: 50.00,
    quantity: 100,
    category: {
      name: "electronics",
      subcategories: ["audio", "accessories"]
    },
    reviews: [
      {
        rating: 4,
        comment: "Great sound quality",
        createdAt: new Date("2022-03-20")
      },
      {
        rating: 3,
        comment: "Comfortable but average sound",
        createdAt: new Date("2022-04-25")
      }
    ]
  },
  {
    name: "tablet",
    price: 200.00,
    quantity: 30,
    category: {
      name: "electronics",
      subcategories: ["mobile", "gadgets"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Very useful",
        createdAt: new Date("2022-05-30")
      },
      {
        rating: 4,
        comment: "Good performance",
        createdAt: new Date("2022-06-15")
      }
    ]
  },
  {
    name: "monitor",
    price: 150.00,
    quantity: 20,
    category: {
      name: "electronics",
      subcategories: ["office", "home"]
    },
    reviews: [
      {
        rating: 4,
        comment: "Clear display",
        createdAt: new Date("2022-07-20")
      },
      {
        rating: 3,
        comment: "Average build quality",
        createdAt: new Date("2022-08-25")
      }
    ]
  },
  {
    name: "keyboard",
    price: 30.00,
    quantity: 200,
    category: {
      name: "electronics",
      subcategories: ["office", "accessories"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Very comfortable",
        createdAt: new Date("2022-09-30")
      },
      {
        rating: 4,
        comment: "Good for typing",
        createdAt: new Date("2022-10-15")
      }
    ]
  },
  {
    name: "mouse",
    price: 20.00,
    quantity: 150,
    category: {
      name: "electronics",
      subcategories: ["office", "accessories"]
    },
    reviews: [
      {
        rating: 4,
        comment: "Smooth and responsive",
        createdAt: new Date("2022-11-20")
      },
      {
        rating: 3,
        comment: "Average build",
        createdAt: new Date("2022-12-25")
      }
    ]
  },
  {
    name: "printer",
    price: 100.00,
    quantity: 40,
    category: {
      name: "electronics",
      subcategories: ["office", "home"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Excellent print quality",
        createdAt: new Date("2023-01-30")
      },
      {
        rating: 4,
        comment: "Easy to use",
        createdAt: new Date("2023-02-15")
      }
    ]
  },
  {
    name: "router",
    price: 80.00,
    quantity: 60,
    category: {
      name: "electronics",
      subcategories: ["networking", "home"]
    },
    reviews: [
      {
        rating: 4,
        comment: "Good coverage",
        createdAt: new Date("2023-03-20")
      },
      {
        rating: 3,
        comment: "Average speed",
        createdAt: new Date("2023-04-25")
      }
    ]
  },
  {
    name: "webcam",
    price: 40.00,
    quantity: 70,
    category: {
      name: "electronics",
      subcategories: ["office", "accessories"]
    },
    reviews: [
      {
        rating: 5,
        comment: "High quality video",
        createdAt: new Date("2023-05-30")
      },
      {
        rating: 4,
        comment: "Easy to set up",
        createdAt: new Date("2023-06-15")
      }
    ]
  },
  {
    name: "book",
    price: 15.00,
    quantity: 100,
    category: {
      name: "literature",
      subcategories: ["fiction", "novel"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Great read",
        createdAt: new Date("2023-01-10")
      },
      {
        rating: 4,
        comment: "Interesting story",
        createdAt: new Date("2023-02-15")
      }
    ]
  },
  {
    name: "chair",
    price: 45.00,
    quantity: 80,
    category: {
      name: "furniture",
      subcategories: ["office", "home"]
    },
    reviews: [
      {
        rating: 4,
        comment: "Comfortable",
        createdAt: new Date("2023-03-20")
      },
      {
        rating: 3,
        comment: "Average build quality",
        createdAt: new Date("2023-04-25")
      }
    ]
  },
  {
    name: "desk",
    price: 120.00,
    quantity: 30,
    category: {
      name: "furniture",
      subcategories: ["office", "home"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Spacious and sturdy",
        createdAt: new Date("2023-05-30")
      },
      {
        rating: 4,
        comment: "Good value",
        createdAt: new Date("2023-06-15")
      }
    ]
  },
  {
    name: "blender",
    price: 60.00,
    quantity: 50,
    category: {
      name: "appliances",
      subcategories: ["kitchen", "home"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Powerful and efficient",
        createdAt: new Date("2023-07-20")
      },
      {
        rating: 4,
        comment: "Easy to clean",
        createdAt: new Date("2023-08-25")
      }
    ]
  },
  {
    name: "vacuum cleaner",
    price: 150.00,
    quantity: 40,
    category: {
      name: "appliances",
      subcategories: ["cleaning", "home"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Very effective",
        createdAt: new Date("2023-09-30")
      },
      {
        rating: 4,
        comment: "Good suction power",
        createdAt: new Date("2023-10-15")
      }
    ]
  },
  {
    name: "bicycle",
    price: 200.00,
    quantity: 20,
    category: {
      name: "sports",
      subcategories: ["outdoor", "fitness"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Smooth ride",
        createdAt: new Date("2023-11-20")
      },
      {
        rating: 4,
        comment: "Good build quality",
        createdAt: new Date("2023-12-25")
      }
    ]
  },
  {
    name: "t-shirt",
    price: 20.00,
    quantity: 150,
    category: {
      name: "clothing",
      subcategories: ["casual", "men"]
    },
    reviews: [
      {
        rating: 4,
        comment: "Comfortable and stylish",
        createdAt: new Date("2024-01-30")
      },
      {
        rating: 3,
        comment: "Average quality",
        createdAt: new Date("2024-02-15")
      }
    ]
  },
  {
    name: "coffee maker",
    price: 80.00,
    quantity: 60,
    category: {
      name: "appliances",
      subcategories: ["kitchen", "home"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Makes great coffee",
        createdAt: new Date("2024-03-20")
      },
      {
        rating: 4,
        comment: "Easy to use",
        createdAt: new Date("2024-04-25")
      }
    ]
  },
  {
    name: "backpack",
    price: 50.00,
    quantity: 70,
    category: {
      name: "accessories",
      subcategories: ["travel", "school"]
    },
    reviews: [
      {
        rating: 5,
        comment: "Very spacious",
        createdAt: new Date("2024-05-30")
      },
      {
        rating: 4,
        comment: "Durable material",
        createdAt: new Date("2024-06-15")
      }
    ]
  }
];

collection.insertMany(products);

collection.find({"category.name": "electronics"}, {name: 1, price: 1, category: 1})

collection.find({"category.name": "electronics", "category.subcategories": {$ne: "office"}}, {name: 1, price: 1, category: 1})

collection.find({
  $or: [ 
    {"category.subcategories": "home"},
    {"category.name": "electronics", "category.subcategories": "audio"} ]}, 
    {name: 1, price: 1, category: 1}
  )

collection.find({"category.subcategories": {$all: ["home", "office"]}}, {name: 1, price: 1, category: 1})
collection.find({reviews: {$size: 2}}, {name: 1, "reviews.comment": 1, "reviews.rating": 1})
collection.find({reviews: {$elemMatch: {rating: 5, comment: /great/i }}}, {name: 1, "reviews.comment": 1, "reviews.rating": 1})

collection.find(
  {reviews: {$elemMatch: {createdAt: {$gte: new Date("2024-01-01"), $lt: new Date("2025-01-01")}}}},
  {name: 1, reviews: 1}
)
