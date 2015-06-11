using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel;
namespace ObjectCMS.DataAccess
{
    /// <summary>
    /// SqlHelper扩展(依赖AutoMapper.dll)
    /// </summary>
    public sealed partial class SqlHelper
    {

        #region 实例方法

        public T ExecuteObject<T>(string commandText, params SqlParameter[] parms) where T : class, new()
        {
            return ExecuteObject<T>(this.ConnectionString, commandText, parms);
        }

        public List<T> ExecuteObjects<T>(string commandText, params SqlParameter[] parms) where T : class, new()
        {
            return ExecuteObjects<T>(this.ConnectionString, commandText, parms);
        }

        #endregion

        #region 静态方法

        public static T ExecuteObject<T>(string connectionString, string commandText, params SqlParameter[] parms) where T : class, new()
        {
            //DataTable dt = ExecuteDataTable(connectionString, commandText, parms);
            //return AutoMapper.Mapper.DynamicMap<List<T>>(dt.CreateDataReader()).FirstOrDefault();
            using (SqlDataReader reader = ExecuteDataReader(connectionString, commandText, parms))
            {
                return Translate<T>(reader).FirstOrDefault();
            }
        }

        public static List<T> ExecuteObjects<T>(string connectionString, string commandText, params SqlParameter[] parms) where T : class, new()
        {
            //DataTable dt = ExecuteDataTable(connectionString, commandText, parms);
            //return AutoMapper.Mapper.DynamicMap<List<T>>(dt.CreateDataReader());
            using (SqlDataReader reader = ExecuteDataReader(connectionString, commandText, parms))
            {
                return Translate<T>(reader);
            }
        }

        public static List<TEntity> Translate<TEntity>(IDataReader reader) where TEntity : class, new()
        {
            List<TEntity> list = new List<TEntity>();
            Type entityType = typeof(TEntity);

            Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo info in entityType.GetProperties())
            {
                dic.Add(info.Name, info);
            }

            string columnName = string.Empty;
            while (reader.Read())
            {
                TEntity t = new TEntity();
                foreach (KeyValuePair<string, PropertyInfo> attribute in dic)
                {
                    columnName = attribute.Key;
                    int filedIndex = 0;
                    while (filedIndex < reader.FieldCount)
                    {

                        if (reader.GetName(filedIndex) == columnName)
                        {
                            attribute.Value.SetValue(t, DataTypeConverter.ChangeType(attribute.Value.PropertyType, reader[filedIndex]), null);
                            break;

                        }
                        filedIndex++;
                    }
                }
                list.Add(t);

            }
            return list;
        }
        #endregion
    }
    public class DataTypeConverter
    {
        public static object ChangeType(Type type, object value)
        {
            if (value is DBNull)
                return null;
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value != null)
                {
                    NullableConverter nullableConverter = new NullableConverter(type);
                    type = nullableConverter.UnderlyingType;
                }
                else
                {
                    return null;
                }
            }
            return Convert.ChangeType(value, type);
        }
    }

}