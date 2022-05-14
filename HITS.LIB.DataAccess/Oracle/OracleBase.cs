using HITS.Extensions.Object;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace HITS.LIB.DataAccess
{
    /// <summary>
    /// This class in inherited by OracleAsync and OracleSync
    /// </summary>
    public abstract class OracleBase : IDisposable
    {
        /// <summary>
        /// This property will contain the last executed SQL statement.
        /// </summary>
        public string LastSqlStatement { get; set; }

        internal const int QUERY_TIMEOUT = 30;

        internal void CaptureSqlStatement(string sql, OracleParameter[] parameters)
        {
            try
            {
                string message = sql.Contains(' ') ? $"EXEC {sql} " : $"{sql} ";
                if (parameters != null)
                {
                    foreach (OracleParameter parameter in parameters)
                    {
                        message += $@"@{parameter.ParameterName}='{parameter.Value.PrepValue().AsString()}',";
                    }
                }
                LastSqlStatement = message.TrimEnd(',') + ";";
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// This method will return an Oracle reference cursor parameter object.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <returns></returns>
        public OracleParameter GetRefCursorParameter(string storedProcedureName)
        {
            return new OracleParameter(storedProcedureName, OracleDbType.RefCursor, ParameterDirection.Output);
        }

        /// <summary>
        /// This method tests the connection to the database.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>true on success</returns>
        public string ConnectionTest(string connectionString)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand cmd = new OracleCommand("select SYSDATE mydate from dual", connection) { CommandType = CommandType.Text })
                    {
                        connection.Open();
                        object dateTime = cmd.ExecuteScalar();
                        return dateTime.AsString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DataAccessBase() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
