using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.ModelConfig;
using UserControls.Controls.jeasyui;
using ObjectCMS.Model;

namespace SiteWeb.Manage.Model
{
    public partial class NodeSetTemplate : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(10, "staticset");
            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);

            Node currNode = Node.GetOne(id);

            FormBuilder1.Entity = currNode;


            //字段配置  类型\提示信息\验证方式等     
            if (currNode.PageType)
            {

                FormBuilder1.Items = new List<FormItem>()
                {
                    new FormItem(){Title = "列表模板", FieldName = "ListTemplateId", FieldType = FieldTypeInHTML.Select,
                        FieldModel = new ModelSelect(){Items=(from u in TeTemplates.GetALL("1=1","Id") select u.Id+"|"+u.Title).ToArray()}},
                    new FormItem(){Title = "生成地址", FieldName = "ListPageUrl", FieldType =  FieldTypeInHTML.SingleLine},
                    new FormItem(){Title = "内容模板", FieldName = "DetailTemplateId", FieldType = FieldTypeInHTML.Select,
                        FieldModel = new ModelSelect(){Items=(from u in TeTemplates.GetALL("1=1","Id") select u.Id+"|"+u.Title).ToArray()}},
                    new FormItem(){Title = "生成地址", FieldName = "DetailPageUrl", FieldType =  FieldTypeInHTML.SingleLine}

                };
            }
            else
            {
                FormBuilder1.Items = new List<FormItem>()
                {
                    new FormItem(){Title = "选择模板", FieldName = "ListTemplateId", FieldType = FieldTypeInHTML.Select,
                        FieldModel = new ModelSelect(){Items=(from u in TeTemplates.GetALL("1=1","Id") select u.Id+"|"+u.Title).ToArray()}},
                    new FormItem(){Title = "生成地址", FieldName = "ListPageUrl", FieldType =  FieldTypeInHTML.SingleLine},

                };
            }
            #endregion

            if (IsPostBack)
            {
                //需要额外给id赋值
                var entity = (Node)FormBuilder1.Entity;
                entity.Id = id;
                entity.Update();
                Response.Write("<script>parent.Message.show('设置成功','提示');parent.TreeGrid1Reload();parent.UIDialog.Close();</script>");
                Response.End();
            }
        }
    }
}