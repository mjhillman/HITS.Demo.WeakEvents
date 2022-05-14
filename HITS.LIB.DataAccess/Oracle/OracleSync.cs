using HITS.Extensions.Object;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace HITS.LIB.DataAccess
{
    /// <summary>
    /// This class contains data access methods for the Oracle Client
    /// </summary>
    /// <remarks>Be sure to wraps call to this class in a try-catch block and handle the errors appropriately.</remarks>
    public class OracleSync : OracleBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OracleSync()
        {
            OracleConfiguration.AddOracleTypesDeserialization();
        }

        /// <summary>
        /// This method will retrieve data from the database as a DataSet.
        /// </summary>
        /// <param name="connectionString">the connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">array of OracleParameter objects</param>
        /// <param name="timeout">query timeout value in seconds</param>
        /// <returns>DataSet object</returns>
        internal protected DataSet GetDataSet(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT)
        {
            DataSet ds = null;

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

                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        //da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        ds = new DataSet();
                        da.Fill(ds);
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
        internal protected DataTable GetDataTable(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT)
        {
            DataTable dt = null; ;

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

                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        //da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        dt = new DataTable();
                        da.Fill(dt);
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
        internal protected List<T> GetDataTableAsList<T>(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT) where T : new()
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
        /// <param name="connectionString">the connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">array of OracleParameter objects</param>
        /// <param name="timeout">query timeout value in seconds</param>
        /// <returns>List</returns>
        internal protected T GetDataRecord<T>(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT) where T : new()
        {
            DataTable dt = null;
            try
            {
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
        /// <param name="connectionString">the connection string</param>
        /// <param name="sql">the stored procedure name or SQL statement</param>
        /// <param name="parameters">array of OracleParameter objects</param>
        /// <param name="timeout">query timeout value in seconds</param>
        internal protected int ExecuteNonQuery(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = 30)
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

                    connection.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
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
        internal protected object ExecuteScalar(string connectionString, string sql, OracleParameter[] parameters = null, int timeout = QUERY_TIMEOUT)
        {
            object result = null;

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

                    connection.Open();
                    result = cmd.ExecuteScalar();
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