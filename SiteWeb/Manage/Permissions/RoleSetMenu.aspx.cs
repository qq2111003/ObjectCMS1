using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.System;
using UserControls.Controls;
using ObjectCMS.BLL;
using System.Data;
using ObjectCMS.Model.ModelConfig;

namespace SiteWeb.Manage.Permissions
{
    public partial class RoleSetMenu : PageBase
    {
        int roleId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(4, "setmenu");
            int.TryParse(Request.QueryString["id"], out roleId);
            if (!string.IsNullOrEmpty(Request.Form["method"]))
            {
                this.GetType().GetMethod(Request.Form["method"]).Invoke(this, null);
            }
            GridConfig();
        }

        protected void GridConfig()
        {
            TreeGrid1.DataSource = (ref int recordCount) =>
            {
                return PermissionsManage.Instance.GetAllMenus(roleId);
            };

            //设置各显示列  必填
            TreeGrid1.Columns = new List<Column>()
            {
                new Column(){Name="#",FieldName="Id",Width=20},
                new Column(){Name="标题",FieldName="Title",Align=Algin.left},
                new Column(){Name="栏目按钮",FieldName="Url",Align=Algin.left,FuncFormater=(row,cell,i)=>{
                    int itemId = Convert.ToInt32(((DataRow)row)["Id"]);

                    string allCtrlRadio = "";
                    if (itemId > 10000 && Convert.ToInt32(((DataRow)row)["ParentId"])!=16)
                    {
                        DataTable dt = PermissionsManage.Instance.GetAllNodeControls(roleId, itemId);

                        allCtrlRadio += "<input type=\"checkbox\" name=\"ctrlid\" id=\"ctrl" + itemId + "_Add" + "\" value=\"" + itemId + "_Add\"  " + (dt.AsEnumerable().Where(f => f.Field<string>("NodeControlMark") == itemId + "_Add").Count() > 0 ? "checked=\"checked\"" : "") + " /><label for=\"ctrl" + itemId + "_Add" + "\">添加</label>";
                        allCtrlRadio += "<input type=\"checkbox\" name=\"ctrlid\" id=\"ctrl" + itemId + "_Edit" + "\" value=\"" + itemId + "_Edit\" " + (dt.AsEnumerable().Where(f => f.Field<string>("NodeControlMark") == itemId + "_Edit").Count() > 0 ? "checked=\"checked\"" : "") + "/><label for=\"ctrl" + itemId + "_Edit" + "\">修改</label>";
                        allCtrlRadio += "<input type=\"checkbox\" name=\"ctrlid\" id=\"ctrl" + itemId + "_Del" + "\" value=\"" + itemId + "_Del\" " + (dt.AsEnumerable().Where(f => f.Field<string>("NodeControlMark") == itemId + "_Del").Count() > 0 ? "checked=\"checked\"" : "") + "/><label for=\"ctrl" + itemId + "_Del" + "\">删除</label>";
                        allCtrlRadio += "<input type=\"checkbox\" name=\"ctrlid\" id=\"ctrl" + itemId + "_Build" + "\" value=\"" + itemId + "_Build\" " + (dt.AsEnumerable().Where(f => f.Field<string>("NodeControlMark") == itemId + "_Build").Count() > 0 ? "checked=\"checked\"" : "") + "/><label for=\"ctrl" + itemId + "_Build" + "\">生成栏目页</label>";
                        allCtrlRadio += "<input type=\"checkbox\" name=\"ctrlid\" id=\"ctrl" + itemId + "_Enable" + "\" value=\"" + itemId + "_Enable\" " + (dt.AsEnumerable().Where(f => f.Field<string>("NodeControlMark") == itemId + "_Enable").Count() > 0 ? "checked=\"checked\"" : "") + "/><label for=\"ctrl" + itemId + "_Enable" + "\">启用状态</label>";
                        var allMark = DataMark.GetALL("1=1","Id desc");
                        foreach (var item in allMark)
                        {
                            allCtrlRadio += "<input type=\"checkbox\" name=\"ctrlid\" id=\"ctrl" + itemId + "_" + item.MarkName + "\" value=\"" + itemId + "_" + item.MarkName + "\" " + (dt.AsEnumerable().Where(f => f.Field<string>("NodeControlMark") == itemId + "_" + item.MarkName).Count() > 0 ? "checked=\"checked\"" : "") + "/><label for=\"ctrl" + itemId + "_" + item.MarkName + "" + "\">" + item.Title + "</label>";
                        }
                    }
                    else
                    {
                        DataTable dt = PermissionsManage.Instance.GetAllControls(roleId, itemId);

                        foreach (var item in dt.AsEnumerable())
                        {
                            allCtrlRadio += "<input type=\"checkbox\" name=\"ctrlid\" id=\"ctrl" + ((DataRow)row)["Id"] + "_" + item.Field<int>("Id") + "\" value=\"" + item.Field<int>("Id") + "\" " + (item.Field<int>("flag") == 1 ? "checked=\"checked\"" : "") + "/><label for=\"ctrl" + ((DataRow)row)["Id"] + "_" + item.Field<int>("Id") + "\">" + item.Field<string>("Name") + "</label>";
                        }
                    }
                    return allCtrlRadio;
                }},
                new Column(){FieldName="flag",Name="<input type=\"checkbox\" onchange=\"checkall(this)\">",FuncFormater=(row,cell,i)=>{
                    return "<input type=\"checkbox\" name=\"flag\" value=\"" + ((DataRow)row)["Id"] + "\" " + (cell.ToString() == "1" ? "checked=\"checked\"" : "") + " onchange=\"checkleftall(this," + ((DataRow)row)["Id"] + ")\"/>";                    
                },Width=10,Align=Algin.center}
            };
            TreeGrid1.TreeField = "Title";

        }

