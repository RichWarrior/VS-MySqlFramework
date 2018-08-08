# ExecuteQuery(Query)
```c#
 DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<bool> task = Task.Run(() => dbContext.ExecuteQuery("DROP TABLE dersler"));
            if (task.Result)
                Console.WriteLine("Query Execute Succesfully");
```
# ExecuteQuery(Query,Param)
```c#
 DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<bool> task = Task.Run(() => dbContext.ExecuteQuery("INSERT INTO users (username) VALUES(@param1)",new {param1="Admin" }));
            if (task.Result)
                Console.WriteLine("Query Execute Succesfully");
```
