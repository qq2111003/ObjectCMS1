using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using ObjectCMS.Common;
using ObjectCMS.BLL;
using ObjectCMS.Model.ModelConfig;
using ObjectCMS.TemplateEngine.Common;
using System.Text.RegularExpressions;

namespace ObjectCMS.TemplateEngine.Core
{
    public class lPage
    {

        public static string ReplaceLabeltoData(string labelHTML, Hashtable[] Param, ref int ListPageCount)
        {

            int nodeId = ParamController.GetParam("nodeid", Param).ToInt();
            string nodeIds = ParamController.GetParam("nodeids", Param);
            int pageSize = ParamController.GetParam("pagesize",  Param ).ToInt();
            int pageIndex = ParamController.GetParam("pageindex", Param).ToInt();
            string sql = ParamController.GetParam("sql", Param );

            Node node = Node.GetOne(nodeId);
            string tableName = UserModel.GetOne(node.UserModelId).TableName;
            string where = "NodeId=" + nodeId + " and Enable='True' ";

            if (!string.IsNullOrEmpty(nodeIds))
            {
                where = "NodeId in (" + nodeIds + ") and Enable='True' ";
            }

            if (!string.IsNullOrEmpty(sql))
            {
                where += " and " + sql;
            }

            int recordCount = TemplateEngineManage.Instance.GetRecordCount(tableName, where);

            return LabelPageView(pageSize, pageIndex, recordCount, ParamController.GetParam("pagestr", Param ), labelHTML, ref ListPageCount);
        }


        /// <summary>
        /// 分页样式4(纯分页)
        /// </summary>
        /// <returns></returns>
        public static string LabelPageView(int pageSize, int pageIndex, int recordCount, string pageStr, string LabelHTML, ref int ListPageCount)
        {
            int pageCount = 0;
            if (recordCount % pageSize == 0)
            {
                pageCount = recordCount / pageSize;
            }
            else
            {
                pageCount = (recordCount / pageSize) + 1;
            }
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (pageCount == 0)
            {
                pageCount = 1;
            }

            int fromPage, toPage;
            string temp = "";
            int pages = pageCount;
            if (pages <= 10)
            {
                fromPage = 1;
                toPage = pages;
            }
            else
            {
                if (pageIndex <= 3 && pages - pageIndex >= 8)
                {
                    fromPage = 1;
                    toPage = 10;
                }
                else if (pageIndex > 3 && pages - pageIndex < 8)
                {
                    fromPage = pages - 9;
                    toPage = pages;
                }
                else
                {
                    fromPage = pageIndex - 2;
                    toPage = pageIndex + 7;
                }
            }


            LabelHTML = LabelHTML.IReplace("{firstUrl}", pageStr + ".html")
                .IReplace("{prevUrl}", pageStr + (pageIndex == 2 || pageIndex == 1 ? "" : "_" + Convert.ToString(pageIndex - 1)) + ".html")
                .IReplace("{nextUrl}", pageStr + (pageCount <= 1 ? ".html" : "_" + Convert.ToString(pageIndex < pageCount ? pageIndex + 1 : pageCount) + ".html"))
                .IReplace("{lastUrl}", pageStr + (pageCount <= 1 ? ".html" : "_" + Convert.ToString(pageCount) + ".html"))
                .IReplace("{pageCount}", pageCount + "")
                .IReplace("{pageIndex}", pageIndex + "")
                .IReplace("{pageSize}", pageSize + "")
                .IReplace("{recordCount}", recordCount + "")
                .IReplace("{pageStr}", pageStr);

            Regex regex = new Regex(@"(?s)<for>(.+?)</for\>", RegexOptions.IgnoreCase);
            Match m = regex.Match(LabelHTML);
            while (m.Success)
            {
                string PageItemStr = "";
                ListPageCount = 0;
                for (int i = fromPage; i <= toPage; i++)
                {
                    if (pageCount > ListPageCount)
                    {
                        ListPageCount = pageCount;
                    }
                    PageItemStr += m.Result("$1").IReplace("{itemUrl}", pageStr + (i == 1 ? "" : "_" + i) + ".html").IReplace("{item}", i + "");
                }
                LabelHTML = LabelHTML.IReplace(m.Result("$0"), PageItemStr);
                m = m.NextMatch();
            }
            return LabelHTML;
        }

    }
}
