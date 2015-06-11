using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.ModelConfig;
using System.Web.Script.Serialization;
using UserControls.Controls.jeasyui;
using System.Net;
using System.Text;
using System.IO;

namespace SiteWeb.Manage.Weixin
{
    public partial class SetWeixinMenu : System.Web.UI.Page
    {
        public JavaScriptSerializer _jssl = new JavaScriptSerializer();
        protected void Page_Load(object sender, EventArgs e)
        {

            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);

            //设置form初始值 
            FormBuilder1.Entity = new AppSettings();
            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = new List<FormItem>()
            {
                new FormItem(){Title = "appid", FieldName = "appid", FieldType =  FieldTypeInHTML.SingleLine, 
                    FieldModel= new ModelSingleLine(){ValidateTypes=new List<FieldValidate>(){FieldValidate.required}}},
                new FormItem(){Title = "appsecret", FieldName = "appsecret", FieldType =  FieldTypeInHTML.SingleLine, 
                    FieldModel= new ModelSingleLine(){ValidateTypes=new List<FieldValidate>(){FieldValidate.required}}}
            };
            #endregion

            if (IsPostBack)
            {
                #region 获取access_token
                var entity = (AppSettings)FormBuilder1.Entity;

                WriteLog("[appid:" + entity.appid.Trim() + ",appsecret:" + entity.appsecret.Trim() + "]");

                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + entity.appid.Trim() + "&secret=" + entity.appsecret.Trim();
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "get";
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);



                var atm = _jssl.Deserialize<access_tokenModel>(sr.ReadToEnd());

                WriteLog("[access_token:" + atm.access_token + "]");
                #endregion

                if (atm.access_token.IndexOf("errcode") > 0)
                {

                    Response.Write("<script>parent.Message.show('appid或appsecret设置错误','提示');parent.TreeGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();
                    return;
                }

                #region 删除老菜单

                url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + atm.access_token;

                webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "get";
                response = (HttpWebResponse)webRequest.GetResponse();

                sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                string result = sr.ReadToEnd();

                WriteLog("[deleteresult:" + result + "]");
                #endregion


                #region 设置新菜单

                var firstNodes = Node.GetALL("parentid=28", "Id");
                string menuJson = 
                            "{\"button\":[";
                foreach (var item in firstNodes)
                {
                    menuJson += "{\"name\":\""+item.Title+"\",\"sub_button\":[";
                    var secNodes = Node.GetALL("parentid=" + item.Id, "Id");
                    foreach (var subItem in secNodes)
                    {
                        menuJson += "{\"type\":\"click\",\"name\":\""+subItem.Title+"\",\"key\":\"Id_"+subItem.Id+"\"},";
                    }
                    menuJson += "]},";
                }
                menuJson += "]}";   

                WriteLog(menuJson);

                url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + atm.access_token;//请求的URL
                try
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(menuJson); // 转化
                    webRequest = (HttpWebRequest)WebRequest.Create(url);
                    webRequest.Method = "POST";
                    webRequest.ContentLength = byteArray.Length;

                    Stream newStream = webRequest.GetRequestStream();
                    newStream.Write(byteArray, 0, byteArray.Length); //写入参数
                    newStream.Close();
                    response = (HttpWebResponse)webRequest.GetResponse();
                    sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

                    result = sr.ReadToEnd();

                    WriteLog("[setnewresult:" + result + "]");

                    Response.Write("<script>parent.Message.show('设置成功','提示');parent.TreeGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();

                }
                catch (Exception ex)
                {

                    Response.Write("<script>parent.Message.show('" + ex + "','提示');parent.TreeGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();
                }


                #endregion



            }






        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="strMemo">内容</param>
        private void WriteLog(string strMemo)
        {
            string logPath = System.Configuration.ConfigurationManager.AppSettings["SysLog"].ToString();
            string filename = Server.MapPath(logPath + "/WeiXin.txt");
            if (!System.IO.Directory.Exists(Server.MapPath(logPath)))
                System.IO.Directory.CreateDirectory(Server.MapPath(logPath));
            System.IO.StreamWriter sr = null;
            try
            {
                if (!System.IO.File.Exists(filename))
                {
                    sr = System.IO.File.CreateText(filename);
                }
                else
                {
                    sr = System.IO.File.AppendText(filename);
                }
                sr.WriteLine(strMemo);
            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }
    }

    class ButtonModel
    {
        public string name { get; set; }
        private List<subButtonModel> _sub_button = new List<subButtonModel>();

        internal List<subButtonModel> sub_button
        {
            get { return _sub_button; }
            set { _sub_button = value; }
        }
    }
    class subButtonModel
    {
        public string type { get; set; }
        public string name { get; set; }
        public string key { get; set; }
    }
    class AppSettings
    {
        public string appid { get; set; }
        public string appsecret { get; set; }
    }
    class access_tokenModel
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }

}