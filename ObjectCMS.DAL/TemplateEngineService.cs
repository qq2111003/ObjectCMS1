using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ObjectCMS.Common;

namespace ObjectCMS.DAL
{
    public class TemplateEngineService : DataProviderBase
    {

        public string GetAllNodeField(int nodeId)
        {

            SqlParameter[] pars = new SqlParameter[]{
                new SqlParameter("@NodeId", SqlDbType.Int, 4)
			};
            pars[0].Value = nodeId;
            var result = CurrentDB.ExecuteScalar(CommandType.StoredProcedure, "GetAllNodeField", pars);
            if (result != null)
                return result.ToString();
            return "";
        }
        public DataSet GetPrevNext(int currId, string tableName, string field, string where, string orderBy)
        {

            string sql = "select * from (select row_number() over(order by " + orderBy + ") as r,* from " + tableName + " where " + where + ") as a inner join(select r from (select row_number() over(order by " + orderBy + ") as r,id from " + tableName + " where " + where + ") as t where id=" + currId + ") as b on a.r=b.r-1";
            sql += " select * from (select row_number() over(order by " + orderBy + ") as r,* from " + tableName + " where " + where + ") as a inner join(select r from (select row_number() over(order by " + orderBy + ") as r,id from " + tableName + " where " + where + ") as t where id=" + currId + ") as b on a.r=b.r+1";
            return CurrentDB.ExecuteDataSet(CommandType.Text, sql, null);
        }
        public int GetRecordCount(string tableName, string where = "1=1")
        {
            string sql = "select count(Id) from " + tableName + " where " + where;
            var result = CurrentDB.ExecuteScalar(CommandType.Text, sql, null);
            if (result != null)
                return result.ToInt();
            return 0;
        }

        public DataTable GetAllData(int nodeId,string tableName)
        {
            string sql = "select id from " + tableName + " where nodeId=" + nodeId + " and Enable='True' ";
            return CurrentDB.ExecuteDataTable(CommandType.Text, sql, null);
        }
    }
}
