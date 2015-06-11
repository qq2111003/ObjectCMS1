using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;

namespace WeixinApp
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //获取access_token
        protected void _btnAccessToken_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_txtAppid.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('请输入appid');", true);
                return;
            }
            if (string.IsNullOrEmpty(_txtAppsecret.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('请输入appsecret');", true);
                return;
            }
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + _txtAppid.Text.Trim() + "&secret=" + _txtAppsecret.Text.Trim();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "get";
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            _lblMsg.Text = "返回结果：" + sr.ReadToEnd();

        }




        //设置菜单
        protected void _btnSetMenu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_txtacetoken.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('请输入access_token');", true);
                return;
            }
            if (string.IsNullOrEmpty(_txtMenu.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('请输入自定义菜单内容');", true);
                return;
            }

            string padata = _txtMenu.Text.Trim();
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + _txtacetoken.Text.Trim();//请求的URL
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentLength = byteArray.Length;

                Stream newStream = webRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length); //写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                _lblMsg2.Text = "返回结果：" + sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //删除菜单
        protected void _btnDelMenu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_txtacetoken.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('请输入access_token');", true);
                return;
            }
            string url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + _txtacetoken.Text.Trim();

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "get";
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            _lblMsg2.Text = "返回结果：" + sr.ReadToEnd();
        }

    }
}