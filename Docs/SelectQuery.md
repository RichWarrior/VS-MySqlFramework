# SelectQuery(DataTable)
```c#
DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<DataTable> task = Task.Run(()=>dbContext.SelectQuery("SELECT * FROM users"));
            for (int i = 0; i < task.Result.Rows.Count; i++)
            {
                for (int k = 0; k < task.Result.Columns.Count; k++)
                {
                    Console.WriteLine(task.Result.Rows[i].ItemArray[k]);
                }
            }
```
# SelectQuery(DataTable,Parameter)
```c#
DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<DataTable> task = Task.Run(() => dbContext.SelectQuery("SELECT * FROM users WHERE username=@u",new { u="User 1"}));
            for (int i = 0; i < task.Result.Rows.Count; i++)
            {
                for (int k = 0; k < task.Result.Columns.Count; k++)
                {
                    Console.WriteLine(task.Result.Rows[i].ItemArray[k]);
                }
            }
```
# SelectQuery(List)
## Required Model
```c#
DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<List<model>> task = Task.Run(()=>dbContext.SelectQuery<model>("SELECT * FROM users"));
            foreach (var item in task.Result)
            {
                Console.WriteLine(item.username+":"+item.password);
            }
```
# SelectQuery(List,Parameter)
```c#
DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
 Task<List<model>> task = Task.Run(() => dbContext.SelectQuery<model>("SELECT * FROM users WHERE username=@user",new {user="User 1" }));
            foreach (var item in task.Result)
            {
                Console.WriteLine(item.username + ":" + item.password);
            }
```
