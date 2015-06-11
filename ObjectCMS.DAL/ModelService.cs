using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.ModelConfig;
using System.Data;
using System.Data.SqlClient;
using ObjectCMS.Common;

namespace ObjectCMS.DAL
{
    public class ModelService : DataProviderBase
    {
        public void CreateTable(UserModel um)
        {
            SqlParameter[] pars = new SqlParameter[]{
                new SqlParameter("@Title", SqlDbType.NVarChar, 100),
                new SqlParameter("@TableName", SqlDbType.VarChar, 50)
			};
            pars[0].Value = um.Title;
            pars[1].Value = um.TableName;
            CurrentDB.ExecuteDataTable(CommandType.StoredProcedure, "CreateTable", pars);
        }

        public void AddColumn(UserModelField umf)
        {

            SqlParameter[] pars = new SqlParameter[]{
                new SqlParameter("@UserModelId", SqlDbType.Int, 4),
                new SqlParameter("@Title", SqlDbType.NVarChar, 200),
                new SqlParameter("@FieldName", SqlDbType.VarChar, 50),
                new SqlParameter("@FieldTypeId", SqlDbType.Int, 4)
			};
            pars[0].Value = umf.UserModelId;
            pars[1].Value = umf.Title;
            pars[2].Value = umf.FieldName;
            pars[3].Value = umf.FieldTypeId;
            CurrentDB.ExecuteDataTable(CommandType.StoredProcedure, "AddColumn", pars);
        }
        public void DelColumn(int umfId)
        {

            SqlParameter[] pars = new SqlParameter[]{
                new SqlParameter("@Id", SqlDbType.Int, 4),
			};
            pars[0].Value = umfId;
            CurrentDB.ExecuteDataTable(CommandType.StoredProcedure, "DelColumn", pars);
        }


        public DataTable GetAllColumn(int nodeId)
        {
            string sql = @"select a.Id,a.FieldName,(select TypeName from UserModelFieldType where Id = a.FieldTypeId) as FieldType,
                            a.Title,(case when a.id=b.UserModelFieldId then 1 else 0 end) as flag,
                            b.ReWriteTitle,b.ShowInList,b.ListWidth,b.Sort,b.DefVal,b.OtherAttr,b.Validator,b.Tip
                            from UserModelField a left join NodeUserModelField b 
                            on a.id=b.UserModelFieldId and NodeId=" + nodeId
                            + "where a.usermodelid in (select UserModelId from Node where Id=" + nodeId + ")";
            return CurrentDB.ExecuteDataTable(CommandType.Text, sql);
        }

        public DataTable DataList(int pageIndex, int pageSize, string field, string tableName, string where, string orderBy, out int recordCount)
        {
            recordCount = 0;
            SqlParameter[] pars = {
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@Field",field),
                new SqlParameter("@TableName",tableName),
                new SqlParameter("@Where",where+" order by "+orderBy),
                new SqlParameter("@rowcount",SqlDbType.Int ,4 ,ParameterDirection.Output, false, ((global::System.Byte)(0)), ((global::System.Byte)(0)), "", DataRowVersion.Current, null)
            };
            DataTable dt = CurrentDB.ExecuteDataTable(CommandType.StoredProcedure, "Pager", pars);
            int.TryParse(pars[5].Value != null ? pars[5].Value.ToString() : "0", out recordCount);
            return dt;
        }

        public Dictionary<string, object> DataReader(int id, string tableName, string fields)
        {
            string sql = "select top 1 " + fields + " from " + tableName + " where Id=" + id + "";
            var dt = CurrentDB.ExecuteDataTable(CommandType.Text, sql);

            if (dt != null && dt.Rows.Count == 1)
            {
                Dictionary<string, object> datarow = new Dictionary<string, object>();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    datarow.Add(dt.Columns[i].ColumnName, dt.Rows[0][i]);
                }
                return datarow;
            }
            return null;
        }
        public Dictionary<string, object> DataReader(string where, string tableName, string fields)
        {
            string sql = "select top 1 " + fields + " from " + tableName + " where " + where + " order by Id desc";
            var dt = CurrentDB.ExecuteDataTable(CommandType.Text, sql);

            if (dt != null && dt.Rows.Count == 1)
            {
                Dictionary<string, object> datarow = new Dictionary<string, object>();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    datarow.Add(dt.Columns[i].ColumnName, dt.Rows[0][i]);
                }
                return datarow;
            }
            return null;
        }


        public int DataCreate(string tableName, Dictionary<string, object> datarow)
        {
            SqlParameter[] pars = new SqlParameter[datarow.Count];

            for (int i = 0; i < pars.Length; i++)
            {
                pars[i] = new SqlParameter("@" + datarow.ElementAt(i).Key, datarow.ElementAt(i).Value);
            }
            var fields = from f in datarow
                         select f.Key;
            string sql = "INSERT INTO " + tableName + "(" + string.Join(",", fields.ToArray()) + ") VALUES(@" + string.Join(",@", fields.ToArray()) + ")";
            sql += " select SCOPE_IDENTITY() as 'id'";
            var obj = CurrentDB.ExecuteScalar(CommandType.Text, sql, pars);
            if (obj != null) {
                return obj.ToInt();
            }
            return 0;
        }
        public void DataUpdate(string tableName, Dictionary<string, object> datarow)
        {
            int Id = Convert.ToInt32(datarow["Id"]);
            datarow.Remove("Id");
            SqlParameter[] pars = new SqlParameter[datarow.Count];

            for (int i = 0; i < pars.Length; i++)
            {
                pars[i] = new SqlParameter("@" + datarow.ElementAt(i).Key, datarow.ElementAt(i).Value);
            }
            var updateFields = from f in datarow
                               select f.Key + "=@" + f.Key;
            string sql = "UPDATE " + tableName + " set " + string.Join(",", updateFields.ToArray()) + " WHERE Id=" + Id;
            CurrentDB.ExecuteNonQuery(CommandType.Text, sql, pars);
        }
        public void DataDel(int id, string tableName)
        {
            string sql = "DELETE " + tableName + " WHERE Id=" + id;
            CurrentDB.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void SyncDataMark_Add(string MarkName)
        {
            var allTable = UserModel.GetALL("1=1", "Id");
            string sql = "";
            foreach (var table in allTable)
            {
                sql += "ALTER TABLE " + table.TableName + " add " + MarkName + " bit default(0) \r\n";
            }
            CurrentDB.ExecuteDataTable(CommandType.Text, sql, null);
        }

        public void SyncDataMark_Del(string MarkName)
        {
            var allTable = UserModel.GetALL("1=1", "Id");
            string sql = "";
            foreach (var table in allTable)
            {
                sql += "EXEC SyscMarkName_Del '" + table.TableName + "','" + MarkName + "' \r\n";
            }
            CurrentDB.ExecuteDataTable(CommandType.Text, sql, null);
        }
    }
}
