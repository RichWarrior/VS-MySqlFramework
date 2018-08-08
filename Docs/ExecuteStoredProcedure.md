# ExecuteStoredProcedure
```c#
 DbClass.DbContext dbContext = new DbClass.DbContext("Server=localhost;Database=deneme;Uid=root;Pwd=1234;Ssl Mode=none");
            Task<bool> task = Task.Run(()=>dbContext.ExecuteStoredProcedure(new my_procedure() {first_parameter="User 1" }));
            if (task.Result)
                Console.WriteLine("Stored Procedure Execute Successfully");
}
```