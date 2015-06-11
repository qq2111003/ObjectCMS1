using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.System;
using UserControls.Controls;

namespace SiteWeb.Manage.Site
{
    public partial class SiteManage : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(20, "view");
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

                return SysSite.Pager(PageIndex, PageSize, "1=1", orderBy, out recordCount);
                #endregion
            };

            DataGrid1.Controls = new List<UserControls.Controls.Control>()
            { 
                new UserControls.Controls.Control(){
                    ID = "btn_add",
                    CType=CtrlType.JsButton,
                    IconCls = ICO.add,
                    Text="添加",
                    Visible = this.HasPermission(20, "add"),
                    Jscript="OpenWindow('添加站点', 'SiteEdit.aspx',500,320)"
                },             
                new UserControls.Controls.Control(){
                    ID = "btn_del",
                    CType=CtrlType.ServerButton,
                    IconCls = ICO.cancel,
                    Text="删除",
                    Confirmation = "确认删除所选项？",
                    IsValidateSelections = true,
                    Visible = this.HasPermission(20, "del"),
                    //参数是选中的行的id数组
                    Handler = (selectedIds)=>{
                        //删除操作
                        try
                        {
                            for (int i = 0; i < selectedIds.Length; i++)
                            {
                                SysSite.DeleteByWhere("Id=" + int.Parse(selectedIds[i]));
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
                    ID = "btn_setdefault",
                    CType=CtrlType.ServerButton,
                    Text="删除",
                    Visible = false,
                    //参数是选中的行的id数组
                    Handler = (selectedIds)=>{
                        //删除操作
                        try
                        {                            
                            
                            
                            return new ServerBtnResult() { Msg = "设置成功" };
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
                
                new Column(){Name="#",FieldName="Id",Width=20},
                new Column(){IsCheckbox=true,FieldName="ck",Width=20,Align=Algin.right},
                new Column(){Name="站名",FieldName="Name",FuncFormater=(row,cell,i)=>{
                    return cell!=null?cell.ToString():"";
                },Align= Algin.left},
                new Column(){Name="站点标识",FieldName="SiteMark",FuncFormater=(row,cell,i)=>{
                    return cell!=null?cell.ToString():"";
                },Width=50,Align= Algin.left},
                new Column(){Name="操作",FieldName="opreate",Width=30,FuncFormater=(row,cell,i)=>{
                   string editButton = "";
                   if (this.HasPermission(20, "edit"))
                   {
                       editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('修改', 'SiteEdit.aspx?id=" + ((SysSite)row).Id + "',500,320);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">修改</span></span></a>";
                   }
                   if (this.HasPermission(20, "setdefault"))
                   {
                       editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('设为默认', 'SiteEdit.aspx?id=" + ((SysSite)row).Id + "',500,320);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">设为默认</span></span></a>";
                   }
                   if (this.HasPermission(20, "copysite"))
                   {
                       editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('复制站点', 'SiteCopy.aspx?id=" + ((SysSite)row).Id + "',500,320);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">复制站点</span></span></a>";
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