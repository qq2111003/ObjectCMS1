using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using ObjectCMS.Common;
using System.Configuration;
using ObjectCMS.Model.System;
using System.Xml.Linq;
using System.Data;
using System.Web.Configuration;

namespace ObjectCMS.DAL
{
    public class SiteService : DataProviderBase
    {
        public void CreateSite(SysSite site)
        {
            #region 数据库部分
            //创建库
            this.MainDB.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "CreateNewSite", new SqlParameter("@SiteMark", site.SiteMark + "DB"));
           

            Cookie.SetCookie("curdb", site.SiteMark.ToLower());

            //初始化数据库
            string allSql = "";
            using (StreamReader sr = new StreamReader(new FileStream(System.Web.HttpContext.Current.Server.MapPath("/App_Data/EmptySite.sql"), FileMode.Open, FileAccess.Read)))
            {
                allSql = sr.ReadToEnd();
            }
            var sqlArray = allSql.Split(new string[] { "\r\nGO\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < sqlArray.Length; i++)
            {
                this.CurrentDB.ExecuteNonQuery(CommandType.Text, sqlArray[i]);
            }
            //记入主库
            site.Insert();
            #endregion
        }

        public void CopySite()
        {

        }






    }
}