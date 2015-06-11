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
    public partial class MenuManage : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(2, "view");
            TreeGrid1.DataSource = (ref int recordCount) =>
            {
                return SysMenu.GetALL("1=1", "id");
            };
            //设置按钮  可选
            TreeGrid1.Controls = new List<UserControls.Controls.Control>()
            {       
                new UserControls.Controls.Control(){
                    ID = "btn_add",
                    CType=CtrlType.JsButton,
                    IconCls = ICO.add,
                    Text="添加",
                    Visible = this.HasPermission(2, "add"),
                    Jscript="OpenWindow('添加菜单', 'MenuEdit.aspx',500,320)"
                },            
                new UserControls.Controls.Control(){                    
                    ID = "btn_del",
                    CType=CtrlType.ServerButton,
                    IconCls = ICO.cancel,
                    Text="删除",
                    Visible = this.HasPermission(2, "del"),
                    //参数是选中的行的id数组
                    Handler = (selectedIds)=>{
                        //删除操作
                        //if (ChannelManage.Del(selectedIds)>0)
                        //{
                        //    return  new ServerBtnResult(){Msg="删除成功"};
                        //}
                        return  new ServerBtnResult(){Msg="删除失败"};
                    }
                },
                new UserControls.Controls.Control(){
                    ID = "collapseAll",
                    CType=CtrlType.JsButton,
                    Text="收起",
                    Jscript="$('#"+TreeGrid1.ClientID+"_treegrid').treegrid('collapseAll');"
                },
                new UserControls.Controls.Control(){
                    ID = "expandAll",
                    CType=CtrlType.JsButton,
                    Text="展开",
                    Jscript="$('#"+TreeGrid1.ClientID+"_treegrid').treegrid('expandAll');"
                },
            };
            //设置各显示列  必填
            TreeGrid1.Columns = new List<Column>()
            {
                new Column(){Name="#",FieldName="Id",Width=20},
                new Column(){IsCheckbox=true,FieldName="ck",Width=20,Align=Algin.right},
                new Column(){Name="标题",FieldName="Title",Align=Algin.left},
                new Column(){Name="地址",FieldName="Url",Align=Algin.left},
                new Column(){Name="状态",FieldName="Enable",Width=20,FuncFormater=(row,cell,i)=>{
                    return "<a href=\"javascript:;\"  onclick=\"ChangeStatus(this);\" data=\"" + ((SysMenu)row).Id + "\"><img src=\"../images/" + (Convert.ToBoolean(cell) ? "yes" : "no") + ".gif\" /></a>";
                }},
                new Column(){Name="操作",FieldName="opreate",Width=30,FuncFormater=(row,cell,i)=>{
                    
                    string editButton = "";

                    if (this.HasPermission(2, "edit"))
                    {
                        editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('修改', 'MenuEdit.aspx?id=" + ((SysMenu)row).Id + "',500,320);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">修改</span></span></a>";
                    }

                    if (this.HasPermission(2, "setcontrol"))
                    {
                        editButton += "<a href=\"javascript:;\" onclick=\"OpenWindow('设置按钮', 'MenuControlsManage.aspx?id=" + ((SysMenu)row).Id + "',500,500);\" class=\"easyui-linkbutton l-btn l-btn-plain\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">设置按钮</span></span></a>";
                    
                    }
                    return editButton;
                }}
            };

            TreeGrid1.TreeField = "Title";
        }
    }
}