using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using UserControls.Controls.jeasyui.Form;
using System.Reflection;
using ObjectCMS.Common;
using System.Data;

namespace UserControls.Controls.jeasyui
{
    /// <summary>
    /// 核心类
    /// </summary>
    public partial class FormBuilder : System.Web.UI.UserControl
    {

        #region 属性
        private object _Entity;
        /// <summary>
        /// 获取或设置Form的模型对象
        /// </summary>
        public object Entity
        {
            get
            {
                if (_Entity is Dictionary<string, object>)
                {
                    for (int i = 0; i < _Items.Count; i++)
                    {
                        ((Dictionary<string, object>)_Entity)[_Items[i].FieldName] = GetFormEntityValue(_Items[i], i);
                    }
                }
                else
                {
                    for (int i = 0; i < _Items.Count; i++)
                    {
                        var p = _Entity.GetType().GetProperty(_Items[i].FieldName);
                        p.SetValue(_Entity, Convert.ChangeType(GetFormEntityValue(_Items[i], i), p.PropertyType), null);

                    }
                }
                return _Entity;
            }
            set { _Entity = value; }
        }

        private List<FormItem> _Items = new List<FormItem>();
        /// <summary>
        /// 设置表单元素类型
        /// </summary>
        public List<FormItem> Items
        {
            set { _Items = value; }
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
            BindFormTable();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 绑定table
        /// </summary>
        protected void BindFormTable()
        {
            StringBuilder sb = new StringBuilder();
            TableRow tr;
            for (int i = 0; i < _Items.Count; i++)
            {
                tr = new TableRow();
                tr.CssClass = "td_bg";
                TableCell td = new TableCell();
                td.Width = 70;
                td.Text = "<span><strong>" + _Items[i].Title + "：</strong></span>";
                tr.Cells.Add(td);
                td = new TableCell();
                if (_Entity is Dictionary<string, object>)
                {
                    td.Controls.Add(SetFormEntityValueFromDictionary(_Items[i], i));
                }
                else
                {
                    td.Controls.Add(SetFormEntityValue(_Items[i], i));
                }
                tr.Cells.Add(td);
                tbl_Warpper.Rows.Add(tr);
            }
        }

        /// <summary>
        /// 初始化各类型控件 并赋初始值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public System.Web.UI.UserControl SetFormEntityValue(FormItem item, int index)
        {

            Action action = () =>
            {

            };
            PropertyInfo Pi = _Entity.GetType().GetProperty(item.FieldName);
            switch (item.FieldType)
            {
                #region 日期时间
                case FieldTypeInHTML.DateTimeBox:
                    Form.DateTimeBox dtb = (Form.DateTimeBox)TemplateControl.LoadControl("Form/DateTimeBox.ascx");
                    dtb.DateModel = ShowType.DateTime;
                    dtb.ID = "DateTimeBox" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        if (item.DefVal.ToLower().Trim() == "getdate()")
                        {
                            dtb.Date = DateTime.Now;
                        }
                        else
                        {
                            dtb.Date = Convert.ToDateTime(item.DefVal);
                        }
                    }

                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelDateTimeBox mdtb = (ModelDateTimeBox)item.FieldModel;
                        dtb.MaxDate = mdtb.MaxDate;
                        dtb.MinDate = mdtb.MinDate;
                        dtb.Date = mdtb.Date;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            dtb.Date = Convert.ToDateTime(val);
                    }
                    return dtb;
                #endregion
                #region 日期
                case FieldTypeInHTML.DateBox:
                    Form.DateTimeBox db = (Form.DateTimeBox)TemplateControl.LoadControl("Form/DateTimeBox.ascx");
                    db.DateModel = ShowType.OnlyDate;
                    db.ID = "DateBox" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        if (item.DefVal.ToLower().Trim() == "getdate()")
                        {
                            db.Date = DateTime.Now;
                        }
                        else
                        {
                            db.Date = Convert.ToDateTime(item.DefVal);
                        }
                    }

                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelDateBox mdb = (ModelDateBox)item.FieldModel;
                        db.MaxDate = mdb.MaxDate;
                        db.MinDate = mdb.MinDate;
                        db.Date = mdb.Date;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            db.Date = Convert.ToDateTime(val);
                    }
                    return db;
                #endregion
                #region 时间
                case FieldTypeInHTML.TimeBox:
                    Form.DateTimeBox tb = (Form.DateTimeBox)TemplateControl.LoadControl("Form/DateTimeBox.ascx");
                    tb.DateModel = ShowType.OnlyTime;
                    tb.ID = "TimeBox" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        if (item.DefVal.ToLower().Trim() == "getdate()")
                        {
                            tb.Date = DateTime.Now;
                        }
                        else
                        {
                            tb.Date = Convert.ToDateTime(item.DefVal);
                        }
                    }

                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelDateBox mtb = (ModelDateBox)item.FieldModel;
                        tb.MaxDate = mtb.MaxDate;
                        tb.MinDate = mtb.MinDate;
                        tb.Date = mtb.Date;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            tb.Date = Convert.ToDateTime(val);
                    }
                    return tb;
                #endregion
                #region 编辑器
                case FieldTypeInHTML.Editor:
                    Form.Editor e = (Form.Editor)TemplateControl.LoadControl("Form/Editor.ascx");
                    e.ID = "Editor" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        e.Text = item.DefVal;
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelEditor me = (ModelEditor)item.FieldModel;
                        e.Height = me.Height;
                        e.Width = me.Width;
                        e.Text = me.Text;
                    }
                    else
                    {
                        e.Width = Unit.Percentage(99);
                        e.Height = 320;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            e.Text = val.ToString();
                    }
                    return e;
                #endregion
                #region 代码编辑器
                case FieldTypeInHTML.CodeEditor:
                    Form.CodeEditor ce = (Form.CodeEditor)TemplateControl.LoadControl("Form/CodeEditor.ascx");
                    ce.ID = "CodeEditor" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        ce.Text = item.DefVal;
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelCodeEditor mce = (ModelCodeEditor)item.FieldModel;
                        ce.Height = mce.Height;
                        ce.Width = mce.Width;
                        ce.Text = mce.Text;
                    }
                    else
                    {
                        ce.Width = Unit.Percentage(99);
                        ce.Height = 320;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            ce.Text = val.ToString();
                    }
                    return ce;
                #endregion
                #region 文件上传
                case FieldTypeInHTML.FileUpload:
                    Form.FileUpload f = (Form.FileUpload)TemplateControl.LoadControl("Form/FileUpload.ascx");
                    f.ID = "FileUpload" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        f.Path = item.DefVal;
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelFileUpload mf = (ModelFileUpload)item.FieldModel;
                        f.Path = mf.Path;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            f.Path = val.ToString();
                    }
                    return f;
                #endregion
                #region 图片上传
                case FieldTypeInHTML.ImgUpload:
                    Form.ImgUpload i = (Form.ImgUpload)TemplateControl.LoadControl("Form/ImgUpload.ascx");
                    i.ID = "ImgUpload" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        i.Path = item.DefVal;
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelImgUpload mi = (ModelImgUpload)item.FieldModel;
                        i.Path = mi.Path;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            i.Path = val.ToString();
                    }
                    return i;
                #endregion
                #region 单行文本
                case FieldTypeInHTML.SingleLine:
                    Form.TextBox sl = (Form.TextBox)TemplateControl.LoadControl("Form/TextBox.ascx");
                    sl.ID = "SingleLine" + index;
                    sl.TextMode = TextBoxMode.SingleLine;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        sl.Text = item.DefVal;
                    }
                    sl.ValidateTypes = item.ValidateTypes;

                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelSingleLine msl = (ModelSingleLine)item.FieldModel;
                        sl.TextMode = msl.TextModel;
                        sl.Height = msl.Height;
                        sl.OnBlur = msl.OnBlur;
                        sl.OnKeyDown = msl.OnKeyDown;
                        sl.OnKeyUp = msl.OnKeyUp;
                        sl.ReadOnly = msl.ReadOnly;
                        sl.Text = msl.Text;
                        sl.Width = msl.Width;
                        sl.ValidateTypes = msl.ValidateTypes;
                    }
                    else
                    {
                        sl.Width = Unit.Percentage(98);
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            sl.Text = val.ToString();
                    }
                    return sl;
                #endregion
                #region 多行文本
                case FieldTypeInHTML.MultiLine:
                    Form.TextBox ml = (Form.TextBox)TemplateControl.LoadControl("Form/TextBox.ascx");
                    ml.ID = "MultiLine" + index;
                    ml.TextMode = TextBoxMode.MultiLine;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        ml.Text = item.DefVal;
                    }
                    ml.ValidateTypes = item.ValidateTypes;

                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelMultiLine mml = (ModelMultiLine)item.FieldModel;
                        ml.Height = mml.Height;
                        ml.OnBlur = mml.OnBlur;
                        ml.OnKeyDown = mml.OnKeyDown;
                        ml.OnKeyUp = mml.OnKeyUp;
                        ml.ReadOnly = mml.ReadOnly;
                        ml.Text = mml.Text;
                        ml.Width = mml.Width;
                    }
                    else
                    {
                        ml.Width = Unit.Percentage(98);
                        ml.Height = 120;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            ml.Text = val.ToString();
                    }
                    return ml;
                #endregion
                #region 下拉列表框
                case FieldTypeInHTML.Select:
                    Form.Selections s = (Form.Selections)TemplateControl.LoadControl("Form/Selections.ascx");
                    s.ID = "Select" + index;
                    s.Type = SelectionType.select;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        s.SelectedValue = item.DefVal;
                    }
                    if (!string.IsNullOrEmpty(item.OtherAttr))
                    {
                        s.Items = item.OtherAttr.Split(';');
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelSelect ms = (ModelSelect)item.FieldModel;
                        s.SelectedValue = ms.SelectedValue;
                        s.Items = ms.Items;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            s.SelectedValue = val.ToString();
                    }
                    return s;
                #endregion
                #region 单选按钮
                case FieldTypeInHTML.RadioButton:
                    Form.Selections r = (Form.Selections)TemplateControl.LoadControl("Form/Selections.ascx");
                    r.ID = "RadioButton" + index;
                    r.Type = SelectionType.radio;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        r.SelectedValue = item.DefVal;
                    }
                    if (!string.IsNullOrEmpty(item.OtherAttr))
                    {
                        r.Items = item.OtherAttr.Split(';');
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelRadioButton mr = (ModelRadioButton)item.FieldModel;
                        r.SelectedValue = mr.SelectedValue;
                        r.Items = mr.Items;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            r.SelectedValue = val.ToString();
                    }
                    return r;

                #endregion
                #region 多选框
                case FieldTypeInHTML.CheckBox:
                    Form.Selections cb = (Form.Selections)TemplateControl.LoadControl("Form/Selections.ascx");
                    cb.ID = "CheckBox" + index;
                    cb.Type = SelectionType.checkbox;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        cb.SelectedValue = item.DefVal;
                    }
                    if (!string.IsNullOrEmpty(item.OtherAttr))
                    {
                        cb.Items = item.OtherAttr.Split(';');
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelCheckBox mcb = (ModelCheckBox)item.FieldModel;
                        cb.SelectedValue = mcb.SelectedValue;
                        cb.Items = mcb.Items;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            cb.SelectedValue = val.ToString();
                    }
                    return cb;
                #endregion
                #region 树形节点
                case FieldTypeInHTML.TreeNode:
                    Form.TreeNode tn = (Form.TreeNode)TemplateControl.LoadControl("Form/TreeNode.ascx");
                    tn.ID = "TreeNode" + index;
                    if (item.FieldModel != null)
                    {
                        ModelTreeNode mtn = (ModelTreeNode)item.FieldModel;
                        tn.SelectedValue = mtn.SelectedValue;
                        tn.IdField = mtn.IdField;
                        tn.ParentField = mtn.ParentField;
                        tn.TextField = mtn.TextField;
                        tn.DataSource = mtn.DataSource;
                    }
                    if (Pi != null)
                    {
                        object val = Pi.GetValue(_Entity, null);
                        if (val != null)
                            tn.SelectedValue = val.ToString();
                    }
                    return tn;
                #endregion
                default:
                    return null;
            }
        }


        public System.Web.UI.UserControl SetFormEntityValueFromDictionary(FormItem item, int index)
        {
            object val = null;
            if (((Dictionary<string, object>)_Entity).Keys.Contains(item.FieldName))
            {
                val = ((Dictionary<string, object>)_Entity)[item.FieldName];
            }
            switch (item.FieldType)
            {
                #region 日期时间
                case FieldTypeInHTML.DateTimeBox:
                    Form.DateTimeBox dtb = (Form.DateTimeBox)TemplateControl.LoadControl("Form/DateTimeBox.ascx");
                    dtb.DateModel = ShowType.DateTime;
                    dtb.ID = "DateTimeBox" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        if (item.DefVal.ToLower().Trim() == "getdate()")
                        {
                            dtb.Date = DateTime.Now;
                        }
                        else
                        {
                            dtb.Date = Convert.ToDateTime(item.DefVal);
                        }
                    }

                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelDateTimeBox mdtb = (ModelDateTimeBox)item.FieldModel;
                        dtb.MaxDate = mdtb.MaxDate;
                        dtb.MinDate = mdtb.MinDate;
                        dtb.Date = mdtb.Date;
                    }
                    if (val != null)
                        dtb.Date = TypeConverter.ObjectToDateTime(val);

                    return dtb;
                #endregion
                #region 日期
                case FieldTypeInHTML.DateBox:
                    Form.DateTimeBox db = (Form.DateTimeBox)TemplateControl.LoadControl("Form/DateTimeBox.ascx");
                    db.DateModel = ShowType.OnlyDate;
                    db.ID = "DateBox" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        if (item.DefVal.ToLower().Trim() == "getdate()")
                        {
                            db.Date = DateTime.Now;
                        }
                        else
                        {
                            db.Date = Convert.ToDateTime(item.DefVal);
                        }
                    }

                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelDateBox mdb = (ModelDateBox)item.FieldModel;
                        db.MaxDate = mdb.MaxDate;
                        db.MinDate = mdb.MinDate;
                        db.Date = mdb.Date;
                    }
                    if (val != null)
                    {
                        db.Date = TypeConverter.ObjectToDateTime(val);
                    }

                    return db;
                #endregion
                #region 时间
                case FieldTypeInHTML.TimeBox:
                    Form.DateTimeBox tb = (Form.DateTimeBox)TemplateControl.LoadControl("Form/DateTimeBox.ascx");
                    tb.DateModel = ShowType.OnlyTime;
                    tb.ID = "TimeBox" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        if (item.DefVal.ToLower().Trim() == "getdate()")
                        {
                            tb.Date = DateTime.Now;
                        }
                        else
                        {
                            tb.Date = Convert.ToDateTime(item.DefVal);
                        }
                    }

                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelDateBox mtb = (ModelDateBox)item.FieldModel;
                        tb.MaxDate = mtb.MaxDate;
                        tb.MinDate = mtb.MinDate;
                        tb.Date = mtb.Date;
                    }
                    if (val != null)
                        tb.Date = TypeConverter.ObjectToDateTime(val);

                    return tb;
                #endregion
                #region 编辑器
                case FieldTypeInHTML.Editor:
                    Form.Editor e = (Form.Editor)TemplateControl.LoadControl("Form/Editor.ascx");
                    e.ID = "Editor" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        e.Text = item.DefVal;
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelEditor me = (ModelEditor)item.FieldModel;
                        e.Height = me.Height;
                        e.Width = me.Width;
                        e.Text = me.Text;
                    }
                    else
                    {
                        e.Width = Unit.Percentage(99);
                        e.Height = 320;
                    }
                    if (val != null)
                        e.Text = val.ToString();

                    return e;
                #endregion
                #region 代码编辑器
                case FieldTypeInHTML.CodeEditor:
                    Form.CodeEditor ce = (Form.CodeEditor)TemplateControl.LoadControl("Form/CodeEditor.ascx");
                    ce.ID = "CodeEditor" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        ce.Text = item.DefVal;
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelCodeEditor mce = (ModelCodeEditor)item.FieldModel;
                        ce.Height = mce.Height;
                        ce.Width = mce.Width;
                        ce.Text = mce.Text;
                    }
                    else
                    {
                        ce.Width = Unit.Percentage(99);
                        ce.Height = 320;
                    }
                    if (val != null)
                        ce.Text = val.ToString();

                    return ce;
                #endregion
                #region 文件上传
                case FieldTypeInHTML.FileUpload:
                    Form.FileUpload f = (Form.FileUpload)TemplateControl.LoadControl("Form/FileUpload.ascx");
                    f.ID = "FileUpload" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        f.Path = item.DefVal;
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelFileUpload mf = (ModelFileUpload)item.FieldModel;
                        f.Path = mf.Path;
                    }
                    if (val != null)
                        f.Path = val.ToString();

                    return f;
                #endregion
                #region 图片上传
                case FieldTypeInHTML.ImgUpload:
                    Form.ImgUpload i = (Form.ImgUpload)TemplateControl.LoadControl("Form/ImgUpload.ascx");
                    i.ID = "ImgUpload" + index;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        i.Path = item.DefVal;
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelImgUpload mi = (ModelImgUpload)item.FieldModel;
                        i.Path = mi.Path;
                    }
                    if (val != null)
                        i.Path = val.ToString();

                    return i;
                #endregion
                #region 单行文本
                case FieldTypeInHTML.SingleLine:
                    Form.TextBox sl = (Form.TextBox)TemplateControl.LoadControl("Form/TextBox.ascx");
                    sl.ID = "SingleLine" + index;
                    sl.TextMode = TextBoxMode.SingleLine;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        sl.Text = item.DefVal;
                    }
                    sl.ValidateTypes = item.ValidateTypes;

                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelSingleLine msl = (ModelSingleLine)item.FieldModel;
                        sl.TextMode = msl.TextModel;
                        sl.Height = msl.Height;
                        sl.OnBlur = msl.OnBlur;
                        sl.OnKeyDown = msl.OnKeyDown;
                        sl.OnKeyUp = msl.OnKeyUp;
                        sl.ReadOnly = msl.ReadOnly;
                        sl.Text = msl.Text;
                        sl.Width = msl.Width;
                        sl.ValidateTypes = msl.ValidateTypes;
                    }
                    else
                    {
                        sl.Width = Unit.Percentage(98);
                    }
                    if (val != null)
                        sl.Text = val.ToString();

                    return sl;
                #endregion
                #region 多行文本
                case FieldTypeInHTML.MultiLine:
                    Form.TextBox ml = (Form.TextBox)TemplateControl.LoadControl("Form/TextBox.ascx");
                    ml.ID = "MultiLine" + index;
                    ml.TextMode = TextBoxMode.MultiLine;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        ml.Text = item.DefVal;
                    }
                    ml.ValidateTypes = item.ValidateTypes;

                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelMultiLine mml = (ModelMultiLine)item.FieldModel;
                        ml.Height = mml.Height;
                        ml.OnBlur = mml.OnBlur;
                        ml.OnKeyDown = mml.OnKeyDown;
                        ml.OnKeyUp = mml.OnKeyUp;
                        ml.ReadOnly = mml.ReadOnly;
                        ml.Text = mml.Text;
                        ml.Width = mml.Width;
                    }
                    else
                    {
                        ml.Width = Unit.Percentage(98);
                        ml.Height = 120;
                    }
                    if (val != null)
                        ml.Text = val.ToString();

                    return ml;
                #endregion
                #region 下拉列表框
                case FieldTypeInHTML.Select:
                    Form.Selections s = (Form.Selections)TemplateControl.LoadControl("Form/Selections.ascx");
                    s.ID = "Select" + index;
                    s.Type = SelectionType.select;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        s.SelectedValue = item.DefVal;
                    }
                    if (!string.IsNullOrEmpty(item.OtherAttr))
                    {
                        s.Items = item.OtherAttr.Split(';');
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelSelect ms = (ModelSelect)item.FieldModel;
                        s.SelectedValue = ms.SelectedValue;
                        s.Items = ms.Items;
                    }
                    if (val != null)
                        s.SelectedValue = val.ToString();

                    return s;
                #endregion
                #region 单选按钮
                case FieldTypeInHTML.RadioButton:
                    Form.Selections r = (Form.Selections)TemplateControl.LoadControl("Form/Selections.ascx");
                    r.ID = "RadioButton" + index;
                    r.Type = SelectionType.radio;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        r.SelectedValue = item.DefVal;
                    }
                    if (!string.IsNullOrEmpty(item.OtherAttr))
                    {
                        r.Items = item.OtherAttr.Split(';');
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelRadioButton mr = (ModelRadioButton)item.FieldModel;
                        r.SelectedValue = mr.SelectedValue;
                        r.Items = mr.Items;
                    }
                    if (val != null)
                        r.SelectedValue = val.ToString();

                    return r;

                #endregion
                #region 多选框
                case FieldTypeInHTML.CheckBox:
                    Form.Selections cb = (Form.Selections)TemplateControl.LoadControl("Form/Selections.ascx");
                    cb.ID = "CheckBox" + index;
                    cb.Type = SelectionType.checkbox;
                    #region 次要属性
                    if (!string.IsNullOrEmpty(item.DefVal))
                    {
                        cb.SelectedValue = item.DefVal;
                    }
                    if (!string.IsNullOrEmpty(item.OtherAttr))
                    {
                        cb.Items = item.OtherAttr.Split(';');
                    }
                    #endregion
                    if (item.FieldModel != null)
                    {
                        ModelCheckBox mcb = (ModelCheckBox)item.FieldModel;
                        cb.SelectedValue = mcb.SelectedValue;
                        cb.Items = mcb.Items;
                    }
                    if (val != null)
                        cb.SelectedValue = val.ToString();

                    return cb;
                #endregion
                #region 树形节点
                case FieldTypeInHTML.TreeNode:
                    Form.TreeNode tn = (Form.TreeNode)TemplateControl.LoadControl("Form/TreeNode.ascx");
                    tn.ID = "TreeNode" + index;
                    if (item.FieldModel != null)
                    {
                        ModelTreeNode mtn = (ModelTreeNode)item.FieldModel;
                        tn.SelectedValue = mtn.SelectedValue;
                        tn.IdField = mtn.IdField;
                        tn.ParentField = mtn.ParentField;
                        tn.TextField = mtn.TextField;
                        tn.DataSource = mtn.DataSource;
                    }
                    if (val != null)
                        tn.SelectedValue = val.ToString();

                    return tn;
                #endregion
                default:
                    return null;
            }
        }




        /// <summary>
        /// 获取请求中的数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public object GetFormEntityValue(FormItem item, int index)
        {
            switch (item.FieldType)
            {
                case FieldTypeInHTML.DateTimeBox:
                    return Convert.ToDateTime(Request.Form[this.ClientID + "$DateTimeBox" + index + "$tb_DateTime"]);
                case FieldTypeInHTML.DateBox:
                    return Convert.ToDateTime(Request.Form[this.ClientID + "$DateBox" + index + "$tb_DateTime"]);
                case FieldTypeInHTML.TimeBox:
                    return Convert.ToDateTime(Request.Form[this.ClientID + "$TimeBox" + index + "$tb_DateTime"]);
                case FieldTypeInHTML.Editor:
                    return Request.Form[this.ClientID + "$Editor" + index + "$tb_Editor"];
                case FieldTypeInHTML.CodeEditor:
                    return Request.Form[this.ClientID + "$CodeEditor" + index + "$tb_CodeEditor"];
                case FieldTypeInHTML.FileUpload:
                    return Request.Form[this.ClientID + "$FileUpload" + index + "$tb_FilePath"];
                case FieldTypeInHTML.ImgUpload:
                    return Request.Form[this.ClientID + "$ImgUpload" + index + "$tb_ImgPath"];
                case FieldTypeInHTML.SingleLine:
                    return Request.Form[this.ClientID + "$SingleLine" + index + "$tb_SingleLine"];
                case FieldTypeInHTML.MultiLine:
                    return Request.Form[this.ClientID + "$MultiLine" + index + "$tb_SingleLine"];
                case FieldTypeInHTML.Select:
                    return Request.Form[this.ClientID + "$Select" + index + "$Selection"];
                case FieldTypeInHTML.CheckBox:
                    return Request.Form[this.ClientID + "$CheckBox" + index + "$Selection"];
                case FieldTypeInHTML.RadioButton:
                    return Request.Form[this.ClientID + "$RadioButton" + index + "$Selection"];
                case FieldTypeInHTML.TreeNode:
                    return int.Parse(Request.Form[this.ClientID + "$TreeNode" + index + "$TreeNode"]);
                default:
                    return null;
            }
        }
        #endregion
    }
    /// <summary>
    /// 表单元素
    /// </summary>
    public class FormItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 字段名 (必须与Entity的属性名大小写一致)
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段在form中显示的类型
        /// </summary>
        public FieldTypeInHTML FieldType { get; set; }
        /// <summary>
        /// 字段类型所需的参数(可选)
        /// </summary>
        public FormControlModel FieldModel { get; set; }

        public string DefVal { get; set; }
        public string OtherAttr { get; set; }
        public List<FieldValidate> ValidateTypes { get; set; }
        /// <summary>
        /// 验证类型
        /// </summary>
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Tip { get; set; }
    }
    /// <summary>
    /// 表单类型
    /// </summary>
    public enum FieldTypeInHTML
    {
        CheckBox,
        DateBox,
        DateTimeBox,
        Editor,
        CodeEditor,
        FileUpload,
        ImgUpload,
        MultiLine,
        RadioButton,
        Select,
        SingleLine,
        TimeBox,
        TreeNode
    }
    /// <summary>
    /// 验证类型
    /// </summary>
    public enum FieldValidate
    {
        /// <summary>
        /// 必填
        /// </summary>
        required,
        /// <summary>
        /// 电话
        /// </summary>
        phone,
        /// <summary>
        /// 电子邮箱
        /// </summary>
        email,
        /// <summary>
        /// 网址
        /// </summary>
        url,
        /// <summary>
        /// 日期
        /// </summary>
        date,
        /// <summary>
        /// 数字
        /// </summary>
        number,
        /// <summary>
        /// ip地址
        /// </summary>
        ipv4
    }

    #region 控件模型类(仅用于传值)
    public class FormControlModel
    {
    }
    /// <summary>
    /// 日期和时间(年-月-日 时:分)
    /// </summary>
    public class ModelDateTimeBox : FormControlModel
    {
        public DateTime? Date { get; set; }

        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get; set; }
    }
    /// <summary>
    /// 日期(年-月-日)
    /// </summary>
    public class ModelDateBox : FormControlModel
    {
        public DateTime? Date { get; set; }

        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get; set; }
    }
    /// <summary>
    /// 时间(小时:分钟)
    /// </summary>
    public class ModelTimeBox : FormControlModel
    {
        public DateTime? Date { get; set; }

        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get; set; }
    }
    /// <summary>
    /// 编辑器
    /// </summary>
    public class ModelEditor : FormControlModel
    {
        public string Text { get; set; }
        private Unit _Width = Unit.Percentage(99);

        public Unit Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        public Unit Height { get; set; }

    }
    public class ModelCodeEditor : FormControlModel
    {
        public string Text { get; set; }
        private Unit _Width = Unit.Percentage(99);

        public Unit Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        public Unit Height { get; set; }

    }
    /// <summary>
    /// 文件上传
    /// </summary>
    public class ModelFileUpload : FormControlModel
    {
        public string Path { get; set; }
    }
    /// <summary>
    /// 图片上传
    /// </summary>
    public class ModelImgUpload : FormControlModel
    {
        public string Path { get; set; }
    }
    /// <summary>
    /// 当行文本
    /// </summary>
    public class ModelSingleLine : FormControlModel
    {
        public string Text { get; set; }
        private Unit _Width = Unit.Percentage(99);

        public Unit Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        public Unit Height { get; set; }
        public bool ReadOnly { get; set; }
        public TextBoxMode TextModel { get; set; }
        public List<FieldValidate> ValidateTypes { get; set; }

        /// <summary>
        /// 光标移出事件
        /// </summary>
        public Func<string, EventResult> OnBlur { get; set; }
        /// <summary>
        /// 键盘按下事件
        /// </summary>
        public Func<string, EventResult> OnKeyDown { get; set; }
        /// <summary>
        /// 键盘抬起事件
        /// </summary>
        public Func<string, EventResult> OnKeyUp { get; set; }

    }
    /// <summary>
    /// 多行文本
    /// </summary>
    public class ModelMultiLine : FormControlModel
    {
        public string Text { get; set; }
        private Unit _Width = Unit.Percentage(99);

        public Unit Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        private Unit _Height = 120;

        public Unit Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        public bool ReadOnly { get; set; }

        /// <summary>
        /// 光标移出事件
        /// </summary>
        public Func<string, EventResult> OnBlur { get; set; }
        /// <summary>
        /// 键盘按下事件
        /// </summary>
        public Func<string, EventResult> OnKeyDown { get; set; }
        /// <summary>
        /// 键盘抬起事件
        /// </summary>
        public Func<string, EventResult> OnKeyUp { get; set; }
    }
    /// <summary>
    /// 下拉
    /// </summary>
    public class ModelSelect : FormControlModel
    {
        public string SelectedValue { get; set; }
        public string[] Items { get; set; }
    }
    /// <summary>
    /// 单选
    /// </summary>
    public class ModelRadioButton : FormControlModel
    {
        public string SelectedValue { get; set; }
        public string[] Items { get; set; }
    }
    /// <summary>
    /// 多选
    /// </summary>
    public class ModelCheckBox : FormControlModel
    {
        public string SelectedValue { get; set; }
        public string[] Items { get; set; }
    }
    /// <summary>
    /// 树形节点
    /// </summary>
    public class ModelTreeNode : FormControlModel
    {
        private string _IdField = "Id";

        public string IdField
        {
            get { return _IdField; }
            set { _IdField = value; }
        }

        private string _ParentField = "ParentId";

        public string ParentField
        {
            get { return _ParentField; }
            set { _ParentField = value; }
        }
        private string _TextField = "Title";

        public string TextField
        {
            get { return _TextField; }
            set { _TextField = value; }
        }
        public string SelectedValue { get; set; }
        public object DataSource { get; set; }
    }
    #endregion


    #region 用于呈现各类型只读状态的html
    public static class FieldTypeForReadOnly
    {
        public static string GetView(this FieldTypeInHTML type, object val)
        {
            switch (type)
            {
                case FieldTypeInHTML.DateBox:
                    return val.ToFormatDate("yyyy-MM-dd");
                case FieldTypeInHTML.DateTimeBox:
                    return val.ToFormatDate("yyyy-MM-dd HH:mm:ss");
                case FieldTypeInHTML.TimeBox:
                    return val.ToFormatDate("HH:mm:ss");
                case FieldTypeInHTML.FileUpload:
                    return "<a  href=\"" + val.ToString() + "\">点击下载</a>";
                case FieldTypeInHTML.ImgUpload:
                    string sitepath = ObjectCMS.Common.Cookie.GetCookie("curdb");
                    if (!string.IsNullOrEmpty(sitepath) && val.ToString().StartsWith("/"))
                    {
                        return "<img src=\"/Publish/" + sitepath + val.ToString() + "\" width=\"100%\"/>";
                    }

                    return "<img src=\"" + val.ToString() + "\" width=\"100%\"/>";
                default:
                    return val.ToString();
            }

        }
    }
    #endregion
}