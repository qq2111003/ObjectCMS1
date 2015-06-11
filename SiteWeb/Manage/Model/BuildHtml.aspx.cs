using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ObjectCMS.TemplateEngine;
using System.Reflection;
using ObjectCMS.Common;

namespace SiteWeb.Manage.Model
{
    public partial class BuildHtml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["nodeid"]))
            {

                int nodeId = 0;
                int.TryParse(Request["nodeid"], out nodeId);
                BuildNode.doBuild(nodeId);
            }
            if (!string.IsNullOrEmpty(Request["nodeids"]))
            {
                var nodeIds = from s in Request["nodeids"].Split(',')
                              where s != "0"
                              select s;

                BuildNode.doBuild(nodeIds.ToArray());
            }

        }
    }
}