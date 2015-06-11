using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.DAL;
using System.Data;

namespace ObjectCMS.BLL
{
    public class TemplateEngineManage
    {

        private readonly TemplateEngineService dal = new TemplateEngineService();
        public static readonly TemplateEngineManage Instance = new TemplateEngineManage();
        public string GetAllNodeField(int nodeId)
        {
            return dal.GetAllNodeField(nodeId);
        }
        public DataSet GetPrevNext(int currId, string tableName, string field, string where, string orderBy)
        {
            return dal.GetPrevNext(currId, tableName, field, where, orderBy);
        }
        public int GetRecordCount(string tableName, string where = "1=1")
        {
            return dal.GetRecordCount(tableName, where);
        }
        public DataTable GetAllData(int nodeId, string tableName)
        {
            return dal.GetAllData(nodeId,tableName);
        }
    }
}
