using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.DAL;
using ObjectCMS.Model.ModelConfig;
using System.Data;

namespace ObjectCMS.BLL
{
    public class ModelManage
    {

        private readonly ModelService dal = new ModelService();
        public static readonly ModelManage Instance = new ModelManage();
        public void CreateTable(UserModel um)
        {
            dal.CreateTable(um);
        }
        public void AddColumn(UserModelField umf)
        {
            dal.AddColumn(umf);
        }
        public void DelColumn(int Id)
        {
            dal.DelColumn(Id);
        }


        public DataTable GetAllColumn(int nodeId)
        {

            return dal.GetAllColumn(nodeId);
        }
        public DataTable DataList(int pageIndex, int pageSize, string field, string tableName, string where, string orderBy, out int recordCount)
        {
            return dal.DataList(pageIndex, pageSize, field, tableName, where, orderBy, out recordCount);
        }

        public Dictionary<string, object> DataReader(int id, string tableName, string fields)
        {
            return dal.DataReader(id, tableName, fields);
        }
        public Dictionary<string, object> DataReader(string where, string tableName, string fields)
        {
            return dal.DataReader(where, tableName, fields);
        }


        public int DataCreate(string tableName, Dictionary<string, object> datarow)
        {
            return dal.DataCreate(tableName, datarow);
        }
        public void DataUpdate(string tableName, Dictionary<string, object> datarow)
        {
            dal.DataUpdate(tableName, datarow);
        }
        public void DataDel(int id, string tableName)
        {
            dal.DataDel(id, tableName);
        }
        public void SyncDataMark_Add(string MarkName)
        {
            dal.SyncDataMark_Add(MarkName);
        }
        public void SyncDataMark_Del(string MarkName)
        {
            dal.SyncDataMark_Del(MarkName);
        }
    }
}
