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
    public partial class MenuControlsEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(2, "setcontrol");
            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);
            int menuId = Convert.ToInt32(Request["MenuId"]);

            //设置form初始值 
            if (id == 0)
            {
                FormBuilder1.Entity = new SysMenuControls() { MenuId = menuId };
            }
            else
            {
                FormBuilder1.Entity = SysMenuControls.GetOne(id);
            }

            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = new List<FormItem>()
            {
                new FormItem(){Title = "名　　称", FieldName = "Name", FieldType =  FieldTypeInHTML.SingleLine, 
                    FieldModel= new ModelSingleLine(){ValidateTypes=new List<FieldValidate>(){FieldValidate.required}}},
                new FormItem(){Title = "CtrlID", FieldName = "CtrlID", FieldType = FieldTypeInHTML.SingleLine}
            };
            #endregion

            if (IsPostBack)
            {
                if (id == 0)                //添加模式
                {
                    var entity = (SysMenuControls)FormBuilder1.Entity;
                    entity.Insert();
                    Response.Write("<script>parent.Message.show('添加成功','提示');parent.DataGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();
                }
                else                        //修改模式
                {
                    //需要额外给id赋值
                    var entity = (SysMenuControls)FormBuilder1.Entity;
                    entity.Id = id;
                    entity.Update();
                    Response.Write("<script>parent.Message.show('修改成功','提示');parent.DataGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();
                }
            }
        }
    }
}