using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Common;
using ObjectCMS.BLL;
using System.Reflection;
using ObjectCMS.Model.System;

namespace SiteWeb.Manage
{
    public partial class Login : System.Web.UI.Page
    {

        protected string position = "";
        protected string rtMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = Request["method"];
            if (!string.IsNullOrEmpty(method))
            {
                MethodInfo methodInfo = this.GetType().GetMethod(method);
                methodInfo.Invoke(this, null);
                Response.End();
            }
            if (!IsPostBack)
            {
                Cookie.SetCookie("AdminManage", "", -1);
            }
            else
            {
                CheckLogin();
            }
        }

        public void CheckLogin()
        {
            if (check_p.Visible && Session["CheckCode"] == null)
            {
                position = "check_code";
                rtMsg = "验证码过期";
            }
            else if (check_p.Visible && Session["CheckCode"].ToString().ToUpper() != check_code.Text.ToUpper())
            {
                position = "check_code";
                rtMsg = "验证码错误";
            }
            else
            {
                string LoginName = user_name.Text;
                string LoginPwd = user_password.Text;
                LoginPwd = MD5.EncryptStringMD5(LoginPwd);
                int status = 0;
                var result = PermissionsManage.Instance.AdminLoginCheck(LoginName, LoginPwd, out status);
                if (status == -1)
                {
                    Cookie.SetCookie("AdminManage", result.Id.ToString(), 1);

                    #region
                    if (string.IsNullOrEmpty(Cookie.GetCookie("curdb")))
                    {
                        string firstSiteMark = SysSite.GetOne("1=1").SiteMark;
                        Cookie.SetCookie("curdb", firstSiteMark);
                    }
                    #endregion


                    Response.Redirect("index.aspx", true);
                }
                else if (status == 0)
                {
                    position = "tUserName";
                    rtMsg = "用户名不存在";

                }
                else if (status == 1)
                {
                    position = "tUserPwd";
                    rtMsg = "密码错误";

                }
                else if (status == 2)
                {
                    position = "tUserName";
                    rtMsg = "账号被禁用";
                }

            }
            Response.Write("<script>alert('" + rtMsg + "');</script>");
        }

        public void GetValiCode()
        {
            string checkCode = Utils.CreateRandomCode(4);
            Session["CheckCode"] = checkCode;
            Utils.CreateImage(checkCode);
        }
    }

    public class PageBase : System.Web.UI.Page
    {
        /// <summary>
        /// 登录用户信息
        /// </summary>
        protected int LogonUserId
        {
            get { return string.IsNullOrEmpty(Cookie.GetCookie("AdminManage")) ? 0 : Cookie.GetCookie("AdminManage").ToInt(); }
            set { }
        }
        /// <summary>
        /// 构造函数，初始化PageBase类的新实例。
        /// </summary>
        public PageBase()
        {
            this.Load += new EventHandler(PageBase_Load);
        }

        #region PageBase_Load
        /// <summary>
        /// 加载页面时进行的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageBase_Load(object sender, EventArgs e)
        {
            this.Visible = true;
            if (this.LogonUserId == 0)
            {
                this.Visible = false;
                Response.Redirect("/Manage/Login.aspx");
            }
        }

        public bool HasPermission(int menuid, string operation)
        {
            var Visible = PermissionsManage.Instance.HasPermission(this.LogonUserId, menuid);
            if (Visible || menuid == -1)
            {
                if (operation.ToLower() == "view")
                {
                    return true;
                }
                else
                {
                    return PermissionsManage.Instance.HasPermission(this.LogonUserId, menuid, operation);
                }
            }
            return false;
        }
        #endregion
    }
}