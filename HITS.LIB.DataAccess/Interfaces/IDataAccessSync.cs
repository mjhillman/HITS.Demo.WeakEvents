using System.Data;

namespace HITS.LIB.DataAccess
{
    public interface IDataAccessSync
    {
        int ExecuteNonQuery(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = 20);
        object ExecuteScalar(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = 20);
        T GetDataRecord<T>(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = 20) where T : new();
        DataTable GetDataTable(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = 20);
        List<T> GetDataTableAsList<T>(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = 20) where T : new();
        string ConnectionTest(string connectionString);
    }
}