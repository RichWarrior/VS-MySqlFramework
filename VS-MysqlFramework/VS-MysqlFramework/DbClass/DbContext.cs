using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Reflection;
using System.Threading.Tasks;
using VS_MysqlFramework.IDbClass;

namespace VS_MysqlFramework.DbClass
{
    public class DbContext : IDbContext
    {

        private string connectionString { get; set; }
        /// <summary>
        /// MySql Framework
        /// </summary>
        /// <param name="connectionString">Veritabanı Bağlantı Anahtarı</param>
        public DbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// Toplu Insert Yapımı İçin Veritabanı Modeli Oluşturulur ve İçerisine Gönderilen Parametreye Göre Toplu Insert Yapar
        /// Test Edildi!
        /// </summary>
        public async Task<bool> BulkInsert<T>(List<T> param)
        {
            var result = false;
            var table_name = typeof(T).Name;
            var query = "INSERT INTO " + table_name + " (";
            var subQuery = "";
            var paramQuery = "";
            IList<PropertyInfo> GenericProperties = new List<PropertyInfo>(typeof(T).GetProperties());
            foreach (PropertyInfo item in GenericProperties)
            {
                subQuery += item.Name + ",";
            }
            subQuery = subQuery.TrimEnd(',') + ") VALUES";
            query = query + subQuery;
            subQuery = "";
            var index = 0;
            using (var con = new MySqlConnection(this.connectionString))
            {
                try
                {
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        foreach (var item in param)
                        {
                            IList<PropertyInfo> props = new List<PropertyInfo>(item.GetType().GetProperties());
                            paramQuery = "(";
                            foreach (PropertyInfo sub in props)
                            {
                                paramQuery += "@" + sub.Name + "_" + index.ToString() + ",";
                                cmd.Parameters.AddWithValue("@" + sub.Name + "_" + index.ToString(), sub.GetValue(item, null));
                                index++;
                            }
                            query = query + paramQuery.TrimEnd(',') + "),";
                        }
                        paramQuery = paramQuery.TrimEnd(',') + "";
                        query = query.TrimEnd(',');
                        cmd.CommandText = query;
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        result = true;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        await con.CloseAsync();
                }
            }
            return result;
        }
        /// <summary>
        /// Verilen Sorgu ve Parametrelere Göre Sorgu Çalışır.
        /// Test Edildi!
        /// </summary>
        public async Task<bool> ExecuteQuery(string query)
        {
            Task<bool> task = Task.Run(() => this.ExecuteQuery(query, new { }));
            return task.Result;
        }
        /// <summary>
        /// Verilen Sorgu ve Parametrelere Göre Sorgu Çalışır.
        /// Sorguda Bulunan Parametre Adları İle Gönderilen Parametre Adları Aynı Olmak Zorundadır.
        /// Test Edildi!
        /// </summary>
        public async Task<bool> ExecuteQuery(string query, object param)
        {
            var result = false;
            using (var con = new MySqlConnection(this.connectionString))
            {
                try
                {
                    IList<PropertyInfo> properties = new List<PropertyInfo>(param.GetType().GetProperties());

                    using (var cmd = new MySqlCommand() { Connection = con, CommandText = query, CommandType = CommandType.Text })
                    {
                        foreach (PropertyInfo item in properties)
                        {
                            cmd.Parameters.AddWithValue("@" + item.Name, item.GetValue(param, null));
                        }

                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        result = true;
                    }

                }
                catch (Exception) { }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        await con.CloseAsync();
                }
            }
            return result;
        }
        /// <summary>
        /// Gönderilen Değer İle Veritabanında Aynı İsimde Olan Procedure Çalışır Ve Geriye Çalışıp Çalışmadığı Bilgisi Döner
        /// Test Edildi!
        /// <summary>
        public async Task<bool> ExecuteStoredProcedure(object stored_procedure)
        {
            var result = false;
            using (var con = new MySqlConnection(this.connectionString))
            {
                try
                {
                    var query = "CALL " + stored_procedure.GetType().Name + "(";
                    IList<PropertyInfo> properties = new List<PropertyInfo>(stored_procedure.GetType().GetProperties());
                    foreach (PropertyInfo item in properties)
                    {
                        Type type = item.GetValue(stored_procedure, null).GetType();
                        if (type == typeof(string))
                            query += "'" + item.GetValue(stored_procedure, null) + "',";
                        else
                            query += item.GetValue(stored_procedure, null) + ",";
                    }
                    query = query.TrimEnd(',') + ")";

                    using (var cmd = new MySqlCommand() { Connection = con, CommandText = query, CommandType = CommandType.Text })
                    {
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        result = true;
                    }

                }
                catch (Exception) { }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        await con.CloseAsync();
                }
            }
            return result;
        }
        /// <summary>
        /// Store Procedure Kullanarak Verileri DataTable Tipinde Almak İçin Kullanılır.
        /// Gönderdilen object adı ile procedure adı aynı olmak zorundadır.
        /// Procedure alması gereken parametreler gönderilen objenin içinde olmalıdır.
        /// Test Edildi!
        /// </summary>
        public async Task<DataTable> GetDataWithStoredProcedure(object data)
        {
            var result = new DataTable();
            string query = "CALL " + data.GetType().Name + "(";
            IList<PropertyInfo> properties = new List<PropertyInfo>(data.GetType().GetProperties());
            foreach (PropertyInfo item in properties)
            {
                Type type = item.GetValue(data, null).GetType();
                if (type == typeof(string))
                    query += "'" + item.GetValue(data, null) + "',";
                else
                    query += item.GetValue(data, null) + ",";

            }
            query = query.TrimEnd(',') + ")";
            using (var con = new MySqlConnection(this.connectionString))
            {
                using (var adapter = new MySqlDataAdapter(query, con))
                {
                    try
                    {
                        await adapter.FillAsync(result);
                    }
                    catch (Exception) { }
                }
            }
            return result;
        }
        /// <summary>
        /// Store Procedure Kullanarak Verileri Generic List Tipinde Almak İçin Kullanılır.
        /// Gönderdilen object adı ile procedure adı aynı olmak zorundadır.
        /// Procedure alması gereken parametreler gönderilen objenin içinde olmalıdır.
        /// Test Edildi!
        /// </summary>
        public async Task<List<T>> GetDataWithStoredProcedure<T>(object data)
        {
            var result = new List<T>();
            string query = "CALL " + data.GetType().Name + "(";
            IList<PropertyInfo> properties = new List<PropertyInfo>(data.GetType().GetProperties());
            IList<PropertyInfo> main_properties = new List<PropertyInfo>(typeof(T).GetProperties());
            MySqlDataReader reader = null;
            using (var con = new MySqlConnection(this.connectionString))
            {
                try
                {

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        foreach (PropertyInfo item in properties)
                        {
                            query += "@" + item.Name + ",";
                        }
                        query = query.TrimEnd(',') + ")";
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        foreach (PropertyInfo item in properties)
                        {
                            cmd.Parameters.AddWithValue("@" + item.Name, item.GetValue(data, null));
                        }
                        using (reader)
                        {
                            await con.OpenAsync();
                            reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                            while (await reader.ReadAsync())
                            {
                                var y = (T)Activator.CreateInstance(typeof(T));
                                foreach (PropertyInfo item in main_properties)
                                {
                                    var col_name = item.Name.Replace("_", " ");
                                    item.SetValue(y, reader[col_name] == DBNull.Value ? null : reader[col_name]);
                                }
                                result.Add(y);
                            }
                        }
                    }

                }
                catch (Exception) { }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        await con.CloseAsync();
                    if (reader != null)
                        reader.Close();
                }
            }
            return result;
        }
        /// <summary>
        /// Yazdığınız Sorguyu İşler ve Size Cevap Verir.
        /// Sorguda Kullanıdğınız Parametre Adı ile Gönderdiğiniz Parametre Adları Aynı Olmalıdır.
        /// Test Edildi!
        /// </summary>        
        public async Task<bool> Insert(string query, object param)
        {
            var result = false;
            using (var con = new MySqlConnection(this.connectionString))
            {
                try
                {
                    IList<PropertyInfo> properties = new List<PropertyInfo>(param.GetType().GetProperties());

                    using (var cmd = new MySqlCommand() { CommandText = query, Connection = con, CommandType = CommandType.Text })
                    {
                        foreach (PropertyInfo item in properties)
                        {
                            cmd.Parameters.AddWithValue("@" + item.Name, item.GetValue(param, null));
                        }
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        result = true;
                    }

                }
                catch (Exception) { }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        await con.CloseAsync();
                }
            }
            return result;
        }
        /// <summary>
        /// Bir Model Göndererek Veritabanına Tekli Insert İşlemleri İçin Kullanılır.
        /// Model İsmi İle Tablo İsmi Aynı Olmak Zorundadır.
        /// Test Edildi!
        /// </summary>          
        public async Task<bool> Insert(object model)
        {
            var result = false;
            using (var con = new MySqlConnection(this.connectionString))
            {
                try
                {
                    var main_query = "INSERT INTO " + model.GetType().Name + "(";
                    var sub_query = "(";
                    IList<PropertyInfo> properties = new List<PropertyInfo>(model.GetType().GetProperties());
                    foreach (PropertyInfo item in properties)
                    {
                        main_query += item.Name + ",";
                        sub_query += "@" + item.Name + ",";
                    }
                    main_query = main_query.TrimEnd(',') + ")";
                    sub_query = sub_query.TrimEnd(',') + ")";
                    main_query = main_query + " VALUES " + sub_query;

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandText = main_query;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        foreach (PropertyInfo item in properties)
                        {
                            cmd.Parameters.AddWithValue("@" + item.Name, item.GetValue(model, null));
                        }
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        result = true;
                    }

                }
                catch (Exception ex) { }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        await con.CloseAsync();

                }
            }
            return result;
        }
        /// <summary>
        /// Seçilen Sorguya Göre Kullanıcıya DataTable Döndürür.
        /// Test Edildi!
        /// </summary>
        public async Task<DataTable> SelectQuery(string query)
        {
            var result = new DataTable();
            try
            {
                using (var con = new MySqlConnection(this.connectionString))
                {
                    using (var adapter = new MySqlDataAdapter(query, con))
                    {
                        await adapter.FillAsync(result);
                    }
                }
            }
            catch (Exception) { }
            return result;
        }
        /// <summary>
        /// Gönderilen Sorguya ve Parametrelere Göre Verileri Çeker.
        /// Test Edildi!
        /// </summary>
        public async Task<List<T>> SelectQuery<T>(string query)
        {
            Task<List<T>> task = Task.Run(() => this.SelectQuery<T>(query, new { }));
            return task.Result;
        }
        /// <summary>
        /// Gönderilen Sorguya ve Parametrelere Göre Verileri Çeker.
        /// Generic Sınıf Veritabanından Gelecek Verilerin Aynısı Olmak Zorundadır.
        /// Parametre Adı ve Gönderilen Parametre Adları Aynı Olmak Zorundadır.
        /// Test Edildi!
        /// </summary>
        public async Task<List<T>> SelectQuery<T>(string query, object param)
        {
            var result = new List<T>();
            MySqlDataReader reader = null;
            using (var con = new MySqlConnection(this.connectionString))
            {
                try
                {

                    using (var cmd = new MySqlCommand() { CommandText = query, Connection = con, CommandType = CommandType.Text })
                    {
                        IList<PropertyInfo> properties = new List<PropertyInfo>(param.GetType().GetProperties());
                        IList<PropertyInfo> sub_properties = new List<PropertyInfo>(typeof(T).GetProperties());
                        foreach (PropertyInfo item in properties)
                        {
                            cmd.Parameters.AddWithValue("@" + item.Name, item.GetValue(param, null));
                        }
                        await con.OpenAsync();
                        reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            var y = (T)Activator.CreateInstance(typeof(T));
                            foreach (PropertyInfo item in sub_properties)
                            {
                                var column_name = item.Name.Replace("_", " ");
                                item.SetValue(y, reader[column_name] == DBNull.Value ? null : reader[column_name]);
                            }
                            result.Add(y);
                        }
                    }

                }
                catch (Exception) { }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        await con.CloseAsync();
                    if (reader != null)
                        reader.Close();
                }
            }
            return result;
        }
        /// <summary>
        /// Gönderilen Sorgu ve Parametrelere Göre DataTable Tipinde Veri Döndürür.
        /// Test Edildi!
        /// </summary>
        public async Task<DataTable> SelectQuery(string query, object param)
        {
            var result = new DataTable();
            try
            {
                using (var con = new MySqlConnection(this.connectionString))
                {
                    using (var adapter = new MySqlDataAdapter(query, con))
                    {
                        IList<PropertyInfo> properties = new List<PropertyInfo>(param.GetType().GetProperties());
                        foreach (PropertyInfo item in properties)
                        {
                            adapter.SelectCommand.Parameters.AddWithValue("@" + item.Name, item.GetValue(param, null));
                        }
                        await adapter.FillAsync(result);
                    }
                }
            }
            catch (Exception) { }
            return result;
        }
        public async Task<List<dynamic>> SelectDynamic(string query, object param)
        {
            var result = new List<dynamic>();
            var con = new MySqlConnection(connectionString);
            MySqlDataReader reader = null;
            try
            {
                var myProperties = new List<PropertyInfo>(param.GetType().GetProperties());
                using (var cmd = new MySqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection = con;
                    foreach (PropertyInfo item in myProperties)
                    {
                        if (item.GetValue(param, null) != null)
                        {
                            if (query.Contains("@" + item.Name))
                            {
                                cmd.Parameters.Add(new MySqlParameter("@" + item.Name, item.GetValue(param, null)));
                            }

                        }

                    }
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        dynamic y = new ExpandoObject();
                        var dictionary = (IDictionary<string, object>)y;
                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            dictionary.Add(reader.GetName(i), reader[i] == DBNull.Value ? null : reader[i]);

                        }
                        result.Add(dictionary);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (con.State == ConnectionState.Open)
                    await con.CloseAsync();
            }
            return result;
        }
        /// Gönderilen Procedure Modeli Olmadan Geriye Döndürür.

        public async Task<List<dynamic>> StoredProcedureDynamic(object procedure)
        {
            var query = "CALL " + procedure.GetType().Name + " (";
            IList<PropertyInfo> properties = new List<PropertyInfo>(procedure.GetType().GetProperties());
            foreach (PropertyInfo item in properties)
            {
                query += "@" + item.Name;
            }
            return this.SelectDynamic(query, procedure).Result;
        }
    }
}
