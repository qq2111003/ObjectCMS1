using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.System;
using System.Text;
using System.Web.Security;

namespace SiteWeb.Manage
{
    public partial class Default : System.Web.UI.Page
    {

        protected int userid { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["AdminManage"] == null)
            //{
            //    Response.Redirect("login.aspx");
            ////}
            //Session["AdminManage"] = "admin";

            //string pwd = FormsAuthentication.HashPasswordForStoringInConfigFile("ywjsObject$#@", "sha1");
        }

        public string LoadChannel()
        {

            IList<SysMenu> lc = SysMenu.GetALL("1=1", "id");
            var RootChannel = from rc in lc
                              where rc.ParentId == 0
                              select rc;

            StringBuilder sb = new StringBuilder();
            foreach (var item in RootChannel)
            {
                if (!item.Enable)
                {
                    continue;
                }
                sb.AppendLine("<div title=\"" + item.Title + "\">");
                foreach (var sonItem in (from rc in lc
                                         where rc.ParentId == item.Id
                                         select rc))
                {
                    if (!item.Enable)
                    {
                        continue;
                    }
                    var sonsonList = from rc in lc
                                     where rc.ParentId == sonItem.Id
                                     select rc;

                    sb.AppendLine("<div class=\"menu_bac\">");
                    sb.AppendLine("    <div class=\"menuTitle" + (sonsonList.Count() > 0 ? "1" : "") + "\">");
                    if (sonsonList.Count() > 0)
                    {
                        sb.AppendLine("        <a href=\"javascript:void(0);\" style=\"font-weight: bold;\" title=\"" + sonItem.Title + "\">" + sonItem.Title + "</a>");
                    }
                    else
                    {
                        sb.AppendLine("        <a href=\"javascript:void(0);\" src=\"" + sonItem.Url + "\" class=\"cs-navi-tab\" style=\"font-weight: bold;\" title=\"" + sonItem.Title + "\">" + sonItem.Title + "</a>");
                    }
                    sb.AppendLine("    </div>");
                    sb.AppendLine("    <div class=\"menuContent\">");
                    sb.AppendLine("        <div class=\"treescss\">");
                    sb.AppendLine("            <ul>");
                    foreach (var sonsonItem in sonsonList)
                    {
                        if (!item.Enable)
                        {
                            continue;
                        }
                        sb.AppendLine("                <li>");
                        sb.AppendLine("                    <a href=\"javascript:void(0);\" src=\"" + sonsonItem.Url + "\" class=\"cs-navi-tab\" title=\"" + sonsonItem.Title + "\">");
                        sb.AppendLine("                                    " + sonsonItem.Title + "</a>");
                        sb.AppendLine("                </li>");
                    }

                    sb.AppendLine("            </ul>");
                    sb.AppendLine("        </div>");
                    sb.AppendLine("    </div>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("<div class='clear'></div>");
                }
                sb.AppendLine("</div>");
            }


            return sb.ToString();
        }
    }
}