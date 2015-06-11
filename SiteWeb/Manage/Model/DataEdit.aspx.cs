using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.ModelConfig;
using UserControls.Controls.jeasyui;
using ObjectCMS.BLL;
using System.Data;
using ObjectCMS.TemplateEngine;
using ObjectCMS.Common;

namespace SiteWeb.Manage.Model
{
    public partial class DataEdit : System.Web.UI.Page
    {
        int nodeId = 0;
        Node node = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.Form["method"]))
            {
                string tname = UserModel.GetOne(Node.GetOne(Request.Form["nodeid"].ToInt()).UserModelId).TableName;
                var p = new Dictionary<string, object>();
                p.Add("Id", Request.Form["id"].ToInt());
                p.Add("Sort", Request.Form["value"].ToInt());
                ModelManage.Instance.DataUpdate(tname, p);
                Response.End();
            }

            if (!int.TryParse(Request.QueryString["nodeid"], out nodeId))
            {

            }
            node = Node.GetOne(nodeId);
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
                               Title = f.ReWriteTitle,
                               DefVal = f.DefVal,
                               OtherAttr = f.OtherAttr,
                               ValidateTypes = string.IsNullOrEmpty(f.Validator) ? null : new List<FieldValidate>(){(FieldValidate)Enum.Parse(typeof(FieldValidate), f.Validator)},
                               Tip = f.Tip
                           };

            string tableName = UserModel.GetOne(node.UserModelId).TableName;
            string fields = string.Join( ",", (from f in lnumf
                                             from g in lumf
                                             where f.UserModelFieldId == g.Id
                                             select "[" + g.FieldName + "]").ToArray());
            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);

            //设置form初始值 
            if (id == 0)
            {
                Dictionary<string, object> datarow = new Dictionary<string, object>();
                //
                FormBuilder1.Entity = datarow;
            }
            else
            {
                FormBuilder1.Entity = ModelManage.Instance.DataReader(id, tableName, "Id," + fields);
            }

            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = AllField.ToList();
            #endregion

            if (IsPostBack)
            {

                Dictionary<string, object> data = (Dictionary<string, object>)FormBuilder1.Entity;
                data.Add("NodeId", nodeId);

                if (id == 0)                //添加模式
                {
                    id = ModelManage.Instance.DataCreate(tableName, (Dictionary<string, object>)data);
                    BuildNode.BuildListPage(nodeId, id);
                    Response.Write("<script>parent.Message.show('添加成功','提示');try{parent.DataGrid1Reload();}catch(e){}parent.UIDialog.Close();</script>");
                    Response.End();
                }
                else                        //修改模式
                {
                    ModelManage.Instance.DataUpdate(tableName, (Dictionary<string, object>)data);
                    BuildNode.BuildListPage(nodeId, id);
                    Response.Write("<script>parent.Message.show('修改成功','提示');try{parent.DataGrid1Reload();}catch(e){}parent.UIDialog.Close();</script>");
                    Response.End();
                }

            }
        }
    }
}