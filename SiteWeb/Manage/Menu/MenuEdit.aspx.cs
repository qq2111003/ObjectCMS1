using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.System;
using UserControls.Controls.jeasyui;

namespace SiteWeb.Manage.Menu
{
    public partial class MenuEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);

            //设置form初始值 
            if (id == 0)
            {
                this.Visible = this.HasPermission(2, "add");
                FormBuilder1.Entity = new SysMenu() { Enable=true};
            }
            else
            {
                this.Visible = this.HasPermission(2, "edit");
                FormBuilder1.Entity = SysMenu.GetOne(id);
            }

            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = new List<FormItem>()
            {
                new FormItem(){Title = "标　　题", FieldName = "Title", FieldType =  FieldTypeInHTML.SingleLine, 
                    FieldModel= new ModelSingleLine(){ValidateTypes=new List<FieldValidate>(){FieldValidate.required}}},
                new FormItem(){Title = "父　　级", FieldName = "ParentId", FieldType = FieldTypeInHTML.TreeNode,
                    FieldModel = new ModelTreeNode(){IdField = "Id",ParentField = "ParentId",DataSource = SysMenu.GetALL("1=1", "id")}},      
                new FormItem(){Title = "图　　标", FieldName = "Ico", FieldType = FieldTypeInHTML.ImgUpload},
                new FormItem(){Title = "地　　址", FieldName = "Url", FieldType =  FieldTypeInHTML.SingleLine}
            };
            #endregion

            if (IsPostBack)
            {
                if (id == 0)                //添加模式
                {
                    var entity = (SysMenu)FormBuilder1.Entity;
                    entity.Insert();
                    Response.Write("<script>parent.Message.show('添加成功','提示');parent.TreeGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();
                }
                else                        //修改模式
                {
                    //需要额外给id赋值
                    var entity = (SysMenu)FormBuilder1.Entity;
                    entity.Id = id;
                    entity.Update();
                    Response.Write("<script>parent.Message.show('修改成功','提示');parent.TreeGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();
                }
            }
        }
    }
}