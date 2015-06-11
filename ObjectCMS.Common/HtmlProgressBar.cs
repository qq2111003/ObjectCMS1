using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace XHR.Common
{
    /// <summary>
    /// 网页进度条
    /// </summary>
    public class HtmlProgressBar
    {
        /// <summary>
        /// 进度条的初始化
        /// </summary>
        public static void Start()
        {
            Start("正在加载...");
        }
        /// <summary>
        /// 进度条的初始化
        /// </summary>
        /// <param name="msg">最开始显示的信息</param>
        public static void Start(string msg)
        {
            string str = "";
            str += "<html>";
            str += "<head>";
            str += "    <link href=\"../plugin/jquery-easyui/themes/gray/easyui.css\" rel=\"stylesheet\" type=\"text/css\">";
            str += "    <script src=\"../js/jquery-1.8.0.min.js\" type=\"text/javascript\"></script>";
            str += "    <script>";
            str += "        function go(msg,val) {";
            str += "            $('.progressbar-text').text(msg+'  '+val + '%');";
            str += "            $('.progressbar-value').css('width', val + '%');";
            str += "        }";
            str += "    </script>";
            str += "</head>";
            str += "<body>";
            str += "    <div id=\"p\" class=\"easyui-progressbar progressbar\" style=\"width: 398px; height: 20px;\">";
            str += "        <div class=\"progressbar-text\" style=\"width: 398px; height: 20px; line-height: 20px;\">";
            str += "            0%</div>";
            str += "        <div class=\"progressbar-value\" style=\"width: 0%; height: 20px; line-height: 20px;\">";
            str += "            <div class=\"progressbar-text\" style=\"width: 398px; height: 20px; line-height: 20px;\">";
            str += "                0%</div>";
            str += "        </div>";
            str += "    </div>";
            str += "</body>";
            str += "</html>";

            HttpContext.Current.Response.Write(str);
            HttpContext.Current.Response.Flush();
        }
        /// <summary>
        /// 滚动进度条
        /// </summary>
        /// <param name="Msg">在进度条上方显示的信息</param>
        /// <param name="Pos">显示进度的百分比数字</param>
        public static void Roll(string Msg, int Pos)
        {
            string jsBlock = "<script language=\"javascript\">go('" + Msg + "'," + Pos + ");</script>";
            HttpContext.Current.Response.Write(jsBlock);
            HttpContext.Current.Response.Flush();
        }
    }
}
