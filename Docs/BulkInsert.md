# Bulk Insert
```c#

            DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            List<users> data = new List<users>();
            for (int i = 0; i < 50; i++)
            {
                data.Add(new users {
                    username = "User "+(i+1),
                    password = "Pass "+(i+1)
                });
            }

            Task<bool> task = Task.Run(() => dbContext.BulkInsert<users>(data));
            if (task.Result)
                Console.WriteLine("Insert Successfully");
```