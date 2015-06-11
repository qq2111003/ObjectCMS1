using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.DataAccess;
using System.Configuration;
using System.Web;
using ObjectCMS.Common;

namespace ObjectCMS.DAL
{
    public class DataProviderBase
    {
        protected SqlHelper MainDB
        {
            get { return DataHelperFactory.Create(System.Configuration.ConfigurationManager.ConnectionStrings["maindb"].ConnectionString); }
        }
        protected SqlHelper CurrentDB
        {
            get
            {
                string constr = System.Configuration.ConfigurationManager.AppSettings["sitedbconstrrule"];
                constr = constr.IReplace("{SiteMark}", Cookie.GetCookie("curdb"));
                return DataHelperFactory.Create(constr);
            }
        }

    }
}
