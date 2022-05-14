using Microsoft.Data.Sqlite;
using System.Data;

namespace HITS.LIB.DataAccess
{
    public class SqliteBase : IDisposable
    {
        /// <summary>
        /// Last SQL Statement Executed
        /// </summary>
        public string LastSqlStatement { get; set; }

        public bool CaptureLastSqlStatement { get; set; } = false;

        internal const int QUERY_TIMEOUT = 20;

        public static object PrepValue(object value)
        {
            if (value is string)
            {
                return Convert.ToString(value).Replace("'", "''");
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// This method builds the SQL statement that has been executed for logging purposes.
        /// </summary>
        /// <param name="sql">the SQL text</param>
        /// <param name="parameters">the parameter dictionary</param>
        internal void CaptureSqlStatement(string sql, IDictionary<string, object> parameters)
        {
            if (!CaptureLastSqlStatement) return;

            try
            {
                string message = string.Empty;
                if (parameters != null && parameters.Count > 0)
                {
                    message = $"{sql} ";
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        if (sql.Contains($"{parameter.Key}"))
                        {
                            sql = sql.Replace($@"(${parameter.Key})", $@"'{Convert.ToString(parameter.Value)}'");
                        }
                    }
                    LastSqlStatement = sql;
                }
                else
                {
                    LastSqlStatement = sql;
                }
            }
            catch (Exception)
            {
            }
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
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    using (SqliteCommand cmd = new SqliteCommand("SELECT GETDATE();", connection) { CommandType = CommandType.Text })
                    {
                        connection.Open();
                        object dateTime = cmd.ExecuteScalar();
                    }
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal SqliteParameter GetParameter(string name, object value)
        {
            SqliteParameter sqlParameter = new SqliteParameter();
            sqlParameter.ParameterName = name;

            if (value.GetType() == typeof(double))
            {
                sqlParameter.SqliteType = SqliteType.Real;
                sqlParameter.Value = value;
            }
            else if (value.GetType() == typeof(decimal))
            {
                sqlParameter.SqliteType = SqliteType.Text;
                sqlParameter.Value = value;
            }
            else if (value.GetType() == typeof(Single))
            {
                sqlParameter.SqliteType = SqliteType.Real;
                sqlParameter.Value = value;
            }
            else if (value.GetType() == typeof(Byte[]))
            {
                sqlParameter.SqliteType = SqliteType.Blob;
                sqlParameter.Value = value;
            }
            else if (value.GetType() == typeof(Int64))
            {
                sqlParameter.SqliteType = SqliteType.Integer;
                sqlParameter.Value = Convert.ToInt64(value);
            }
            else if (value.GetType() == typeof(Int32))
            {
                sqlParameter.SqliteType = SqliteType.Integer;
                sqlParameter.Value = Convert.ToInt64(value);
            }
            else if (value.GetType() == typeof(long))
            {
                sqlParameter.SqliteType = SqliteType.Integer;
                sqlParameter.Value = value;
            }
            else if (value.GetType() == typeof(int))
            {
                sqlParameter.SqliteType = SqliteType.Integer;
                sqlParameter.Value = value;
            }
            else if (value.GetType() == typeof(bool))
            {
                sqlParameter.SqliteType = SqliteType.Integer;
                sqlParameter.Value = value;
            }
            else if (value.GetType() == typeof(DateTime))
            {
                sqlParameter.SqliteType = SqliteType.Text;
                sqlParameter.Value = Convert.ToDateTime(value);
            }
            else if (value.GetType() == typeof(DateTimeOffset))
            {
                sqlParameter.SqliteType = SqliteType.Text;
                sqlParameter.Value = Convert.ToString(value);
            }
            else if (value.GetType() == typeof(TimeSpan))
            {
                sqlParameter.SqliteType = SqliteType.Text;
                sqlParameter.Value = Convert.ToString(value);
            }
            else if (value.GetType() == typeof(Guid))
            {
                sqlParameter.SqliteType = SqliteType.Text;
                sqlParameter.Value = Convert.ToString(value);
            }
            else if (value.GetType() == typeof(Char))
            {
                sqlParameter.SqliteType = SqliteType.Text;
                sqlParameter.Value = Convert.ToString(value);
            }
            else
            {
                sqlParameter.SqliteType = SqliteType.Text;
                sqlParameter.Value = Convert.ToString(value);
            }

            return sqlParameter;
        }

        #region dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SqlBase()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion dispose
    }
}
