using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ObjectCMS.DataAccess;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Configuration;
using System.Web;
using ObjectCMS.Common;

namespace ObjectCMS.Model.Core
{
    public class BaseModel<T> where T : BaseModel<T>, new()
    {
        private readonly Dictionary<string, Func<object, object>> _getValueDelegates = new Dictionary<string, Func<object, object>>();
        private readonly Dictionary<string, Action<object, object>> _setValueDelegates = new Dictionary<string, Action<object, object>>();

        private static ModelAttribute classAttribute = (ModelAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ModelAttribute));

        private object GetValue(object instance, string memberName)
        {
            var type = instance.GetType();
            var key = type.FullName + memberName;
            Func<object, object> getValueDelegate;
            _getValueDelegates.TryGetValue(key, out getValueDelegate);
            if (getValueDelegate == null)
            {
                var info = type.GetProperty(memberName);
                var target = Expression.Parameter(typeof(object), "target");

                var getter = Expression.Lambda(typeof(Func<object, object>),
                    Expression.Convert(Expression.Property(Expression.Convert(target, type), info), typeof(object)),
                    target
                    );

                getValueDelegate = (Func<object, object>)getter.Compile();
                _getValueDelegates.Add(key, getValueDelegate);
            }

            return getValueDelegate(instance);
        }
        public object GetValue(string memberName)
        {
            return GetValue(this, memberName);
        }
        private void SetValue(object instance, string memberName, object memberVal)
        {
            var type = instance.GetType();
            var key = type.FullName + memberName;
            Action<object, object> setValueDelegate;
            _setValueDelegates.TryGetValue(key, out setValueDelegate);
            if (setValueDelegate == null)
            {
                var info = type.GetProperty(memberName);
                var target = Expression.Parameter(typeof(object), "target");
                var val = Expression.Parameter(typeof(object), "memberVal");

                var setter = Expression.Lambda(
                    typeof(Action<object, object>),
                    Expression.Call(
                        Expression.Convert(target, instance.GetType()),
                        info.GetSetMethod(),
                        Expression.Convert(val, info.PropertyType)
                    ),
                    target,
                    val
                );
                setValueDelegate = (Action<object, object>)setter.Compile();
                _setValueDelegates.Add(key, setValueDelegate);
            }
            setValueDelegate(instance, memberVal);
        }
        public void SetValue(string memberName, object memberVal)
        {
            SetValue(this, memberName, memberVal);
        }

        public static List<T> Translate(DataTable dataTable)
        {
            List<T> list = new List<T>();
            Type entityType = typeof(T);

            Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo info in entityType.GetProperties())
            {
                dic.Add(info.Name, info);
            }

            string columnName = string.Empty;
            foreach (DataRow dr in dataTable.Rows)
            {
                T t = new T();
                foreach (KeyValuePair<string, PropertyInfo> attribute in dic)
                {
                    columnName = attribute.Key;
                    int filedIndex = 0;
                    while (filedIndex < dataTable.Columns.Count)
                    {

                        if (dataTable.Columns[filedIndex].ColumnName == columnName)
                        {
                            attribute.Value.SetValue(t, ChangeType(attribute.Value.PropertyType, dr[filedIndex]), null);
                            break;
                        }
                        filedIndex++;
                    }
                }
                list.Add(t);
            }
            return list;
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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

        #region 基础SQL方法

