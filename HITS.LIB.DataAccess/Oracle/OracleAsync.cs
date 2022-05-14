using HITS.Extensions.Object;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace HITS.LIB.DataAccess
{
    /// <summary>
    /// This class contains data access methods for the SQL Client
    /// </summary>
    /// <remarks>Be sure to wraps call to this class in a try-catch block and handle the errors appropriately.</remarks>
    public class OracleAsync : OracleBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OracleAsync()
        {
            OracleConfiguration.AddOracleTypesDeserialization();
        }

        /// <summary>
        /// This method will retrieve data from the database as a DataSet.
        /// </summary>
        /// <param name="connectionString">the connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">optional: the parameter array</param>
        /// <param name="timeout">query timeout value in secoonds</param>
        /// <returns>DataSet object</returns>
        internal protected async Task<DataSet> GetDataSetAsync(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT)
        {
            DataSet ds = null;

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(sql, connection))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = sql.Contains(' ') ? CommandType.Text : CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (OracleParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    CaptureSqlStatement(sql, parameters);

                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
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
        /// <param name="connectionString">the connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">array of OracleParameter objects</param>
        /// <param name="timeout">query timeout value in seconds</param>
        /// <returns>DataTable object</returns>
        internal protected async Task<DataTable> GetDataTableAsync(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT)
        {
            DataTable dt = null;

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(sql, connection))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = sql.Contains(' ') ? CommandType.Text : CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (OracleParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    
                    CaptureSqlStatement(sql, null);

                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
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
        /// This method will retrieve data from the database as a List.
        /// </summary>
        /// <typeparam name="T">your data type that corresponds to the database data</typeparam>
        /// <param name="connectionString">the connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">array of OracleParameter objects</param>
        /// <param name="timeout">query timeout value in seconds</param>
        /// <returns>List</returns>
        internal protected async Task<List<T>> GetDataTableAsListAsync<T>(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT) where T : new()
        {
            DataTable dt = null;
            try
            {
                dt = await GetDataTableAsync(connectionString, sql, parameters, timeout).ConfigureAwait(false);
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
        /// <param name="connectionString">the connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">array of OracleParameter objects</param>
        /// <param name="timeout"></param>
        /// <returns>List</returns>
        internal protected async Task<T> GetDataRecordAsync<T>(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT) where T : new()
        {
            DataTable dt = null;
            try
            {
                dt = await GetDataTableAsync(connectionString, sql, parameters, timeout);
                if (!dt.IsEmpty())
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
        /// <param name="connectionString">the connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">array of OracleParameter objects</param>
        /// <param name="timeout">query timeout value in seconds</param>
        /// <returns>the number of rows affected</returns>
        internal protected async Task<int> ExecuteNonQueryAsync(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT)
        {
            int rowsAffected = 0;

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(sql, connection) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = sql.Contains(' ') ? CommandType.Text : CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (OracleParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
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
        /// This method will execute a stored procedure that returns a single value.
        /// </summary>
        /// <param name="connectionString">the connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">array of OracleParameter objects</param>
        /// <param name="timeout">query timeout value in seconds</param>
        /// <returns>an object value</returns>
        internal protected async Task<object> ExecuteScalarAsync(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT)
        {
            object result = null;

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(sql, connection))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = sql.Contains(' ') ? CommandType.Text : CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (OracleParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    CaptureSqlStatement(sql, parameters);

                    await connection.OpenAsync();
                    result = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                    if (result.IsNull())
                    {
                        foreach (OracleParameter parameter in parameters)
                        {
                            if (parameter.Direction == ParameterDirection.Output)
                            {
                                result = parameter.Value;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

    }

}