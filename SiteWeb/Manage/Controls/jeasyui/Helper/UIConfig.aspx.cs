using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web.Script.Serialization;

namespace UserControls.Plugin.jeasyui
{
    public partial class UIConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected string LoadData()
        {
            var data = from x in XElement.Load(Server.MapPath("JeasyUI.config")).Elements()
                       select new
                       {
                           name = x.Name.LocalName,
                           value = x.Value,
                           editor = "text"
                       };
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(data);
        }
    }
}