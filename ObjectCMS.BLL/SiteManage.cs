using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.DAL;
using ObjectCMS.Model.System;

namespace ObjectCMS.BLL
{
    public class SiteManage
    {
        private readonly SiteService dal = new SiteService();
        public static readonly SiteManage Instance = new SiteManage();
        public void CreateSite(SysSite site)
        {
            dal.CreateSite(site);
        }
    }
}
