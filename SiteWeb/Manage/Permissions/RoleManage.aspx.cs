using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.System;
using UserControls.Controls;

namespace SiteWeb.Manage.Permissions
{
    public partial class RoleManage : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(4, "view");
            DataGrid1.DataSource = (ref int recordCount) =>
            {
                #region 查询方法
                int PageSize = 10;
                int PageIndex = 1;
                string orderBy = "ID";
                if (!string.IsNullOrEmpty(Request.Form["rows"]))
                {
                    int.TryParse(Request.Form["rows"], out PageSize);
                }
                if (!string.IsNullOrEmpty(Request.Form["page"]))
                {
                    int.TryParse(Request.Form["page"], out PageIndex);
                }
                if (!string.IsNullOrEmpty(Request["sort"]))
                {
                    orderBy = Request["sort"] + " " + Request["order"];
                }

                return SysRoles.Pager(PageIndex, PageSize, "1=1", orderBy, out recordCount);
                #endregion
            };

            DataGrid1.Controls = new List<UserControls.Controls.Control>()
            { 
                new UserControls.Controls.Control(){
                    ID = "btn_add",
                    CType=CtrlType.JsButton,
                    IconCls = ICO.add,
                    Text="添加",
                    Visible = this.HasPermission(4, "add"),
                    Jscript="OpenWindow('添加角色', 'RoleEdit.aspx',500,320)"
                },                
                new UserControls.Controls.Control(){                    
                    ID = "btn_del",
                    CType=CtrlType.ServerButton,
                    IconCls = ICO.cancel,
                    Text="删除",
                    Visible = this.HasPermission(4, "del"),
                    Confirmation = "确认删除所选项？",
                    IsValidateSelections = true,
                    //参数是选中的行的id数组
                    Handler = (selectedIds)=>{
                        try
                        {
                            //删除操作
                            for (int i = 0; i < selectedIds.Length; i++)
                            {
                                SysRoles.DeleteByWhere("Id=" + int.Parse(selectedIds[i]));
                            }
                            return new ServerBtnResult() { Msg = "删除成功" };
                        }
                        catch (Exception ex)
                        {
                            return new ServerBtnResult() { Msg = ex.Message };
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
                new Column(){Name="#",FieldName="Id",Width=10},

                new Column(){IsCheckbox=true,FieldName="ck",Width=50,Align=Algin.right},
                new Column(){Name="名称",FieldName="Name",FuncFormater=(row,cell,i)=>{
                    return ""+cell.ToString()+"";
                },Width=150,Align= Algin.left},
                new Column(){Name="状态",FieldName="Enable",FuncFormater=(row,cell,i)=>{
                    return "<a href=\"javascript:;\"  onclick=\"ChangeStatus(this);\" data=\"" + ((SysRoles)row).Id + "\"><img src=\"../images/" + (Convert.ToBoolean(cell) ? "yes" : "no") + ".gif\" /></a>";
                },Width=150},
                new Column(){Name="操作",FieldName="opreate",Width=50,FuncFormater=(row,cell,i)=>{
                    
                    string editButton = "";
                    if( this.HasPermission(4, "edit")){
                        editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('修改', 'RoleEdit.aspx?id=" + ((SysRoles)row).Id + "',500,320);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">修改</span></span></a>";
                    }
                    if( this.HasPermission(4, "setmenu")){
                        editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('设置栏目权限', 'RoleSetMenu.aspx?id=" + ((SysRoles)row).Id + "',950,600);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">设置栏目权限</span></span></a>";                    
                    }
                    return editButton;
                }}
            }; 

            //设置初始排序字段 可选
            DataGrid1.SortField = "Id";
            DataGrid1.SortOrder = "desc";
        }
    }
}