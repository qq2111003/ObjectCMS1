using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.ModelConfig;
using UserControls.Controls.jeasyui;
using ObjectCMS.BLL;
using ObjectCMS.TemplateEngine;

namespace SiteWeb.Manage.Model
{
    public partial class NodeDataEdit : System.Web.UI.Page
    {

        int nodeId = 0;
        Node node = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["nodeid"], out nodeId))
            {

            }

            node = Node.GetOne(nodeId);
            #region 字段配置
            List<NodeUserModelField> lnumf = NodeUserModelField.GetALL("NodeId=" + nodeId, "Sort");
            List<UserModelField> lumf = UserModelField.GetALL("UserModelId=" + node.UserModelId, "Id");
            var AllField = from f in lnumf
                           from g in lumf
                           from h in UserModelFieldType.GetALL("1=1", "Id")
                           where f.UserModelFieldId == g.Id && g.FieldTypeId == h.Id
                           select new FormItem()
                           {
                               FieldName = g.FieldName,
                               FieldType = (FieldTypeInHTML)Enum.Parse(typeof(FieldTypeInHTML), h.TypeName),
                               Title = f.ReWriteTitle
                           };

            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = AllField.ToList();
            #endregion

            #region  设置form初始值

            string tableName = UserModel.GetOne(node.UserModelId).TableName;
            string fields = string.Join(",", (from f in lnumf
                                             from g in lumf
                                             where f.UserModelFieldId == g.Id
                                             select "[" + g.FieldName + "]").ToArray());

            var dataRow = ModelManage.Instance.DataReader("NodeId=" + nodeId, tableName, "Id," + fields);

            if (dataRow == null)
            {
                FormBuilder1.Entity = new Dictionary<string, object>();
            }
            else
            {
                FormBuilder1.Entity = dataRow;
            }

            #endregion

            if (IsPostBack)
            {
                Dictionary<string, object> data = (Dictionary<string, object>)FormBuilder1.Entity;
                data.Add("NodeId", nodeId);

                if (dataRow == null)                //添加模式
                {
                    ModelManage.Instance.DataCreate(tableName, data);
                    BuildNode.BuildMenuPage(nodeId);
                }
                else                        //修改模式
                {
                    ModelManage.Instance.DataUpdate(tableName, data);
                    BuildNode.BuildMenuPage(nodeId);
                }
                Response.Write("<script>parent.Message.show('修改成功','提示');location.href=location.href;</script>");
                Response.End();

            }
        }
    }
}