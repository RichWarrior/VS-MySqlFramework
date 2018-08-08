# INSERT(Parameter)
```c#
DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<bool> task = Task.Run(()=>dbContext.Insert("INSERT INTO users(username,password) VALUES(@param1,@param2)",new {param1="admin",param2="password" }));
            if (task.Result)
                Console.WriteLine("Insert Successfully");
```
# INSERT(Model)
## Required Model
```c#
DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<bool> task = Task.Run(()=>dbContext.Insert(new users { username="Admin" }));
            if (task.Result)
                Console.WriteLine("Insert Successfully");
```
