using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.ModelConfig;
using UserControls.Controls;

namespace SiteWeb.Manage.Model
{
    public partial class TableManage : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(11, "view");
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

                return UserModel.Pager(PageIndex, PageSize, "1=1", orderBy, out recordCount);
                #endregion
            };

            DataGrid1.Controls = new List<UserControls.Controls.Control>()
            { 
                new UserControls.Controls.Control(){
                    ID = "btn_add",
                    CType=CtrlType.JsButton,
                    IconCls = ICO.add,
                    Text="添加",
                    Visible = this.HasPermission(11, "add"),
                    Jscript="OpenWindow('添加模型', 'TableEdit.aspx',500,320)"
                },             
                new UserControls.Controls.Control(){                    
                    ID = "btn_del",
                    CType=CtrlType.ServerButton,
                    IconCls = ICO.cancel,
                    Text="删除",
                    Visible = this.HasPermission(11, "del"),
                    Confirmation = "确认删除所选项？",
                    IsValidateSelections = true,
                    //参数是选中的行的id数组
                    Handler = (selectedIds)=>{
                        //删除操作
                        try
                        {                            
                            for (int i = 0; i < selectedIds.Length; i++)
                            {
                                UserModel.DeleteByWhere("Id=" + int.Parse(selectedIds[i]));
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
                new Column(){Name="模型名称",FieldName="TableName",Align= Algin.left},
                new Column(){Name="操作",FieldName="opreate",Width=30,FuncFormater=(row,cell,i)=>{
                   string editButton = "";
                    
                    if( this.HasPermission(11, "edit")){
                        editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('修改模型', 'TableEdit.aspx?id=" + ((UserModel)row).Id + "',500,320);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">修改</span></span></a>";
                    }
                    
                    if( this.HasPermission(11, "fieldset")){
                        editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('字段管理', 'TableFieldManage.aspx?id=" + ((UserModel)row).Id + "',500,500);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">字段管理</span></span></a>";
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