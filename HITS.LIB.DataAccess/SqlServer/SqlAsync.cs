using System.Data;
using System.Data.SqlClient;

namespace HITS.LIB.DataAccess
{
    /// <summary>
    /// This class contains asynchronous data access methods for the SQL Client
    /// </summary>
    /// <remarks>Be sure to wraps call to this class in a try-catch block and handle the errors appropriately.</remarks>
    public class SqlAsync : SqlBase, IDataAccessAsync
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlAsync()
        {
        }

        /// <summary>
        /// This method will retrieve data from the database as a DataSet.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout"></param>
        /// <returns>DataSet object</returns>
        public async Task<DataSet> GetDataSetAsync(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT)
        {
            DataSet ds = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = sql.Contains(' ') ? CommandType.Text : CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in parameters)
                        {
                            cmd.Parameters.Add(GetParameter(kvp.Key, kvp.Value));
                        }
                    }

                    CaptureSqlStatement(sql, parameters);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        //da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        ds = new DataSet();
                        await Task.Run(() => { da.Fill(ds); }).ConfigureAwait(false);
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// This method will retrieve data from the database as a DataTable.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout"></param>
        /// <returns>DataTable object</returns>
        public async Task<DataTable> GetDataTableAsync(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT)
        {
            DataTable dt = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = sql.Contains(' ') ? CommandType.Text : CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in parameters)
                        {
                            cmd.Parameters.Add(GetParameter(kvp.Key, kvp.Value));
                        }
                    }

                    CaptureSqlStatement(sql, parameters);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        //da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        dt = new DataTable();
                        await Task.Run(() => { da.Fill(dt); }).ConfigureAwait(false);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// This method will retrieve data from the database as a typed record.
        /// </summary>
        /// <typeparam name="T">your data type that corresponds to the database data</typeparam>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout">query timeout in seconds</param>
        /// <returns>List</returns>
        public async Task<T> GetDataRecordAsync<T>(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT) where T : new()
        {
            DataTable dt = null;
            try
            {
                dt = await GetDataTableAsync(connectionString, sql, parameters, timeout);
                return dt.IsEmpty() ? new T() : dt.Copy().ToList<T>()[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dt?.Dispose();
            }
        }

        /// <summary>
        /// This method will retrieve data from the database as a List.
        /// </summary>
        /// <typeparam name="T">your data type that corresponds to the database data</typeparam>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout"></param>
        /// <returns>List</returns>
        public async Task<List<T>> GetDataTableAsListAsync<T>(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT) where T : new()
        {
            DataTable dt = await GetDataTableAsync(connectionString, sql, parameters, timeout).ConfigureAwait(false);
            return dt.ToList<T>();
        }

        /// <summary>
        /// This method will execute a SQL statement.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout"></param>
        /// <returns>number of rows affected</returns>
        public async Task<int> ExecuteNonQueryAsync(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = sql.Contains(' ') ? CommandType.Text : CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in parameters)
                        {
                            cmd.Parameters.Add(GetParameter(kvp.Key, kvp.Value));
                        }
                    }

                    CaptureSqlStatement(sql, parameters);

                    await connection.OpenAsync();
                    rowsAffected = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// This method will execute a scalar statement that returns a single value.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout"></param>
        /// <returns>an object value</returns>
        public async Task<object> ExecuteScalarAsync(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = sql.Contains(' ') ? CommandType.Text : CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in parameters)
                        {
                            cmd.Parameters.Add(GetParameter(kvp.Key, kvp.Value));
                        }
                    }

                    CaptureSqlStatement(sql, parameters);

                    await connection.OpenAsync();

                    return await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                }
            }
        }
    }
}