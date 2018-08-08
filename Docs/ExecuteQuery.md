# ExecuteQuery
```c#
 DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<bool> task = Task.Run(() => dbContext.ExecuteQuery("DROP TABLE dersler"));
            if (task.Result)
                Console.WriteLine("Query Execute Succesfully");
```