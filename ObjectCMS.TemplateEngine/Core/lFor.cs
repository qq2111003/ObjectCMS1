using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using System.Data;
using ObjectCMS.TemplateEngine.Common;
using ObjectCMS.Common;
using ObjectCMS.BLL;
using ObjectCMS.Model.ModelConfig;

namespace ObjectCMS.TemplateEngine.Core
{
    public static class lFor
    {

        public static string ReplaceLabeltoData(string labelHTML, Hashtable[] param_arr)
        {
            StringBuilder sb = new StringBuilder();

            int nodeId = ParamController.GetParam("nodeid", param_arr).ToInt();
            string nodeIds = ParamController.GetParam("nodeids", param_arr);
            int pageSize = ParamController.GetParam("pagesize", param_arr).ToInt(10);
            int pageIndex = ParamController.GetParam("pageindex", param_arr).ToInt(1);
            string sql = ParamController.GetParam("sql", param_arr);
            string orderBy = ParamController.GetParam("orderby", param_arr);


            #region 查询数据
            Node node = Node.GetOne(nodeId);

            string tableName = UserModel.GetOne(node.UserModelId).TableName;
            string where = "NodeId=" + nodeId + " and Enable='True' ";
            if (!string.IsNullOrEmpty(nodeIds)) {
                where = "NodeId in (" + nodeIds + ") and Enable='True' ";
            }
            if (!string.IsNullOrEmpty(sql))
            {
                where += " and " + sql;
            }
            string fields = TemplateEngineManage.Instance.GetAllNodeField(nodeId);

            int recordCount = 0;
            DataTable dt = ModelManage.Instance.DataList(pageIndex, pageSize, fields, tableName, where, orderBy??"Sort DESC", out recordCount);


            #endregion
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append(lModelData.ReplaceLabeltoData(labelHTML.IReplace("{index}", i + "").IReplace("{recordcount}", recordCount.ToString()), dt.Rows[i],node,param_arr) + "\n");
            }
            dt.Dispose();
            dt.Clear();
            return sb.ToString();
        }
    }
}
