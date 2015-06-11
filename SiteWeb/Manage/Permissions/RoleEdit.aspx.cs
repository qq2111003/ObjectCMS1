using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserControls.Controls.jeasyui;
using ObjectCMS.Model.System;

namespace SiteWeb.Manage.Permissions
{
    public partial class RoleEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);

            //设置form初始值 
            if (id == 0)
            {
                this.Visible = this.HasPermission(4, "add");
                FormBuilder1.Entity = new SysRoles() { Enable = true };
            }
            else
            {
                this.Visible = this.HasPermission(4, "edit");
                FormBuilder1.Entity = SysRoles.GetOne(id);
            }

            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = new List<FormItem>()
            {
                new FormItem(){Title = "角色名称", FieldName = "Name", FieldType =  FieldTypeInHTML.SingleLine, 
                    FieldModel= new ModelSingleLine(){ValidateTypes=new List<FieldValidate>(){FieldValidate.required}}},
            };
            #endregion

            if (IsPostBack)
            {
                if (id == 0)                //添加模式
                {
                    var entity = (SysRoles)FormBuilder1.Entity;
                    entity.Insert();
                    Response.Write("<script>parent.Message.show('添加成功','提示');parent.DataGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();
                }
                else                        //修改模式
                {
                    //需要额外给id赋值
                    var entity = (SysRoles)FormBuilder1.Entity;
                    entity.Id = id;
                    entity.Update();
                    Response.Write("<script>parent.Message.show('修改成功','提示');parent.DataGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();
                }
            }
        }
    }
}