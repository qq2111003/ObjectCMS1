using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.System;
using UserControls.Controls;

namespace SiteWeb.Manage.Menu
{
    public partial class MenuControlsManage : PageBase
    {
        int menuId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(2, "setcontrol");
            int.TryParse(Request.QueryString["id"], out menuId);

            if (menuId <= 0)
            {
                return;
            }
            DataGrid1.DataSource = (ref int recordCount) =>
            {
                return SysMenuControls.GetALL("MenuId=" + menuId, "id");
            };
            DataGrid1.Controls = new List<UserControls.Controls.Control>()
            { 
                new UserControls.Controls.Control(){
                    ID = "btn_add",
                    CType=CtrlType.JsButton,
                    IconCls = ICO.add,
                    Text="添加",
                    Jscript="OpenWindow('添加按钮', 'MenuControlsEdit.aspx?MenuId="+menuId+"',300,250)"
                },                 
                new UserControls.Controls.Control(){
                    ID = "btn_del",
                    CType=CtrlType.ServerButton,
                    IconCls = ICO.cancel,
                    Text="删除",
                    Confirmation = "确认删除所选项？",
                    IsValidateSelections = true,
                    //参数是选中的行的id数组
                    Handler = (selectedIds)=>{
                        try{
                            //删除操作
                            for (int i = 0; i < selectedIds.Length; i++)
                            {
                                SysMenuControls.DeleteByWhere("Id="+int.Parse( selectedIds[i]));
                            }                        
                            return  new ServerBtnResult(){Msg="删除成功"};
                        }
                        catch (Exception ex)
                        {
                            return  new ServerBtnResult(){Msg=ex.Message};
                        }
                    }
                },
                new UserControls.Controls.Control(){
                    CType=CtrlType.JsButton,
                    IconCls = ICO.reload,
                    Text="刷新",
                    Jscript = "$('#"+this.DataGrid1.ClientID+"_datagrid').datagrid('reload');"
                }
            };
            DataGrid1.Columns = new List<Column>()
            {
                new Column(){Name="#",FieldName="Id",Width=20},
                new Column(){IsCheckbox=true,FieldName="ck",Width=20,Align=Algin.right},
                new Column(){Name="名称",FieldName="Name"},
                new Column(){Name="CtrlId",FieldName="CtrlID"},
                new Column(){Name="操作",FieldName="opreate",Width=50,FuncFormater=(row,cell,i)=>{
                   string editButton = "";
                   editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('修改', 'MenuControlsEdit.aspx?MenuId=" + menuId + "&id=" + ((SysMenuControls)row).Id + "',500,320);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">修改</span></span></a>";
                   editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-cancel l-btn-icon-left\">删除</span></span></a>";
                    
                    return editButton;
                }}
            };

            DataGrid1.Pagination = false;
        }
    }
}