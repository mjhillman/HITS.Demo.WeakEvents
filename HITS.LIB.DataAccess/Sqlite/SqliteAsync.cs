using Microsoft.Data.Sqlite;
using System.Data;

namespace HITS.LIB.DataAccess
{
    public class SqliteAsync : SqliteBase, IDataAccessAsync
    {
        /// <summary>
        /// This method will retrieve data from the database as a DataTable.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout">query timeout in seconds</param>
        /// <returns>DataTable object</returns>
        public async Task<DataTable> GetDataTableAsync(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT)
        {
            DataTable dt = null;
            SqliteConnection connection = null;

            try
            {
                connection = new SqliteConnection(connectionString);
                await connection?.OpenAsync();
                using (SqliteCommand cmd = new SqliteCommand(sql, connection))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = CommandType.Text;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in parameters)
                        {
                            cmd.Parameters.Add(GetParameter(kvp.Key, PrepValue(kvp.Value)));
                        }
                    }

                    CaptureSqlStatement(sql, parameters);

                    using (SqliteDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt = new DataTable();
                        dt.Load(reader, LoadOption.OverwriteChanges);
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    if (connection?.State != ConnectionState.Closed) await connection?.CloseAsync();
                }
            }
        }

        /// <summary>
        /// This method will retrieve data from the database as a List.
        /// </summary>
        /// <typeparam name="T">your data type that corresponds to the database data</typeparam>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout">query timeout in seconds</param>
        /// <returns>List</returns>
        public async Task<List<T>> GetDataTableAsListAsync<T>(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT) where T : new()
        {
            DataTable dt = null;

            try
            {
                dt = await GetDataTableAsync(connectionString, sql, parameters, timeout);
                return dt == null ? new List<T>() : dt.Copy().ToList<T>();
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
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Copy().ToList<T>()[0];
                }
                return new T();
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
        /// This method will execute a SQL Stored Procedure.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout">query timeout in seconds</param>
        /// <returns>the number of rows affected</returns>
        public async Task<int> ExecuteNonQueryAsync(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT)
        {
            int rowsAffected = 0;
            SqliteConnection connection = null;

            try
            {
                connection = new SqliteConnection(connectionString);
                await connection.OpenAsync();
                using (SqliteCommand cmd = new SqliteCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = timeout;
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in parameters)
                        {
                            cmd.Parameters.Add(GetParameter(kvp.Key, PrepValue(kvp.Value)));
                        }
                    }

                    CaptureSqlStatement(sql, parameters);

                    rowsAffected = await cmd?.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
                return rowsAffected;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    if (connection?.State != ConnectionState.Closed) await connection?.CloseAsync();
                }
            }
        }

        /// <summary>
        /// This method will execute a stored procedure that returns a single value.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout">query timeout in seconds</param>
        /// <returns>an object value</returns>
        public async Task<object> ExecuteScalarAsync(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT)
        {
            SqliteConnection connection = null;

            try
            {
                connection = new SqliteConnection(connectionString);
                await connection.OpenAsync();
                using (SqliteCommand cmd = new SqliteCommand(sql, connection))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = sql.Contains(' ') ? CommandType.Text : CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in parameters)
                        {
                            cmd.Parameters.Add(GetParameter(kvp.Key, PrepValue(kvp.Value)));
                        }
                    }

                    CaptureSqlStatement(sql, parameters);

                    return await cmd?.ExecuteScalarAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    if (connection?.State != ConnectionState.Closed) await connection?.CloseAsync();
                }
            }
        }
    }
}