        public void SubmitMethod()
        {
            try
            {

                string[] checkedMenuIds = string.IsNullOrEmpty(Request.Form["checkedmenuids"]) ? new string[0] : Request.Form["checkedmenuids"].Split(',');
                string[] controlIds = string.IsNullOrEmpty(Request.Form["ctrlid"]) ? new string[0] : Request.Form["ctrlid"].Split(',');

                #region 系统栏目
                SysRoleMenu.DeleteByWhere("RoleId=" + roleId);
                RoleNode.DeleteByWhere("RoleId=" + roleId);
                for (int i = 0; i < checkedMenuIds.Length; i++)
                {
                    int menuId = 0;
                    int.TryParse(checkedMenuIds[i], out menuId);
                    if (menuId > 0 && menuId < 10000)
                    {
                        new SysRoleMenu()
                        {
                            MenuId = menuId,
                            RoleId = roleId
                        }.Insert();
                    }
                    else if (menuId > 10000)
                    {
                        new RoleNode()
                        {
                            NodeId = menuId - 10000,
                            RoleId = roleId
                        }.Insert();
                    }
                }
                SysRoleControl.DeleteByWhere("RoleId=" + roleId);
                RoleNodeControl.DeleteByWhere("RoleId=" + roleId);
                for (int i = 0; i < controlIds.Length; i++)
                {
                    int ctrlId = 0;
                    int.TryParse(controlIds[i], out ctrlId);
                    if (ctrlId > 0)
                    {
                        new SysRoleControl()
                        {
                            ControlId = ctrlId,
                            RoleId = roleId
                        }.Insert();
                    }
                    else
                    {
                        new RoleNodeControl()
                        {
                            NodeControlMark = controlIds[i],
                            RoleId = roleId
                        }.Insert();
                    }

                }
                #endregion
                Response.Write(1);
            }
            catch (Exception)
            {
                Response.Write(0);
            }
            Response.End();
        }
    }
}