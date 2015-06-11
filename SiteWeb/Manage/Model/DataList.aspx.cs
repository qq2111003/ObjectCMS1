using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserControls.Controls;
using ObjectCMS.Common;
using ObjectCMS.BLL;
using ObjectCMS.Model.ModelConfig;
using System.Data;
using UserControls.Controls.jeasyui;
using ObjectCMS.TemplateEngine;

namespace SiteWeb.Manage.Model
{
    public partial class DataList : PageBase
    {

        protected int nodeId = 0;
        Node node = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["nodeid"], out nodeId))
            {
                throw new Exception("参数错误");
            }
            this.Visible = HasPermission(nodeId + 10000, "view");
            node = Node.GetOne(nodeId);
            List<NodeUserModelField> lnumf = NodeUserModelField.GetALL("NodeId=" + nodeId, "Sort");
            List<UserModelField> lumf = UserModelField.GetALL("UserModelId=" + node.UserModelId, "Id");
            List<UserModelFieldType> lumft = UserModelFieldType.GetALL("1=1", "Id");
            var ShowInListField = from f in lnumf
                                  from g in lumf
                                  from h in lumft
                                  where f.UserModelFieldId == g.Id && f.ShowInList == true && g.FieldTypeId == h.Id
                                  select new Column()
                                  {
                                      Name = f.ReWriteTitle,
                                      FieldName = g.FieldName,
                                      Width = f.ListWidth,
                                      Sortable = true,
                                      Align = Algin.left,
                                      FuncFormater = (row, cell, i) =>
                                      {
                                          return FieldTypeForReadOnly.GetView((FieldTypeInHTML)Enum.Parse(typeof(FieldTypeInHTML), h.TypeName), cell);
                                      }
                                  };

            string tableName = UserModel.GetOne(node.UserModelId).TableName;
            string fields = string.Join(",", (from f in lnumf
                                              from g in lumf
                                              where f.UserModelFieldId == g.Id && f.ShowInList == true
                                              select "[" + g.FieldName + "]").ToArray());
            fields += ",[Sort],[CreateTime],[UpdateTime],[LastBuildTime],[Enable]";
            string dataMarkFields = string.Join(",", (from s in DataMark.GetALL("1=1", "Id")
                                                      select "[" + s.MarkName + "]").ToArray());
            if (dataMarkFields.Count() > 0)
            {
                fields += "," + dataMarkFields;
            }

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

                return ModelManage.Instance.DataList(PageIndex, PageSize, "[Id]," + fields, tableName, "NodeId=" + nodeId, "Sort DESC", out recordCount);
                #endregion
            };

            var ctrls = new List<UserControls.Controls.Control>();
            ctrls.Add(new UserControls.Controls.Control()
            {
                ID = "btn_Enable",
                CType = CtrlType.ServerButton,
                Text = "启用",
                IsValidateSelections = true,
                Visible = HasPermission(nodeId + 10000, "Enable"),
                Handler = (selectedIds) =>
                {
                    foreach (var item in selectedIds)
                    {
                        var p = new Dictionary<string, object>();
                        p.Add("Id", item.ToInt());
                        p.Add("Enable", true);
                        ModelManage.Instance.DataUpdate(tableName, p);
                    }
                    return new ServerBtnResult() { Msg = "设置成功!" };
                }
            });
            ctrls.Add(new UserControls.Controls.Control()
            {
                ID = "btn_unEnable",
                CType = CtrlType.ServerButton,
                Text = "不启用",
                IsValidateSelections = true,
                Visible = HasPermission(nodeId + 10000, "Enable"),
                Handler = (selectedIds) =>
                {
                    foreach (var item in selectedIds)
                    {
                        var p = new Dictionary<string, object>();
                        p.Add("Id", item.ToInt());
                        p.Add("Enable", false);
                        ModelManage.Instance.DataUpdate(tableName, p);
                    }
                    return new ServerBtnResult() { Msg = "设置成功!" };
                }
            });
            foreach (var m in DataMark.GetALL("1=1", "Id desc"))
            {
                string markName = m.MarkName;
                string markTitle = m.Title;
                ctrls.Add(new UserControls.Controls.Control()
                {
                    ID = "btn_" + markName,
                    CType = CtrlType.ServerButton,
                    Text = markTitle,
                    IsValidateSelections = true,
                    Visible = HasPermission(nodeId + 10000, markName),
                    Handler = (selectedIds) =>
                    {
                        foreach (var item in selectedIds)
                        {
                            var p = new Dictionary<string, object>();
                            p.Add("Id", item.ToInt());
                            p.Add(markName, true);
                            ModelManage.Instance.DataUpdate(tableName, p);
                        }
                        return new ServerBtnResult() { Msg = "设置成功!" };
                    }
                });
                ctrls.Add(new UserControls.Controls.Control()
                {
                    ID = "btn_un" + markName,
                    CType = CtrlType.ServerButton,
                    Text = "取消" + markTitle,
                    IsValidateSelections = true,
                    Visible = HasPermission(nodeId + 10000, markName),
                    Handler = (selectedIds) =>
                    {
                        foreach (var item in selectedIds)
                        {
                            var p = new Dictionary<string, object>();
                            p.Add("Id", item.ToInt());
                            p.Add(markName, false);
                            ModelManage.Instance.DataUpdate(tableName, p);
                        }
                        return new ServerBtnResult() { Msg = "设置成功!" };
                    }
                });
            }

            DataGrid1.Controls = new List<UserControls.Controls.Control>()
            { 
                new UserControls.Controls.Control(){
                    ID = "btn_add",
                    CType=CtrlType.JsButton,
                    IconCls = ICO.add,
                    Text="添加",
                    Visible = HasPermission(nodeId + 10000, "Add"),
                    Jscript="OpenWindow('添加', 'DataEdit.aspx?NodeId="+nodeId+"',840,500)"
                },             
                new UserControls.Controls.Control(){                    
                    ID = "btn_del",
                    CType=CtrlType.ServerButton,
                    IconCls = ICO.cancel,
                    Text="删除",
                    Visible = HasPermission(nodeId + 10000, "Del"),
                    Confirmation = "确认删除所选项？",
                    IsValidateSelections = true,
                    //参数是选中的行的id数组
                    Handler = (selectedIds)=>{
                        //删除操作
                        try
                        {                            
                            for (int i = 0; i < selectedIds.Length; i++)
                            {
                                ModelManage.Instance.DataDel(selectedIds[i].ToInt(),tableName);
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
                    ID = "btn_build",
                    CType=CtrlType.ServerButton,
                    IconCls = ICO.save,
                    Text="生成栏目",
                    Visible = HasPermission(nodeId + 10000, "Build"),
                    Handler = (selectedIds)=>{
                        
                        BuildNode.BuildListPage(nodeId,0);
                        return new ServerBtnResult() { Msg = "生成成功" };
                    }
                },
                new UserControls.Controls.Control(){
                    CType=CtrlType.JsButton,
                    IconCls = ICO.reload,
                    Text="刷新",
                    Jscript = "$('#"+this.DataGrid1.ClientID+"_datagrid').datagrid('reload');"
                }
            };
            DataGrid1.Controls.AddRange(ctrls);
            //ID,多选框
            DataGrid1.Columns = new List<Column>()
            {                
                new Column(){Name="#",FieldName="Id",Width=20,Sortable=true},
                new Column(){IsCheckbox=true,FieldName="ck",Width=20,Align=Algin.right}
            };
            //数据主体
            DataGrid1.Columns.AddRange(ShowInListField);
            //数据标记
            DataGrid1.Columns.Add(new Column()
            {
                Name = "属性",
                FieldName = "attr",
                Width = 50,
                Align = Algin.left,
                FuncFormater = (row, cell, i) =>
                {
                    string editButton = "";
                    var allDataMark = DataMark.GetALL("1=1", "Id");
                    foreach (var item in allDataMark)
                    {
                        if (((DataRow)row)[item.MarkName].ToString() == "True")
                        {
                            editButton += "<img src='" + item.Ico + "' alt='" + item.Title + "' title='" + item.Title + "' width='24px' border='0'/>";
                        }
                    }
                    return editButton;
                }
            });
            DataGrid1.Columns.Add(new Column()
            {
                Name = "状态",
                FieldName = "Enable",
                Width = 20,
                Align = Algin.left,
                FuncFormater = (row, cell, i) =>
                {

                    string editButton = "<img src='../Images/" + (cell.ToString() == "True" ? "yes" : "no") + ".png' alt='" + (cell.ToString() == "True" ? "启用" : "未启用") + "' title='" + (cell.ToString() == "True" ? "启用" : "未启用") + "' width='24px' border='0'/>";

                    return editButton;
                }
            });

            DataGrid1.Columns.Add(new Column()
            {
                Name = "排序",
                FieldName = "Sort",
                FuncFormater = (row, cell, i) =>
                {
                    return cell != null ? "<input type=\"text\" value=\"" + cell.ToString() + "\" style=\"width:30px;border: 1px solid #CCCCCC;\" onkeyup=\"if(!Number(this.value)&&this.value!=0)this.value='';\" onchange=\"ChangeSort(this);\" dataid=\"" + ((DataRow)row)["Id"] + "\">" : "";
                },
                Width = 20
            });
            //操作
            DataGrid1.Columns.Add(new Column()
            {
                Name = "操作",
                FieldName = "opreate",
                Width = 30,
                FuncFormater = (row, cell, i) =>
                {
                    string editButton = "";
                    if (HasPermission(nodeId + 10000, "Edit"))
                    {
                        editButton += "<a href=\"javascript:;\" class=\"easyui-linkbutton l-btn l-btn-plain\" onclick=\"OpenWindow('修改', 'DataEdit.aspx?nodeid=" + nodeId + "&id=" + ((DataRow)row)["Id"] + "',840,500);\"><span class=\"l-btn-left\"><span class=\"l-btn-text icon-edit l-btn-icon-left\">修改</span></span></a>";
                    }
                    return editButton;
                }
            });

            //设置初始排序字段 可选
            DataGrid1.SortField = "Id";
            DataGrid1.SortOrder = "desc";
        }
    }
}