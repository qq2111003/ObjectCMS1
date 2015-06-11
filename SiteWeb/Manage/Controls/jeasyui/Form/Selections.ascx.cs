using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace UserControls.Controls.jeasyui.Form
{
    public partial class Selections : System.Web.UI.UserControl
    {
        protected string Html = "";
        protected StringBuilder sb = new StringBuilder();
        private SelectionType _Type = SelectionType.select;

        public SelectionType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private string _SelectedValue;

        public string SelectedValue
        {
            get
            {
                //取值

                return _SelectedValue;
            }
            set { _SelectedValue = value; }
        }

        public string[] Items { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Items == null)
            {
                Items = new string[] { };
            }
            var a = from i in Items select new { Value = i, Text = i };

            if (Items.Length > 0)
            {
                if (Items[0].IndexOf('|') > 0)
                {
                    a = from i in Items select new { Value = i.Split('|')[0], Text = i.Split('|')[1] };
                }
            }

            if (Type == SelectionType.select)
            {
                DropDownList ddl = new DropDownList();
                ddl.ID = "Selection";
                ddl.DataSource = a;
                ddl.DataValueField = "Value";
                ddl.DataTextField = "Text";
                ddl.DataBind();
                ddl.SelectedValue = SelectedValue;
                this.Controls.Add(ddl);
            }
            else if (Type == SelectionType.checkbox)
            {
                Literal l = new Literal();
                string[] selectedValues = SelectedValue != null ? SelectedValue.Split(',') : new string[0];

                sb.AppendLine("<table id=\"FormBuilder1_Selections_3_Selection\" border=\"0\">");
                sb.AppendLine("    <tr>");
                for (int i = 0; i < Items.Length; i++)
                {
                    sb.AppendLine("        <td>");
                    sb.AppendLine("            <input id=\"" + this.ClientID + "_Selection_" + i + "\" " + (selectedValues.Contains(Items[i]) ? "checked=\"checked\"" : "") + " type=\"checkbox\" name=\"" + this.ClientID.Replace("_", "$") + "$Selection\" value=\"" + Items[i] + "\"><label");
                    sb.AppendLine("                for=\"" + this.ClientID + "_Selection_" + i + "\">" + Items[i] + "</label>");
                    sb.AppendLine("        </td>");
                }
                sb.AppendLine("    </tr>");
                sb.AppendLine("</table>");
                l.Text = sb.ToString();
                this.Controls.Add(l);
            }
            else
            {
                RadioButtonList rbl = new RadioButtonList();
                rbl.ID = "Selection";
                rbl.DataSource = a;
                rbl.DataValueField = "Value";
                rbl.DataTextField = "Text";
                rbl.DataBind();
                rbl.SelectedValue = SelectedValue;
                rbl.RepeatDirection = RepeatDirection.Horizontal;
                this.Controls.Add(rbl);
            }
        }
    }

    public enum SelectionType
    {
        checkbox,
        select,
        radio
    }
}