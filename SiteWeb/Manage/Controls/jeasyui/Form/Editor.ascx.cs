using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Common;

namespace UserControls.Controls.jeasyui.Form
{
    public partial class Editor : System.Web.UI.UserControl
    {
        public string Text
        {
            get { return tb_Editor.Text; }
            set { tb_Editor.Text = value; }
        }
        public Unit Width
        {
            get { return tb_Editor.Width; }
            set { tb_Editor.Width = value; }
        }
        public Unit Height
        {
            get { return tb_Editor.Height; }
            set { tb_Editor.Height = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.FindControl("KEHelper") == null)
            {
                System.Web.UI.Control header = Page.LoadControl(StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Helper/KEHelper.ascx"));
                header.ID = "KEHelper";
                this.Page.Header.Controls.Add(header);
            }
        }
    }
}