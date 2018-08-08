# GetDataWithStoredProcedure<DataTable>
```c#
DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<DataTable> task = Task.Run(()=>dbContext.GetDataWithStoredProcedure(new my_procedure() { first_parameter="User 1" }));
            for (int i = 0; i < task.Result.Rows.Count; i++)
            {
                for (int k = 0; k < task.Result.Columns.Count; k++)
                {
                    Console.WriteLine(task.Result.Rows[i].ItemArray[k].ToString());
                }
            }
```
# GetDataWithStoredProcedure<List>
```c#
DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<List<model>> task = Task.Run(() => dbContext.GetDataWithStoredProcedure<model>(new my_procedure() { first_parameter = "User 1" }));
            foreach (var item in task.Result)
            {
                Console.WriteLine(item.username+":"+item.password);
            }
```