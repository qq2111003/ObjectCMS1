using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using ObjectCMS.DataAccess;
using ObjectCMS.Common;
using System.Web.Script.Serialization;
using System.Reflection;

namespace SiteWeb.Interface
{
    /// <summary>
    /// DataHandler 的摘要说明
    /// </summary>
    public class DataHandler : IHttpHandler
    {
        HttpRequest Request;
        HttpResponse Response;
        JavaScriptSerializer _jssl = new JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Request = context.Request;
            Response = context.Response;

            if (!string.IsNullOrEmpty(Request["method"]))
            {
                this.GetType().GetMethod(Request["method"]).Invoke(this, null);
            }

        }

        public void BpjfSort()
        {
            int gsid = Request["gsid"].ToInt(-1);
            int count = Request["count"].ToInt(10);

            string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["XTJPoqWorld"].ConnectionString;

            SqlParameter parame = new SqlParameter("@GSID", SqlDbType.Int, 4);
            parame.Value = gsid;
            DataSet ds = SqlHelper.ExecuteDataSet(Conn, CommandType.StoredProcedure, "_Stat_GameGroupinfo", parame);
            var list = from d in ds.Tables[0].AsEnumerable().Take(count)
                       select new
                       {
                           GroupName = d.Field<string>("F_GroupName"),
                           GroupMaster = d.Field<string>("F_GroupMaster"),
                           AverageScore = d.Field<int>("F_AverageScore"),
                           ZoneName = d.Field<string>("F_ZoneName") ?? ""
                       };
            Response.Write("loadBpjfSort(" + _jssl.Serialize(list) + ");");

        }

        public void GradeSort()
        {
            int gsid = Request["gsid"].ToInt(-1);
            int role = Request["role"].ToInt(-1);

            int count = Request["count"].ToInt(10);

            string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["XTJStatSystem"].ConnectionString;


            SqlParameter[] pars = new SqlParameter[]{
                new SqlParameter("@AreaID", SqlDbType.Int),
                new SqlParameter("@RoleType", SqlDbType.Int),
                new SqlParameter("@StatType", SqlDbType.Int),
                new SqlParameter("@code", SqlDbType.Int)
			};
            pars[0].Value = gsid;
            pars[1].Value = role;
            pars[2].Value = 1;
            pars[3].Direction = ParameterDirection.Output;


            DataSet ds = SqlHelper.ExecuteDataSet(Conn, CommandType.StoredProcedure, "_Stat_RoleTop", pars);
            var list = from d in ds.Tables[0].AsEnumerable().Take(count)
                       select new
                       {
                           RoleName = d.Field<string>("RoleName"),
                           RoleId = GetRoleName(d.Field<string>("RoleType")),
                           RoleLevel = d.Field<int>("RoleLevel"),
                           RoleArea = d.Field<string>("RoleArea")
                       };
            Response.Write("loadGradeSort(" + _jssl.Serialize(list) + ");");
        }
        public void HeroesSort()
        {

            int count = Request["count"].ToInt(10);

            string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["XTJActiveDB"].ConnectionString;
            SqlParameter[] pamrs ={
                                    new SqlParameter("@Type",SqlDbType.VarChar,2),
                                    new SqlParameter("@Code",SqlDbType.Int)
                                 };
            pamrs[0].Direction = ParameterDirection.Output;
            pamrs[1].Direction = ParameterDirection.Output;


            DataSet ds = SqlHelper.ExecuteDataSet(Conn, CommandType.StoredProcedure, "_ACT_Rankings", pamrs);
            var list = from d in ds.Tables[0].AsEnumerable().Take(count)
                       select new
                       {
                           No = d.Field<int>("F_No"),
                           UserName = FormatUserName(d.Field<string>("F_UserName")),
                           Total = d.Field<long>("F_Total")
                       };
            Response.Write("loadHeroesSort(" + _jssl.Serialize(list) + ");");

        }

        private static string FormatUserName(string str)
        {
            str = str.Trim();
            try
            {

                if (str.Length > 4)
                {
                    str = str.Substring(0, str.Length - 4) + "****";
                }
                else
                {
                    str = str.Substring(0, str.Length - 2) + "**";
                }
            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;
        }

        public string GetRoleName(string roleType)
        {
            switch (roleType)
            {
                case "1":
                    return "游侠";
                case "2":
                    return "力士";
                case "3":
                    return "刺客";
                case "4":
                    return "术士";
                case "5":
                    return "巫师";
                default:
                    return "其它职业";
            }
        }

        public void GetAllZone()
        {

            string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["XTJStatSystem"].ConnectionString;
            DataSet ds = SqlHelper.ExecuteDataSet(Conn, "SELECT F_Name,ISNULL(F_GSID,-1) as F_GSID FROM [stat]..T_Group with(nolock) where F_Name not like '%维护%' and F_name not like '%体验区%' and f_name not like '%帮战服务器%' ORDER BY F_Order");
            var list = from d in ds.Tables[0].AsEnumerable()
                       select new
                       {
                           Name = d.Field<string>("F_Name"),
                           GSID = d.Field<int>("F_GSID")
                       };
            Response.Write("loadZone("+_jssl.Serialize(list)+");");

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}