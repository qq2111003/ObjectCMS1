using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Reflection;

namespace LogSystem.Controls.jeasyui
{
    public partial class AutoCompleteTextBox : System.Web.UI.UserControl
    {
        private JavaScriptSerializer _jssl = new JavaScriptSerializer();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.FindControl("UIHeader") == null)
            {
                System.Web.UI.Control header = Page.LoadControl("/Manage/Controls/jeasyui/Helper/UIHeader.ascx");
                header.ID = "UIHeader";
                this.Page.Header.Controls.Add(header);
            }
            if (Request["uc"] != null && Request["uc"] == this.ClientID)
            {
                LoadData();
            }
        }
        /// <summary>
        /// 源数据
        /// </summary>
        public Func<string, object> DataSource { get; set; }

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
        /// 文本框的值
        /// </summary>
        private string _Text;

        public string Text
        {
            get { return this.key.Value; }
            set { _Text = this.key.Value; }
        }
        /// <summary>
        /// 文本框提示
        /// </summary>
        private string _Tip;

        public string Tip
        {
            set
            {
                _Tip = value;
                this.key.Attributes.Add("placeholder", _Tip);
            }
        }

        /// <summary>
        /// 输出数据
        /// </summary>
        public void LoadData()
        {
            string key = Request.Form[this.ClientID + "_key"];
            object result = this.DataSource(key);
            if (result != null)
            {
                Response.Write(_jssl.Serialize(result));
                Response.End();
            }
            else
            {

                Response.Write("[]");
                Response.End();
            }
        }
    }
}