        private static SqlHelper GetDBO(string tableName)
        {
            if (tableName.ToLower().StartsWith("sys"))
            {
                return DataHelperFactory.Create(ConfigurationManager.ConnectionStrings["maindb"].ConnectionString);
            }
            else
            {
                string constr = ConfigurationManager.AppSettings["sitedbconstrrule"];
                constr = constr.IReplace("{SiteMark}", Cookie.GetCookie("curdb"));
                return DataHelperFactory.Create(constr);
            }
        }
        /// <summary>
        /// 插入
        /// </summary>
        public virtual void Insert()
        {
            var array = (from f in classAttribute.Fields
                         where GetValue(this, f) != null
                         select f).ToArray();

            SqlParameter[] pars = new SqlParameter[array.Length];

            for (int i = 0; i < pars.Length; i++)
            {
                pars[i] = new SqlParameter("@" + array[i], GetValue(this, array[i]));
            }
            string sql = "INSERT INTO " + classAttribute.TableName + "(" + string.Join(",", array) + ") VALUES(@" + string.Join(",@", array) + ")";
            GetDBO(classAttribute.TableName).ExecuteNonQuery(CommandType.Text, sql, pars);
        }
        /// <summary>
        /// 更新
        /// </summary>
        public virtual void Update(string where)
        {
            if (string.IsNullOrEmpty(where))
            {
                return;
            }

            var array = (from f in classAttribute.Fields
                         where GetValue(this, f) != null
                         select f).ToArray();

            SqlParameter[] pars = new SqlParameter[array.Length];

            for (int i = 0; i < pars.Length; i++)
            {
                pars[i] = new SqlParameter("@" + array[i], GetValue(this, array[i]));
            }

            var updateFields = from f in array
                               select f + "=@" + f;
            string sql = "UPDATE " + classAttribute.TableName + " set " + string.Join(",", updateFields.ToArray()) + " WHERE " + where;
            GetDBO(classAttribute.TableName).ExecuteNonQuery(CommandType.Text, sql, pars);
        }

        public virtual void Update()
        {
            Update("Id=" + GetValue(this, "Id").ToString());
        }
        /// <summary>
        /// 删除
        /// </summary>
        public virtual void Delete(string where)
        {
            if (string.IsNullOrEmpty(where))
            {
                return;
            }
            string sql = "DELETE " + classAttribute.TableName + " WHERE " + where;
            GetDBO(classAttribute.TableName).ExecuteNonQuery(CommandType.Text, sql);
        }
        /// <summary>
        /// 删除
        /// </summary>
        public virtual void Delete()
        {
            Delete("Id=" + GetValue(this, "Id").ToString());
        }
        public static void DeleteByWhere(string where)
        {
            if (string.IsNullOrEmpty(where))
            {
                return;
            }
            string sql = "DELETE " + classAttribute.TableName + " WHERE " + where;
            GetDBO(classAttribute.TableName).ExecuteNonQuery(CommandType.Text, sql);
        }
        public static T GetOne(string where)
        {
            if (string.IsNullOrEmpty(where))
            {
                return null;
            }
            string sql = "SELECT top 1 Id," + string.Join(",", classAttribute.Fields) + " FROM " + classAttribute.TableName + " WHERE " + where;
            DataTable dt = GetDBO(classAttribute.TableName).ExecuteDataTable(CommandType.Text, sql);

            var model = Translate(dt);
            if (model.Count > 0)
            {
                return model[0];
            }
            return null;
        }
        public static T GetOne(int id)
        {
            return GetOne("Id=" + id);
        }


        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static List<T> GetALL(string where, string orderBy)
        {
            string sql = "SELECT Id," + string.Join(",", classAttribute.Fields) + " FROM " + classAttribute.TableName + " WHERE " + where + " ORDER BY " + orderBy;
            DataTable dt = GetDBO(classAttribute.TableName).ExecuteDataTable(CommandType.Text, sql);

            return Translate(dt);
        }

        public static List<T> Pager(int pageIndex, int pageSize, string where, string orderBy, out int recordCount)
        {
            recordCount = 0;
            SqlParameter[] pars = {
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@Field","Id," + string.Join(",", classAttribute.Fields)),
                new SqlParameter("@TableName",classAttribute.TableName),
                new SqlParameter("@Where",where+" order by "+orderBy),
                new SqlParameter("@rowcount",SqlDbType.Int ,4 ,ParameterDirection.Output, false, ((global::System.Byte)(0)), ((global::System.Byte)(0)), "", DataRowVersion.Current, null)
            };
            DataTable dt = GetDBO(classAttribute.TableName).ExecuteDataTable(CommandType.StoredProcedure, "Pager", pars);
            int.TryParse(pars[5].Value != null ? pars[5].Value.ToString() : "0", out recordCount);
            return Translate(dt);
        }

        #endregion
    }

}
