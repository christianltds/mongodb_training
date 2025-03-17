#### Connecting to a MongoDB Server using mongosh:  

1. Open your **command prompt** and run the following command (make sure you have [mongosh (Mongo Shell)](https://www.mongodb.com/try/download/shell)):  

   ```sh  
   mongosh <mongodb_connstring>  
   ```  

2. Replace `<mongodb_connstring>` with your actual connection string:  
   - If running a **local MongoDB server**, the default connection string is:  
     ```sh  
     mongodb://localhost:27017/  
     ```  
   - If connecting to a **MongoDB cluster**, the connection string follows this format:  
     ```sh  
     mongodb+srv://<username>:<userpassword>@<hostname>.mongodb.net  
     ```  

#### Connecting to a MongoDB Server using MongoDB Compass  

1. Open your **MongoDB Compass** GUI application and click on `New Connection`.  
2. Enter the **connection string** in the **URI box**.  
3. Optionally, assign a **name and/or color** to the connection and save it.  
4. Click on `Connect` or `Save and Connect` to test your connection.  