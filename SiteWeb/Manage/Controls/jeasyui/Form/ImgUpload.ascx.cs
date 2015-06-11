using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Common;

namespace UserControls.Controls.jeasyui.Form
{
    public partial class ImgUpload : System.Web.UI.UserControl
    {
        public string Path
        {
            get { return tb_ImgPath.Text; }
            set { tb_ImgPath.Text = value; }
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