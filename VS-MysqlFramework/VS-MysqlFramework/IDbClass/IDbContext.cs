using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_MysqlFramework.IDbClass
{
    public interface IDbContext
    {
        Task<DataTable> GetDataWithStoredProcedure(object stored_procedure); 
        Task<List<T>> GetDataWithStoredProcedure<T>(object stored_procedure); 
        Task<bool> ExecuteStoredProcedure(object stored_procedure);
        Task<DataTable> SelectQuery(string query); 
        Task<List<T>> SelectQuery<T>(string query); 
        Task<List<T>> SelectQuery<T>(string query, object param);
        Task<bool> ExecuteQuery(string query);
        Task<bool> ExecuteQuery(string query, object param);
        Task<bool> Insert(string query,object param); 
        Task<bool> Insert(object model);
        Task<bool> BulkInsert<T>(List<T> param);

    }
}
