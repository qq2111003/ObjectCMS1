using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.System;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Threading;
using ObjectCMS.BLL;
using ObjectCMS.Model.ModelConfig;
using ObjectCMS.Common;

namespace SiteWeb.Manage
{
    public partial class Index : PageBase
    {
        protected int userid { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.HasPermission(-1, "view");
            string method = Request["method"];
            if (!string.IsNullOrEmpty(method))
            {

                MethodInfo methodInfo = this.GetType().GetMethod(method);
                methodInfo.Invoke(this, null);
                Response.End();
            }

        }


        public void GetChannel()
        {
            int pid = 0;
            int.TryParse(Request.Form["pid"], out pid);
            var lc = PermissionsManage.Instance.GetAllMenusForAdmin(this.LogonUserId, "parentid=" + pid);
            JavaScriptSerializer js = new JavaScriptSerializer();
            var json = js.Serialize(lc);
            Response.Write(json);
        }

        public void GetChannelWithSon()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            int pid = 0;
            int.TryParse(Request.Form["pid"], out pid);

            if (pid == 16)
            {
                GetNodeTree();
                return;
            }
            var allc = PermissionsManage.Instance.GetAllMenusForAdmin(this.LogonUserId);
            var rtValue = allc.Where(c => c.ParentId == pid);
            foreach (var item in rtValue)
            {
                item.Target = js.Serialize(allc.Where(c => c.ParentId == item.Id));
            }
            var json = js.Serialize(rtValue);
            Response.Write(json);
        }
        public void GetNodeTree()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            int pid = 0;
            var allc = PermissionsManage.Instance.GetAllNodeTreeForAdmin(this.LogonUserId);
            var rtValue = from a in allc
                          where a.ParentId == pid
                          select new SysMenu()
                          {
                              Id = a.Id,
                              ParentId = a.ParentId,
                              Title = a.Title,
                              Target = js.Serialize(from s in allc
                                                    where s.ParentId == a.Id
                                                    select new SysMenu()
                                                    {
                                                        Id = s.Id,
                                                        ParentId = s.ParentId,
                                                        Title = s.Title,
                                                        Url = "Model/" + (s.PageType ? "DataList" : "NodeDataEdit") + ".aspx?NodeId=" + s.Id
                                                    }),
                              Url = "Model/" + (a.PageType ? "DataList" : "NodeDataEdit") + ".aspx?NodeId=" + a.Id
                          };

            var json = js.Serialize(rtValue);
            Response.Write(json);
        }

        public void GetAllSite()
        {
            string siteOption = "";
            var allSite = SysSite.GetALL("1=1", "id desc");
            foreach (var item in allSite)
            {
                siteOption += "<option value=\"" + item.SiteMark + "\" " + (Cookie.GetCookie("curdb") == item.SiteMark?"selected":"") + ">" + item.Name + "</option>";
            }

            Response.Write(siteOption);
        }

        public void ChangeSite()
        {
            string sitemark = Request.Form["sitemark"];

            Cookie.SetCookie("curdb", sitemark);
        }
    }
}