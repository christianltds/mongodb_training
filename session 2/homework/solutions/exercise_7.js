const schema = {
  $jsonSchema: {
      bsonType: "object",
      required: ["name", "email", "age", "status"],
      properties: {
          name: {
              bsonType: "string",
              description: "Customer name must be a string and is required."
          },
          email: {
              bsonType: "string",
              pattern: "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$",
              description: "Email must be a valid format."
          },
          age: {
              bsonType: "int",
              minimum: 18,
              maximum: 100,
              description: "Age must be an integer between 18 and 100."
          },
          status: {
              enum: ["Active", "Inactive", "Pending"],
              description: "Status must be one of: Active, Inactive, Pending."
          },
          phone: {
              bsonType: "string",
              pattern: "^[0-9]{10}$",
              description: "Phone number must be exactly 10 digits."
          },
          address: {
              bsonType: "object",
              required: ["city", "zip"],
              properties: {
                  city: {
                      bsonType: "string",
                      description: "City must be a string."
                  },
                  zip: {
                      bsonType: "string",
                      pattern: "^[0-9]{5}$",
                      description: "Zip code must be exactly 5 digits."
                  }
              }
          }
      }
  }
};

var dbName = "users";
var collectionName = "users";

db = db.getSiblingDB(dbName); 
db.createCollection("users", { validator: schema });
var collection = db.getCollection(collectionName);

const validUser = {
  name: "John Doe",
  email: "john.doe@example.com",
  age: 30,
  status: "Active",
  phone: "1234567890",
  address: { city: "New York", zip: "10001" }
};

const invalidUser = {
  name: "Invalid User",
  email: "invalid-email",
  age: 17,  // Below minimum age
  status: "Unknown",  // Not in enum
  phone: "12345",  // Too short
  address: { city: "Los Angeles" }  // Missing 'zip'
};

collection.insertOne(validUser);
collection.insertOne(invalidUser);


const query = {
  $jsonSchema: {
      bsonType: "object",
      properties: {
          phone: {
              bsonType: "string",
              pattern: "^[0-9]{10}$"
          }
      }
  }
};

collection.find(query).toArray();