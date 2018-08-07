using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Gidertakip.ProcedureService._Interfaces
{
    public interface Interface_MySQL
    {
        Task<DataTable> GetDataTableFromProcedureDataTable(object data);
        Task<List<T>> GetDataTableFromProcedureList<T>(object data);
    }
}
