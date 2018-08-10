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
        /// <summary>
        /// Stored Procedure Kullanarak Kullanıcıya DataTable Tipinde Veri Döndürür
        /// Gönderilen Obje Adı ile Procedure Adı Aynı Olmak Zorundadır.
        /// </summary>
        Task<DataTable> GetDataWithStoredProcedure(object stored_procedure); 
        /// <summary>
        /// Stored Procedure Kullanarak Kullanıcıya Generic List Tipinde Veri Döndürür.
        /// Model Gereklidir
        /// </summary>
        Task<List<T>> GetDataWithStoredProcedure<T>(object stored_procedure); 
        /// <summary>
        /// Stored Procedure İşlenir. Kullanıcıya İşlenip İşlenemediği Bilgisi Döndürülür.
        /// </summary>
        Task<bool> ExecuteStoredProcedure(object stored_procedure);
        /// <summary>
        /// Kullanıcıya Gönderdiği Sorgunun Cevabı Olarak Bir DataTable Döndürür.
        /// </summary>
        Task<DataTable> SelectQuery(string query);
        /// <summary>
        /// Kullanıcının Gönderdiği Sorguya Göre DataTable Döndürür.
        /// Gönderilen Parametre Adları İle Object Adları Aynı Olmalıdır.
        /// </summary>
        Task<DataTable> SelectQuery(string query, object param);
        /// <summary>
        /// Kullanıcının Gönderdiği Sorguya Göre Generic List Döndürür.
        /// Gönderilen Parametre Adları İle Object Adları Aynı Olmak Zorundadır.
        /// </summary>
        Task<List<T>> SelectQuery<T>(string query);
        /// <summary>
        /// Kullanıcının Gönderdiği Sorguya Göre Generic List Döndürür.
        /// Gönderilen Parametre Adları İle Object Adları Aynı Olmak Zorundadır.
        /// </summary>
        Task<List<T>> SelectQuery<T>(string query, object param);
        /// <summary>
        /// Gönderilen Sorgunun İşlenip İşlenemediği Bilgisi Döndürülür.
        /// </summary>
        Task<bool> ExecuteQuery(string query);
        /// <summary>
        /// Gönderilen Sorgunun İşlenip İşlenemediği Bilgisini Döndürür.
        /// </summary>
        Task<bool> ExecuteQuery(string query, object param);
        /// <summary>
        /// Gönderilen Sorguyu Insert Eder ve Geriye İşlemin Sonucunu Döndürür.
        /// </summary>
        Task<bool> Insert(string query,object param); 
        /// <summary>
        /// Gönderilen Model İle Veritabanı Tablo Adı Aynı Olan Tabloya Veriyi Insert Eder.
        /// </summary>
        Task<bool> Insert(object model);
        /// <summary>
        /// Gönderilen Obje Adındaki Tabloya Toplu Insert Yapar
        /// </summary>
        Task<bool> BulkInsert<T>(List<T> param);
        Task<List<dynamic>> SelectDynamic(string query, object param);

        Task<List<dynamic>> StoredProcedureDynamic(object procedure);
    }
}
