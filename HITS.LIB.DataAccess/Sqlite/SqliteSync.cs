using Microsoft.Data.Sqlite;
using System.Data;

namespace HITS.LIB.DataAccess
{
    public class SqliteSync : SqliteBase, IDataAccessSync
    {
        /// <summary>
        /// This method will retrieve data from the database as a DataTable.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter dictionary</param>
        /// <param name="timeout">query timeout in seconds</param>
        /// <returns>DataTable object</returns>
        public DataTable GetDataTable(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT)
        {
            DataTable dt = null;
            SqliteConnection connection = null;

            try
            {
                connection = new SqliteConnection(connectionString);
                connection.Open();
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

                    using (SqliteDataReader reader = cmd.ExecuteReader())
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
                    if (connection?.State != ConnectionState.Closed) connection?.Close();
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
        public List<T> GetDataTableAsList<T>(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT) where T : new()
        {
            DataTable dt = null;

            try
            {
                dt = GetDataTable(connectionString, sql, parameters, timeout);
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
        public T GetDataRecord<T>(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT) where T : new()
        {
            DataTable dt = null;

            try
            {
                CaptureSqlStatement(sql, parameters);
                dt = GetDataTable(connectionString, sql, parameters, timeout);
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
        public int ExecuteNonQuery(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT)
        {
            int rowsAffected = 0;
            SqliteConnection connection = null;

            try
            {
                connection = new SqliteConnection(connectionString);
                connection.Open();
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

                    rowsAffected = cmd.ExecuteNonQuery();
                }
                connection.Close();
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
                    if (connection?.State != ConnectionState.Closed) connection?.Close();
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
        public object ExecuteScalar(string connectionString, string sql, IDictionary<string, object> parameters = null, int timeout = QUERY_TIMEOUT)
        {
            SqliteConnection connection = null;

            try
            {
                connection = new SqliteConnection(connectionString);
                connection.Open();
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

                    return cmd.ExecuteScalar();
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
                    if (connection?.State != ConnectionState.Closed) connection?.Close();
                }
            }
        }
    }
}
