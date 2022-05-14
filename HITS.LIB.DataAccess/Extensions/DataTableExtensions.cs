using System.Data;
using System.Reflection;

namespace HITS.LIB.DataAccess
{
    /// <summary>
    /// This class contains an extension to convert a DataTable to a List of T
    /// </summary>
    public static class DataTableExtensions
    {
        private static Dictionary<Type, List<PropertyInfo>> typeDictionary = new Dictionary<Type, List<PropertyInfo>>();

        /// <summary>
        /// This method converts a DataTable to an IList of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            try
            {
                List<PropertyInfo> properties = GetPropertiesForType<T>();
                List<T> result = new List<T>();

                if (table != null && table.Rows?.Count > 0)
                {
                    foreach (var row in table.Rows)
                    {
                        var item = CreateItemFromRow<T>((DataRow)row, properties);
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method get the properties of type T
        /// </summary>
        /// <typeparam name="T">the type</typeparam>
        /// <returns>List of type PropertyInfo</returns>
        public static List<PropertyInfo> GetPropertiesForType<T>()
        {
            Dictionary<Type, List<PropertyInfo>> typeDictionary = new Dictionary<Type, List<PropertyInfo>>();

            var type = typeof(T);
            if (!typeDictionary.ContainsKey(typeof(T)))
            {
                typeDictionary.Add(type, type.GetProperties().ToList());
            }

            return typeDictionary[type];
        }

        private static T CreateItemFromRow<T>(DataRow row, List<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (row.Table.Columns.Contains(property.Name))
                {
                    if (property.PropertyType == typeof(double))
                    {
                        property.SetValue(item, Convert.ToDouble(row[property.Name]), null);
                    }
                    else if (property.PropertyType == typeof(decimal))
                    {
                        property.SetValue(item, Convert.ToDecimal(row[property.Name]), null);
                    }
                    else if (property.PropertyType == typeof(float))
                    {
                        property.SetValue(item, Convert.ToDouble(row[property.Name]), null);
                    }
                    else if (property.PropertyType == typeof(long))
                    {
                        property.SetValue(item, Convert.ToInt64(row[property.Name]), null);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(item, Convert.ToInt32(row[property.Name]), null);
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(item, Convert.ToBoolean(row[property.Name]), null);
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        property.SetValue(item, Convert.ToDateTime(row[property.Name]), null);
                    }
                    else //string
                    {
                        property.SetValue(item, Convert.ToString(row[property.Name]), null);
                    }
                }
            }
            return item;
        }

        /// <summary>
        /// This method converts a DataTable to a List of type string having comma separated values.
        /// </summary>
        /// <param name="table">the data table</param>
        /// <returns>List of type string</returns>
        public static List<string> ToLines(this DataTable table)
        {
            if (table.IsEmpty()) return null;
            List<string> list = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                string line = string.Empty;
                foreach (DataColumn column in table.Columns)
                {
                    line += $"{Convert.ToString(row[column.ColumnName])},";
                }
                list.Add(line);
            }
            return list;
        }

        /// <summary>
        /// This method will determine if a DataTable contains and data.
        /// </summary>
        /// <param name="dt">DataTable object</param>
        /// <returns></returns>
        public static bool IsEmpty(this DataTable dt)
        {
            if (dt == null) return true;
            if (dt.Rows == null) return true;
            if (dt.Rows.Count == 0) return true;
            return false;
        }

        /// <summary>
        /// This method will determine if a DataSet contains and data.
        /// </summary>
        /// <param name="ds">DataSet object</param>
        /// <returns></returns>
        public static bool IsEmpty(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return true;
            if (ds.Tables[0].Rows == null) return true;
            if (ds.Tables[0].Rows.Count == 0) return true;
            return false;
        }

    }
}
