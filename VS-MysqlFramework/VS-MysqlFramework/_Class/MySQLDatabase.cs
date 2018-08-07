using Gidertakip.ProcedureService._Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace Gidertakip.ProcedureService._Class
{
    public class MySQLDatabase : Interface_MySQL
    {
        private string _connectionString = "Server=localhost;Database=bazooka_gidertakip_test;Uid=root;Password=;SslMode=none";
        //private string _connectionString = "Server=Localhost;Database=sehirler;Uid=root;Password=1234;Pooling=true;SslMode=none";
        public async Task<DataTable> GetDataTableFromProcedureDataTable(object data)
        {
            var con = new MySqlConnection(_connectionString);
            DataTable dt = new DataTable();
            try
            {
                string procedure_name = data.GetType().Name;
                string query = "CALL " + procedure_name + " (";
                IList<PropertyInfo> properties = new List<PropertyInfo>(data.GetType().GetProperties());
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                foreach (PropertyInfo item in properties)
                {
                    //query += item.GetValue(data, null) + ",";
                    Type type = item.GetValue(data, null).GetType();
                    if (type.Equals(typeof(string)))
                    {
                        query += "'" + item.GetValue(data, null) + "',";
                    }else
                    {
                        query += item.GetValue(data, null) + ",";
                    }
                }
                query = query.TrimEnd(',');
                query += ")";
                var adapter = new MySqlDataAdapter(query, con);
                await adapter.FillAsync(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return dt;
        }
        public async Task<List<T>> GetDataTableFromProcedureList<T>(object data)
        {
            List<T> result = new List<T>();
            var obj = typeof(T);
            var con = new MySqlConnection(_connectionString);
            try
            {
                //string Query = "CALL " + data.GetType().Name + " (";
                string Query = "CALL "+data.GetType().Name + " (";
                IList<PropertyInfo> properties = new List<PropertyInfo>(data.GetType().GetProperties());
                IList<PropertyInfo> propertiesGeneric = new List<PropertyInfo>(obj.GetProperties());
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                foreach (PropertyInfo item in properties)
                {
                    //Query += item.GetValue(data, null) + ",";
                    Query += "@" + item.Name+",";
                    //cmd.Parameters.AddWithValue("@"+item.Name.ToString(), item.GetValue(data,null));
                }
                Query = Query.TrimEnd(',');
                Query += ")";
                cmd.CommandText = Query;
                foreach (PropertyInfo item in properties)
                {
                    cmd.Parameters.AddWithValue("@" + item.Name, item.GetValue(data,null));
                }
                await con.OpenAsync();
                MySqlDataReader reader =(MySqlDataReader)await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var y = (T)Activator.CreateInstance(typeof(T));
                    foreach (PropertyInfo item in propertiesGeneric)
                    {
                        item.SetValue(y, reader[item.Name.Replace("_"," ")] == DBNull.Value ? "" : reader[item.Name.Replace("_"," ")], null);
                    }
                    result.Add(y);
                }
                reader.Close();
                con.Close();
                //using (var adapter = new MySqlDataAdapter(Query, con))
                //{
                //    DataTable dt = new DataTable();
                //    await adapter.FillAsync(dt);
                //    int index = 0;
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        var y = (T)Activator.CreateInstance(typeof(T));
                //        if (index<dt.Columns.Count)
                //        {
                //            foreach (PropertyInfo item in propertiesGeneric)
                //            {
                //                item.SetValue(y, dt.Rows[i].ItemArray[index], null);
                //                index++;
                //            }
                //            result.Add(y);
                //        }
                //        else
                //        {
                //            index = 0;
                //            i--;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return result;
        }
    }
}
