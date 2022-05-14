using System.Data;

namespace HITS.LIB.DataAccess
{
    public interface IDataAccessAsync
    {
        Task<int> ExecuteNonQueryAsync(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = 20);
        Task<object> ExecuteScalarAsync(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = 20);
        Task<T> GetDataRecordAsync<T>(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = 20) where T : new();
        Task<List<T>> GetDataTableAsListAsync<T>(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = 20) where T : new();
        Task<DataTable> GetDataTableAsync(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = 20);
        string ConnectionTest(string connectionString);
    }
}