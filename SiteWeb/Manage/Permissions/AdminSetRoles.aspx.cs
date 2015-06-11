using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserControls.Controls;
using ObjectCMS.Model.System;
using ObjectCMS.BLL;
using System.Data;

namespace SiteWeb.Manage.Permissions
{
    public partial class AdminSetRoles : PageBase
    {
        int adminId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(5, "setroles");
            if (!int.TryParse(Request.QueryString["id"], out adminId))
            {

            }

            DataGrid1.DataSource = (ref int recordCount) =>
            {
                #region 查询方法
                return PermissionsManage.Instance.GetAllRole(adminId);
                #endregion
            };

            DataGrid1.Pagination = false;
            DataGrid1.Controls = new List<UserControls.Controls.Control>()
            { 
                new UserControls.Controls.Control(){
                    ID = "btn_add",
                    CType=CtrlType.ServerButton,
                    IconCls = ICO.add,
                    Text="设置角色",
                    Visible=false,
                    Handler=(ids)=>{
                        SysAdminRoles.DeleteByWhere("AdminId="+adminId);
                        for (int i = 0; i < ids.Length; i++)
                        {
                            int roleid = 0;
                            int.TryParse(ids[i],out roleid);
                            if(roleid>0){                                
                                SysAdminRoles sysAR = new SysAdminRoles();
                                sysAR.AdminId = adminId;
                                sysAR.RoleId = roleid;
                                sysAR.Insert();
                            }
                        }
                        return new ServerBtnResult() { Jscript = "parent.Message.show('修改成功','提示');parent.UIDialog.Close();" };
                    }
                }
            };
            DataGrid1.Columns = new List<Column>()
            {
                new Column(){Name="#",FieldName="Id",Width=10},
                new Column(){Name="名称",FieldName="Name",FuncFormater=(row,cell,i)=>{
                    return "" + cell.ToString() + (((DataRow)row)["flag"].ToString()=="1"?"<script>$('#" + DataGrid1.ClientID + "_datagrid').datagrid('selectRecord'," + ((DataRow)row)["Id"] + ");</script>":"") ;
                },Width=150,Align= Algin.left},

                new Column(){IsCheckbox=true,FieldName="ck",Width=50,Align=Algin.right}
            };

            //设置初始排序字段 可选
            DataGrid1.SortField = "Id";
            DataGrid1.SortOrder = "ASC";

        }
    }
}