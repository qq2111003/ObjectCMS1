using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Common;
using System.Reflection;

namespace SiteWeb.Manage.Controls.jeasyui
{
    public partial class ProcessBar : System.Web.UI.UserControl
    {
        private int width = 400;
        public int Width
        {
            get { return width; }
            set { width = value; }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AllPageCount"] = 0;
            Session["CurrPageNum"] = 0;
            string method = Request["method"];

            if (!string.IsNullOrEmpty(method) && method == "currPercent")
            {
                MethodInfo methodInfo = this.GetType().GetMethod(method);
                methodInfo.Invoke(this, null);
                Response.End();
            }

            if (this.Page.FindControl("UIHeader") == null)
            {
                System.Web.UI.Control header = Page.LoadControl(StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Helper/UIHeader.ascx"));
                header.ID = "UIHeader";
                this.Page.Header.Controls.Add(header);
            }
        }

        public void currPercent()
        {
            if (Session["AllPageCount"].ToInt() == 0)
            {
                Response.Write(100);
            }
            Response.Write((int)Session["CurrPageNum"].ToInt() * 100 / Session["AllPageCount"].ToInt());
        }
    }
}