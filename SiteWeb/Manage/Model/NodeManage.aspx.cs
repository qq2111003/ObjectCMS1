using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.ModelConfig;
using ObjectCMS.Common;
using UserControls.Controls;
using ObjectCMS.TemplateEngine;

namespace SiteWeb.Manage.Model
{
    public partial class NodeManage : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(10, "view");
            TreeGrid1.DataSource = (ref int recordCount) =>
            {
                return Node.GetALL("1=1", "id");
            };
            //设置按钮  可选
            TreeGrid1.Controls = new List<UserControls.Controls.Control>()
            {       
                new UserControls.Controls.Control(){
                    ID = "btn_add",
                    CType=CtrlType.JsButton,
                    IconCls = ICO.add,
                    Text="添加",
                    Visible = this.HasPermission(10, "add"),
                    Jscript="OpenWindow('添加节点', 'NodeEdit.aspx',500,320)"
                },            
                new UserControls.Controls.Control(){                    
                    ID = "btn_del",
                    CType=CtrlType.ServerButton,
                    IconCls = ICO.cancel,
                    Text="删除",
                    Confirmation = "确认删除?",
                    IsValidateSelections= true,
                    Visible = this.HasPermission(10, "del"),
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
                    ID = "btn_Build",
                    CType=CtrlType.JsButton,
                    Text="生成页面",
                    Visible = this.HasPermission(10, "buildhtml"),
                    //参数是选中的行的id数组
                    Jscript="openBuildAllWindow();"
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
                new UserControls.Controls.Control(){
                    ID = "btn_weixinMenu",
                    CType=CtrlType.JsButton,
                    IconCls = ICO.reload,
                    Text="更新微信菜单",
                    Visible = this.HasPermission(10, "add"),
                    Jscript="OpenWindow('更新微信菜单', '../Weixin/SetWeixinMenu.aspx',500,320)"
                }
            };
            //设置各显示列  必填
            TreeGrid1.Columns = new List<Column>()
            {
                new Column(){Name="#",FieldName="Id",Width=20},
                new Column(){IsCheckbox=true,FieldName="ck",Width=20,Align=Algin.right},
                new Column(){Name="标题",FieldName="Title",Align=Algin.left},
                new Column(){Name="用户模型",FieldName="UserModelId",Align=Algin.left,FuncFormater=(row,cell,i)=>{
                    return UserModel.GetOne(cell.ToInt()).Title;
                }},
                new Column(){Name="页面类型",FieldName="PageType",Align=Algin.left,FuncFormater=(row,cell,i)=>{
                    List<NodeUserModelField> lnumf = NodeUserModelField.GetALL("NodeId=" + ((Node)row).Id, "Sort");

                    return "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" " + (lnumf.Count > 0?"onclick=\"parent.addTab('" + ((Node)row).Title + "','Model/" + (Convert.ToBoolean(cell) ? "DataList" : "NodeDataEdit") + ".aspx?NodeId=" + ((Node)row).Id + "')\"":"") + "><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">" + (Convert.ToBoolean(cell) ? "列表页" : "单篇\\综合页") + "</span></span></a>";
                    
                }},
                new Column(){Name="操作",FieldName="opreate",Width=100,FuncFormater=(row,cell,i)=>{
                    string editButton = "";
                    if(this.HasPermission(10, "edit")){
                        editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('修改', 'NodeEdit.aspx?id=" + ((Node)row).Id + "',500,320);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">修改</span></span></a>";
                    }
                    
                    if(this.HasPermission(10, "fieldset")){
                        editButton += "<a href=\"javascript:;\" onclick=\"OpenWindow('字段设置', 'NodeSetField.aspx?id=" + ((Node)row).Id + "',906,500);\" class=\"easyui-linkbutton l-btn l-btn-plain\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">字段设置</span></span></a>";
                    }
                    
                    if(this.HasPermission(10, "staticset")){
                        editButton += "<a href=\"javascript:;\" onclick=\"OpenWindow('静态化设置', 'NodeSetTemplate.aspx?id=" + ((Node)row).Id + "',632,500);\" class=\"easyui-linkbutton l-btn l-btn-plain\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">静态化设置</span></span></a>";
                    }
                    
                    if(this.HasPermission(10, "buildhtml")){
                        if (!string.IsNullOrEmpty(((Node)row).DetailPageUrl) || !string.IsNullOrEmpty(((Node)row).ListPageUrl))
                        {
                            editButton += "<a href=\"javascript:;\" onclick=\"UIDialog.Init('dobuild', '生成进度', 450, 120,true).Open('BuildHtml.aspx?nodeid=" + ((Node)row).Id + "');\" class=\"easyui-linkbutton l-btn l-btn-plain\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">生成页面</span></span></a>";

                        }
                    }
                    return editButton;
                }}
            };
            TreeGrid1.TreeField = "Title";
        }
    }
}