using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserControls.Controls.jeasyui.Form
{
    public partial class CodeEditor : System.Web.UI.UserControl
    {
        public string Text
        {
            get { return tb_CodeEditor.Text; }
            set { tb_CodeEditor.Text = value; }
        }
        public Unit Width
        {
            get { return tb_CodeEditor.Width; }
            set { tb_CodeEditor.Width = value; }
        }
        public Unit Height
        {
            get { return tb_CodeEditor.Height; }
            set { tb_CodeEditor.Height = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}