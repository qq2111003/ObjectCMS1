using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using ObjectCMS.Common;
using ObjectCMS.TemplateEngine.Common;
using ObjectCMS.Model.ModelConfig;
using ObjectCMS.BLL;

namespace ObjectCMS.TemplateEngine.Core
{
    public static class lDetail
    {
        public static string ReplaceLabeltoData(string labelHTML, Hashtable[] param_arr)
        {
            int id = ParamController.GetParam("id", param_arr).ToInt();
            int nodeId = ParamController.GetParam("nodeid", param_arr).ToInt();
            var node = Node.GetOne(nodeId);

            DataTable dt;

            string fields = TemplateEngineManage.Instance.GetAllNodeField(nodeId);
            string tableName = UserModel.GetOne(node.UserModelId).TableName;
            //单篇内容页
            int recordCount = 0;
            if (id == 0)
            {
                dt = ModelManage.Instance.DataList(1, 1, fields, tableName, "NodeId=" + nodeId, "Id desc", out recordCount);
            }
            else
            {
                dt = ModelManage.Instance.DataList(1, 1, fields, tableName, "NodeId=" + nodeId + " and Id=" + id, "Id desc", out recordCount);
            }
            
            if (dt.Rows.Count > 0)
            {
                return lModelData.ReplaceLabeltoData(labelHTML, dt.Rows[0], node, param_arr);
            }
            else
            {
                return labelHTML;
            }

        }

        public static string ReplaceLabeltoData(string LabelHTML, Hashtable[] param_arr, List<string> SingleParam)
        {
            throw new NotImplementedException();
        }
    }
}
