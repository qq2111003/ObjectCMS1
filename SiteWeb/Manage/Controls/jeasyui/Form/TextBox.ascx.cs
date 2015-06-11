using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Common;

namespace UserControls.Controls.jeasyui.Form
{
    public partial class TextBox : System.Web.UI.UserControl
    {
        #region 属性
        public string Text
        {
            get { return tb_SingleLine.Text; }
            set
            {
                if (TextMode == TextBoxMode.Password)
                {
                    tb_SingleLine.Attributes.Add("value", value);
                }
                else
                {
                    tb_SingleLine.Text = value;
                }
            }
        }
        public Unit Width
        {
            get { return tb_SingleLine.Width; }
            set { tb_SingleLine.Width = value; }
        }
        public Unit Height
        {
            get { return tb_SingleLine.Height; }
            set { tb_SingleLine.Height = value; }
        }
        public bool ReadOnly
        {
            get { return tb_SingleLine.ReadOnly; }
            set { tb_SingleLine.ReadOnly = value; }
        }

        public List<FieldValidate> ValidateTypes
        {
            set
            {
                if (value != null)
                {
                    string classStr = "";
                    foreach (var item in value)
                    {
                        if (item == FieldValidate.required)
                        {
                            classStr += ",required";
                        }
                        else
                        {
                            classStr += ",custom[" + item + "]";
                        }
                    }
                    classStr = classStr.Length > 0 ? classStr.Substring(1) : "";
                    classStr = "validate[" + classStr + "]";
                    this.tb_SingleLine.CssClass = classStr;
                }
            }
        }

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
        /// 文本框类型
        /// </summary>
        public TextBoxMode TextMode
        {
            get { return tb_SingleLine.TextMode; }
            set { tb_SingleLine.TextMode = value; }
        }

        #endregion

        #region 事件
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
        #endregion

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.FindControl("UIHeader") == null)
            {
                System.Web.UI.Control header = Page.LoadControl(StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Helper/UIHeader.ascx"));
                header.ID = "UIHeader";
                this.Page.Header.Controls.Add(header);
            }
            if (Request["uc"] != null && Request["uc"] == this.ClientID)
            {
                this.Text = Request["value"];
                if (Request["event"] == "blur")
                {
                    Response.Write(OnBlur(Text).ToString(this.tb_SingleLine.ClientID));
                    Response.End();
                }
                else if (Request["event"] == "keydown")
                {
                    Response.Write(OnKeyDown(Text).ToString(this.tb_SingleLine.ClientID));
                    Response.End();
                }
                else if (Request["event"] == "keyup")
                {
                    Response.Write(OnKeyUp(Text).ToString(this.tb_SingleLine.ClientID));
                    Response.End();
                }
            }
        }

        /// <summary>
        /// 用js注册事件
        /// </summary>
        /// <returns></returns>
        protected string WriteEventJs()
        {
            string jsStr = "";
            if (OnBlur != null)
            {
                jsStr += "$('#" + this.tb_SingleLine.ClientID + "').blur(function () {\n\r" +
                         "   AjaxController.Post('" + this.DataUrl + "', { 'value': $(this).val(),event:'blur' }, function (data) {\n\r" +
                         "       eval(data);\n\r" +
                         "   });\n\r" +
                         "});\n\r";
            }
            else if (OnKeyDown != null)
            {
                jsStr += "$('#" + this.tb_SingleLine.ClientID + "').keydown(function () {\n\r" +
                         "   AjaxController.Post('" + this.DataUrl + "', { 'value': $(this).val(),event:'keydown' }, function (data) {\n\r" +
                         "       eval(data);\n\r" +
                         "   });\n\r" +
                         "});\n\r";
            }
            else if (OnKeyUp != null)
            {
                jsStr += "$('#" + this.tb_SingleLine.ClientID + "').keyup(function () {\n\r" +
                         "   AjaxController.Post('" + this.DataUrl + "', { 'value': $(this).val(),event:'keyup' }, function (data) {\n\r" +
                         "       eval(data);\n\r" +
                         "   });\n\r" +
                         "});\n\r";
            }
            else
            {

            }
            return jsStr;
        }
    }

    public class EventResult
    {
        /// <summary>
        /// 是否执行js true直接执行js eval(Text) false时直接赋值
        /// </summary>
        private bool runJs = false;

        public bool RunJs
        {
            get { return runJs; }
            set { runJs = value; }
        }
        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public string ToString(string ClientID)
        {
            if (RunJs)
            {
                return Text;
            }
            return "       $('#" + ClientID + "').val('" + Text + "');\n\r";
        }
    }


}