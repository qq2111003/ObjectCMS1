using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Text;
using ObjectCMS.Common;

namespace UserControls.Controls
{
    /// <summary>
    /// 核心类
    /// </summary>
    public partial class DataGrid : System.Web.UI.UserControl
    {
        #region 成员
        public JavaScriptSerializer _jssl = new JavaScriptSerializer();
        #endregion
        #region 属性
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        private string _IdField = "Id";

        public string IdField
        {
            get { return _IdField; }
            set { _IdField = value; }
        }

        public string SortField { get; set; }
        public string SortOrder { get; set; }
        private bool pagination = true;

        public bool Pagination
        {
            get { return pagination; }
            set { pagination = value; }
        }

        /// <summary>
        /// 数据地址
        /// </summary>
        private string _DataUrl;
        public string DataUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_DataUrl))
                {
                    return _DataUrl;
                }
                else
                {
                    string Url = Request.Url.ToString();
                    if (Url.IndexOf('?') > 0)
                    {
                        return Url + "&uc=" + this.ClientID;
                    }
                    return Url + "?uc=" + this.ClientID;
                }
            }
            set { _DataUrl = value; }
        }
        /// <summary>
        /// 数据源 
        /// </summary>
        public delegate object DGDataSource(ref int recountCount);
        public DGDataSource DataSource { get; set; }
        /// <summary>
        /// 数据列
        /// </summary>
        private List<Column> _Columns = new List<Column>();

        public List<Column> Columns
        {
            get { return _Columns; }
            set { _Columns = value; }
        }
        /// <summary>
        /// 工具栏
        /// </summary>
        public List<Control> _Controls = new List<Control>();
        public new List<Control> Controls
        {
            get { return _Controls; }
            set { _Controls = value; }
        }

        private Dictionary<string, string> _Event = new Dictionary<string, string>();

        public Dictionary<string, string> Event
        {
            get { return _Event; }
            set { _Event = value; }
        }

        #endregion
        #region 方法
        /// <summary>
        /// 格式化DataTable数据
        /// </summary>
        public virtual List<Dictionary<string, object>> FormatAll(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
            //格式化后的数据用List<Dictionary<string, string>>存储
            List<Dictionary<string, object>> FormatedData = new List<Dictionary<string, object>>();
            Dictionary<string, object> dic;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dic = new Dictionary<string, object>();
                for (int j = 0; j < Columns.Count; j++)
                {
                    if (dt.Columns.Contains(Columns[j].FieldName))
                    {
                        //如果此列有委托函数 则进行格式化
                        if (Columns[j].FuncFormater != null)
                        {
                            dic.Add(Columns[j].FieldName, Columns[j].FuncFormater(dt.Rows[i], dt.Rows[i][Columns[j].FieldName], i));
                        }
                        else
                        {
                            dic.Add(Columns[j].FieldName, dt.Rows[i][Columns[j].FieldName]);
                        }
                    }
                    else
                    {
                        if (Columns[j].FuncFormater != null)
                        {
                            dic.Add(Columns[j].FieldName, Columns[j].FuncFormater(dt.Rows[i], null, i));
                        }
                        else
                        {
                            dic.Add(Columns[j].FieldName, null);
                        }
                    }
                }
                FormatedData.Add(dic);
            }
            return FormatedData;
        }
        /// <summary>
        /// 格式化List<Entity>数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual List<Dictionary<string, object>> FormatAll(IList list)
        {
            if (list == null || list.Count < 1)
            {
                return null;
            }

            List<Dictionary<string, object>> FormatedData = new List<Dictionary<string, object>>();
            Dictionary<string, object> dic;

            for (int i = 0; i < list.Count; i++)
            {
                dic = new Dictionary<string, object>();
                for (int j = 0; j < Columns.Count; j++)
                {
                    PropertyInfo pi = list[i].GetType().GetProperty(Columns[j].FieldName);
                    object cell = pi == null ? null : pi.GetValue(list[i], null);

                    //如果此列有委托函数 则进行格式化
                    if (Columns[j].FuncFormater != null)
                    {
                        dic.Add(Columns[j].FieldName, Columns[j].FuncFormater(list[i], cell, i));
                    }
                    else
                    {
                        dic.Add(Columns[j].FieldName, cell);
                    }

                }
                FormatedData.Add(dic);
            }
            return FormatedData;
        }
        /// <summary>
        /// list<实体>对象转datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable(IList entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                return null;
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //生成DataTable的structure
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }


        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void LoadData(object value)
        {
            int recordCount = 0;
            List<Dictionary<string, object>> result;
            if (this.DataSource != null)
            {
                object _dataSource = this.DataSource(ref recordCount);
                if (_dataSource != null)
                {
                    if (_dataSource is IList)
                    {
                        result = FormatAll((IList)_dataSource);
                    }
                    else if (_dataSource.GetType() == typeof(DataTable))
                    {
                        result = FormatAll((DataTable)_dataSource);
                    }
                    else
                    {
                        result = null;
                    }
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                throw new ArgumentNullException("DataSource");
            }

            if (result != null)
            {
                var returnJson = new
                {
                    total = recordCount,
                    rows = result,
                    msg = value
                };
                Response.Write(_jssl.Serialize(returnJson));
                Response.End();
            }
            else
            {
                var returnJson = new
                {
                    total = 0,
                    rows = new string[0],
                    msg = value
                };

                Response.Write(_jssl.Serialize(returnJson));
                Response.End();
            }
        }

        protected string ColumnToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Column item in Columns)
            {
                sb.Append("," + item.ToString());
            }
            return sb.Length != 0 ? sb.Remove(0, 1).ToString() : "";
        }

        protected string ToolBarToString()
        {
            if (Controls.Count == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=\"" + this.ClientID + "_tb\">");
            foreach (Control item in Controls)
            {
                sb.Append(item.ToString(this.ClientID));
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        protected string QueryParamsToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Control item in Controls)
            {
                sb.Append(item.GetValueJS(this.ClientID));
            }
            if (sb.Length > 0)
            {
                sb.Remove(0, 1);
            }
            return sb.ToString();
        }
        protected string EventsToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Event)
            {
                sb.Append("," + item.Key + ":" + item.Value);
            }
            return sb.ToString();
        }

        #endregion
        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.FindControl("UIHeader") == null)
            {
                System.Web.UI.Control header = Page.LoadControl(StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Helper/UIHeader.ascx"));
                header.ID = "UIHeader";
                this.Page.Header.Controls.Add(header);
            }
            if (Request["uc"] != null && Request["uc"] == this.ClientID)
            {
                if (Request["click"] != null)
                {
                    string ids = Request["checkedIds"];
                    Control ctrl = this.Controls.Where(c => this.ClientID + "_" + c.ID == Request["click"]).FirstOrDefault();
                    LoadData(ctrl.Handler(ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)));
                }
                else
                {

                    LoadData("");
                }
            }

        }
        #endregion
    }

    /// <summary>
    /// 列配置
    /// </summary>
    public class Column
    {

        private bool _IsCheckbox = false;
        public bool IsCheckbox
        {
            get { return _IsCheckbox; }
            set { _IsCheckbox = value; }
        }
        /// <summary>
        /// 显示的列名
        /// </summary>
        private string _Name = "编号";
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        /// <summary>
        /// 源数据中的字段名
        /// </summary>


        private string _FieldName = "Id";

        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        /// <summary>
        /// 是否可排序
        /// </summary>
        private bool sortable = false;
        public bool Sortable
        {
            get { return sortable; }
            set { sortable = value; }
        }


        /// <summary>
        /// 源数据中的字段名
        /// </summary>
        private int _Width = 100;

        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        private Algin _Align = Algin.center;
        public Algin Align
        {
            get { return _Align; }
            set { _Align = value; }
        }
        /// <summary>
        /// 格式化方法
        /// </summary>
        public Func<object, object, int, string> FuncFormater { get; set; }
        /// <summary>
        /// 输出配置js
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "{ checkbox: " + (IsCheckbox ? "true" : "false") + ",field: '" + FieldName + "', title: '" + Name + "', sortable:" + Sortable.ToString().ToLower() + ",width: " + Width + ", align: '" + Align.ToString() + "' }";
        }
    }
    /// <summary>
    /// 工具栏按钮
    /// </summary>
    public class Control
    {
        public string ID { get; set; }
        public CtrlType CType { get; set; }
        public string Text { get; set; }
        public ICO IconCls { get; set; }
        /// <summary>
        /// 点击后的确认消息(仅服务器按钮)
        /// </summary>
        public string Confirmation { get; set; }
        private bool isValidateSelections = false;
        /// <summary>
        /// 点击后是否验证选择行
        /// </summary>
        public bool IsValidateSelections
        {
            get { return isValidateSelections; }
            set { isValidateSelections = value; }
        }

        public Func<string[], ServerBtnResult> Handler { get; set; }
        public string Jscript { get; set; }
        private bool visible = true;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }


        public string ToString(string ClientID)
        {
            #region 文本框
            if (this.CType == CtrlType.TextBox)
            {
                return "<div style=\"float: left;margin:5px;\">" + Text + " <input id=\"" + ClientID + "_" + ID + "\" type=\"text\" class=\"searchbox\"/></div>";
            }
            #endregion
            #region 日期区间
            else if (this.CType == CtrlType.DateBox)
            {
                return "<div style=\"float: left;margin:3px;\">" + Text + " <input id=\"" + ClientID + "_" + ID + "_B\" type=\"text\" maxlength=\"25\"  class=\"easyui-datebox\" style=\"width:100px;\" />-<input id=\"" + ClientID + "_" + ID + "_E\" type=\"text\" maxlength=\"25\" class=\"easyui-datebox\" style=\"width:100px;\" /></div>";

            }
            #endregion
            #region 下拉列表
            else if (this.CType == CtrlType.ComboBox)
            {
                string[] Options = Text.Split('|');
                StringBuilder sb = new StringBuilder("<div style=\"float: left;margin:3px;\"><select id=\"" + ClientID + "_" + ID + "\" style=\"display: block;box-sizing: border-box;border:1px solid #d3d3d3;\">");
                for (int i = 0; i < Options.Length; i++)
                {
                    sb.Append("<option value=\"" + Options[i] + "\">" + Options[i] + "</option>");
                }
                sb.Append("</select></div>");
                return sb.ToString();
            }
            #endregion
            #region 纯js按钮
            else if (this.CType == CtrlType.JsButton)
            {
                return "<a id=\"" + ClientID + "_" + ID + "\" href=\"javascript:;\" " + (Visible ? "" : "style=\"display:none;\"") + " class=\"easyui-linkbutton\" iconCls=\"" + (IconCls == ICO.none ? "" : "icon-" + IconCls.ToString().Replace("_", "-")) + "\" plain=\"true\" onclick=\"" + Jscript + "\">" + Text + "</a>";
            }
            #endregion
            #region 表单项提交按钮
            else if (this.CType == CtrlType.SearchButton)
            {
                return "<a id=\"" + ClientID + "_" + ID + "\" href=\"javascript:;\" class=\"easyui-linkbutton\"  iconCls=\"" + (IconCls == ICO.none ? "" : "icon-" + IconCls.ToString().Replace("_", "-")) + "\" plain=\"true\" onclick=\"" + ClientID + "Search();\" >" + Text + "</a>";
            }
            #endregion
            #region 服务器按钮
            else if (this.CType == CtrlType.ServerButton)
            {
                string jsEvent = ClientID + "ServerBtnClick('" + ClientID + "_" + ID + "');";
                if (!string.IsNullOrEmpty(Confirmation))
                {
                    jsEvent = "$.messager.confirm('确认','" + Confirmation + "',function(r){if (r){" + jsEvent + "}});";
                }
                if (IsValidateSelections)
                {
                    jsEvent = "if(" + ClientID + "GetSelectedIds().length>0){" + jsEvent + "}else{$.messager.alert('Warning','请至少选择一行');}";
                }
                return "<a id=\"" + ClientID + "_" + ID + "\" href=\"javascript:;\" " + (Visible ? "" : "style=\"display:none;\"") + " class=\"easyui-linkbutton\"  iconCls=\"" + (IconCls == ICO.none ? "" : "icon-" + IconCls.ToString().Replace("_", "-")) + "\" plain=\"true\" onclick=\"" + jsEvent + "\">" + Text + "</a>";
            }
            #endregion
            return "";
        }

        public string GetValueJS(string ClientID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return "";
            }
            if (CType == CtrlType.DateBox)
            {
                return ",'" + ID + "_B':$('#" + ClientID + "_" + ID + "_B').next().find('.combo-value').val(),'" + ID + "_E':$('#" + ClientID + "_" + ID + "_E').next().find('.combo-value').val()";
            }
            else
            {
                return ",'" + ID + "':$('#" + ClientID + "_" + ID + "').val()";
            }
        }
    }
    /// <summary>
    /// button图标
    /// </summary>
    public enum ICO
    {
        none, blank, add, edit, remove, save, cut,
        ok, no, cancel, reload, search, print,
        help, undo, redo, back, sum, tip,
        mini_add, mini_edit, mini_refresh
    }
    /// <summary>
    /// 
    /// </summary>
    public enum Algin
    {
        left, right, center
    }
    /// <summary>
    /// 控件类型
    /// </summary>
    public enum CtrlType
    {
        /// <summary>
        /// 文本
        /// </summary>
        TextBox,

        /// <summary>
        /// 时间区间
        /// </summary>
        DateBox,

        /// <summary>
        /// 下拉
        /// </summary>
        ComboBox,

        /// <summary>
        /// 执行js方法
        /// </summary>
        JsButton,

        /// <summary>
        /// 搜索专用
        /// </summary>
        SearchButton,

        /// <summary>
        /// 服务器按钮
        /// </summary>
        ServerButton,
    }

    public class ServerBtnResult
    {
        public string Msg { get; set; }
        public string Jscript { get; set; }
    }

}