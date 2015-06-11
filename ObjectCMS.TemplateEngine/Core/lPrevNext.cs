using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using ObjectCMS.Common;
using ObjectCMS.TemplateEngine.Common;
using ObjectCMS.Model.ModelConfig;
using ObjectCMS.BLL;

namespace ObjectCMS.TemplateEngine.Core
{
    public class lPrevNext
    {
        public static string ReplaceLabeltoData(string labelHTML, Hashtable[] param_arr)
        {
            int nodeid = ParamController.GetParam("nodeid", param_arr).ToInt();
            int id = ParamController.GetParam("id", param_arr).ToInt();
            string sql = ParamController.GetParam("sql", param_arr);
            string orderby = ParamController.GetParam("orderby", param_arr);

            var node = Node.GetOne(nodeid);
            string fields = TemplateEngineManage.Instance.GetAllNodeField(nodeid);
            string tableName = UserModel.GetOne(node.UserModelId).TableName;

            string where = "NodeId=" + nodeid + " and Enable='True' ";
            if (!string.IsNullOrEmpty(sql))
            {
                where += " and " + sql;
            }
            StringBuilder sb = new StringBuilder();

            var ds = TemplateEngineManage.Instance.GetPrevNext(id, tableName, fields, where, orderby);


            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    labelHTML = labelHTML.IReplace("{prevnext:prevurl}", "details" + ds.Tables[0].Rows[0]["nodeId"].ToString() + "_" + ds.Tables[0].Rows[0]["id"].ToString() + ".html").IReplace("{prevnext:prevTitle}", ds.Tables[0].Rows[0]["Title"].ToString().CutString(40));

                }
                else
                {
                    labelHTML = labelHTML.IReplace("{prevnext:prevurl}", "javascript:;").IReplace("{prevnext:prevTitle}", "已是第一篇了");

                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    labelHTML = labelHTML.IReplace("{prevnext:prevurl}", "details" + ds.Tables[1].Rows[0]["nodeId"].ToString() + "_" + ds.Tables[1].Rows[0]["id"].ToString() + ".html").IReplace("{prevnext:prevTitle}", ds.Tables[1].Rows[0]["Title"].ToString().CutString(40));

                }
                else
                {
                    labelHTML = labelHTML.IReplace("{prevnext:prevurl}", "javascript:;").IReplace("{prevnext:prevTitle}", "已是最后一篇了");
                }
            }
            return labelHTML;

        }
    }
}
