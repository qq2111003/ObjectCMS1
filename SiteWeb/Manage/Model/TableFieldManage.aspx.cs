using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserControls.Controls;
using ObjectCMS.Model.ModelConfig;
using ObjectCMS.BLL;

namespace SiteWeb.Manage.Model
{
    public partial class TableFieldManage : PageBase
    {

        int userModelId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(11, "fieldset");
            int.TryParse(Request.QueryString["id"], out userModelId);

            if (userModelId <= 0)
            {
                return;
            }
            DataGrid1.DataSource = (ref int recordCount) =>
            {
                return UserModelField.GetALL("UserModelId=" + userModelId, "id");
            };
            DataGrid1.Controls = new List<UserControls.Controls.Control>()
            { 
                new UserControls.Controls.Control(){
                    ID = "btn_add",
                    CType=CtrlType.JsButton,
                    IconCls = ICO.add,
                    Text="添加",
                    Jscript="OpenWindow('添加按钮', 'TableFieldEdit.aspx?UserModelId="+userModelId+"',300,400)"
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
                                ModelManage.Instance.DelColumn(int.Parse(selectedIds[i]));
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
                new Column(){Name="列名",FieldName="FieldName"},
                new Column(){Name="名称",FieldName="Title"},
                new Column(){Name="类型",FieldName="FieldTypeId",FuncFormater=(row,cell,i)=>{
                    return UserModelFieldType.GetOne(Convert.ToInt32(cell)).TypeName;
                }},
                new Column(){Name="排序",FieldName="Sort",Width=50,FuncFormater=(row,cell,i)=>{
                   string editButton = "";
                   editButton += "上移|下移";
                   return editButton;
                }}
            };

            DataGrid1.Pagination = false;
        }
    }
}