using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.ModelConfig;
using ObjectCMS.TemplateEngine.Core;
using System.Collections;
using ObjectCMS.BLL;
using ObjectCMS.Common;
using System.Web;
using System.Data;
using XHR.Common;

namespace ObjectCMS.TemplateEngine
{
    public class BuildNode
    {
        public static void doBuild(string[] nodeIds)
        {
            int AllPageCount = nodeIds.Length;
            int CurrPageNum = 0;
            int pagecount = 0;
            HtmlProgressBar.Start();
            DateTime startTime = DateTime.Now;

            for (int i = 0; i < nodeIds.Length; i++)
            {
                Hashtable[] param_arr = new Hashtable[3];
                Node node = Node.GetOne(nodeIds[i].ToInt());
                int temp = 0;
                if (string.IsNullOrEmpty(node.DetailPageUrl) && string.IsNullOrEmpty(node.ListPageUrl))
                {
                    CurrPageNum++;
                    continue;
                }

                if (!node.PageType)
                {
                    param_arr[0] = new Hashtable();
                    param_arr[0].Add("nodeid", nodeIds[i]);
                    new TemplateHelper().BuilderHTML(node.ListTemplateId, node.ListPageUrl, param_arr, ref temp);
                    pagecount++;
                    HtmlProgressBar.Roll(pagecount.ToString() + "页", CurrPageNum * 100 / AllPageCount);
                }
                else
                {

                    DataTable Detailsdt = null;
                    if (!string.IsNullOrEmpty(node.DetailPageUrl) && node.DetailPageUrl.Trim().Length > 0 && !node.DetailPageUrl.StartsWith("/.html"))
                    {
                        string tableName = UserModel.GetOne(node.UserModelId).TableName;
                        Detailsdt = TemplateEngineManage.Instance.GetAllData(nodeIds[i].ToInt(), tableName);
                    }


                    if (!string.IsNullOrEmpty(node.ListPageUrl) && node.ListPageUrl.Trim().Length > 0)
                    {
                        for (int j = 1,
                            ListPageCount = -1;
                            ListPageCount == -1 || j < ListPageCount + 1;
                            j++)
                        {
                            param_arr[0] = new Hashtable();
                            param_arr[0].Add("nodeid", nodeIds[i]);
                            param_arr[0].Add("pageindex", j);
                            string pageStr = node.ListPageUrl.Substring(node.ListPageUrl.LastIndexOf('/') + 1).Split('.')[0];
                            param_arr[0].Add("pagestr", pageStr);
                            string listPageUrl = node.ListPageUrl;
                            if (j > 1)
                            {
                                listPageUrl = node.ListPageUrl.Replace("/" + pageStr + ".", "/" + pageStr + "_" + j + ".");
                            }
                            new TemplateHelper().BuilderHTML(node.ListTemplateId, listPageUrl, param_arr, ref ListPageCount);
                            pagecount++;
                            HtmlProgressBar.Roll(pagecount.ToString() + "页", CurrPageNum * 100 / AllPageCount);
                        }
                    }


                    if (!string.IsNullOrEmpty(node.DetailPageUrl) && node.DetailPageUrl.Trim().Length > 0 && !node.DetailPageUrl.StartsWith("/.html"))
                    {
                        for (int k = 0; k < Detailsdt.Rows.Count; k++)
                        {
                            param_arr[0] = new Hashtable();
                            param_arr[0].Add("nodeid", nodeIds[i]);
                            param_arr[0].Add("id", Detailsdt.Rows[k][0]);
                            new TemplateHelper().BuilderHTML(node.DetailTemplateId, node.DetailPageUrl, param_arr, ref temp);
                            pagecount++;
                            HtmlProgressBar.Roll(pagecount.ToString() + "页 " + CurrPageNum + "/" + AllPageCount, CurrPageNum * 100 / AllPageCount);
                        }
                    }
                }

                CurrPageNum++;

            }

            DateTime endTime = DateTime.Now;

            var takeTime = endTime - startTime;

            HttpContext.Current.Response.Write("<script>parent.Message.show('共生成" + AllPageCount + "个栏目," + pagecount + "个页面;耗时" + takeTime.TotalMilliseconds + "毫秒,平均" + takeTime.TotalMilliseconds / pagecount + "毫秒生成一个页面','提示');parent.UIDialog.Close();</script>");
            HttpContext.Current.Response.End();
        }
        public static void doBuild(int nodeId)
        {
            int AllPageCount = 1;
            int CurrPageNum = 0;
            Node node = Node.GetOne(nodeId);
            Hashtable[] param_arr = new Hashtable[3];

            HtmlProgressBar.Start();

            DateTime startTime = DateTime.Now;
            int temp = 0;
            if (!node.PageType)
            {
                param_arr[0] = new Hashtable();
                param_arr[0].Add("nodeid", nodeId);
                new TemplateHelper().BuilderHTML(node.ListTemplateId, node.ListPageUrl, param_arr, ref temp);

                HtmlProgressBar.Roll("完成", 100);
            }
            else
            {

                DataTable Detailsdt = null;
                if (!string.IsNullOrEmpty(node.DetailPageUrl)&&node.DetailPageUrl.Trim().Length > 0 && !node.DetailPageUrl.StartsWith("/.html"))
                {
                    string tableName = UserModel.GetOne(node.UserModelId).TableName;
                    Detailsdt = TemplateEngineManage.Instance.GetAllData(nodeId, tableName);
                }


                if (!string.IsNullOrEmpty(node.ListPageUrl)&&node.ListPageUrl.Trim().Length > 0)
                {
                    for (int j = 1,
                        ListPageCount = -1;
                        ListPageCount == -1 || j < ListPageCount + 1;
                        j++)
                    {
                        if (j == 2)
                        {
                            if (Detailsdt != null)
                            {
                                AllPageCount = Detailsdt.Rows.Count + ListPageCount;
                            }
                            else
                            {
                                AllPageCount = ListPageCount;
                            }
                        }
                        param_arr[0] = new Hashtable();
                        param_arr[0].Add("nodeid", nodeId);
                        param_arr[0].Add("pageindex", j);
                        string pageStr = node.ListPageUrl.Substring(node.ListPageUrl.LastIndexOf('/') + 1).Split('.')[0];
                        param_arr[0].Add("pagestr", pageStr);
                        string listPageUrl = node.ListPageUrl;
                        if (j > 1)
                        {
                            listPageUrl = node.ListPageUrl.Replace("/" + pageStr + ".", "/" + pageStr + "_" + j + ".");
                        }
                        new TemplateHelper().BuilderHTML(node.ListTemplateId, listPageUrl, param_arr, ref ListPageCount);
                        CurrPageNum++;

                        HtmlProgressBar.Roll(CurrPageNum.ToString() + "/" + AllPageCount + "页", CurrPageNum * 100 / AllPageCount);
                    }
                }


                if (!string.IsNullOrEmpty(node.DetailPageUrl) && node.DetailPageUrl.Trim().Length > 0 && !node.DetailPageUrl.StartsWith("/.html"))
                {
                    for (int k = 0; k < Detailsdt.Rows.Count; k++)
                    {
                        param_arr[0] = new Hashtable();
                        param_arr[0].Add("nodeid", nodeId);
                        param_arr[0].Add("id", Detailsdt.Rows[k][0]);
                        new TemplateHelper().BuilderHTML(node.DetailTemplateId, node.DetailPageUrl, param_arr, ref temp);
                        CurrPageNum++;
                        HtmlProgressBar.Roll(CurrPageNum.ToString() + "/" + AllPageCount + "页", CurrPageNum * 100 / AllPageCount);
                    }
                }

            }

            DateTime endTime = DateTime.Now;

            var takeTime = endTime - startTime;

            HttpContext.Current.Response.Write("<script>parent.Message.show('共耗时" + takeTime.TotalMilliseconds + "毫秒,平均" + takeTime.TotalMilliseconds / AllPageCount + "毫秒生成一个页面','提示');parent.UIDialog.Close();</script>");
            HttpContext.Current.Response.End();
        }


        public static void BuildMenuPage(int nodeId)
        {
            Node node = Node.GetOne(nodeId);
            Hashtable[] param_arr = new Hashtable[3];
            int temp = 0;
            if (!node.PageType)
            {
                param_arr[0] = new Hashtable();
                param_arr[0].Add("nodeid", nodeId);
                new TemplateHelper().BuilderHTML(node.ListTemplateId, node.ListPageUrl, param_arr, ref temp);
            }
        }

        public static void BuildListPage(int nodeId, int id)
        {
            Node node = Node.GetOne(nodeId);
            Hashtable[] param_arr = new Hashtable[3];


            int temp = 0;
            if (node.PageType)
            {
                DataTable Detailsdt = null;
                if (!string.IsNullOrEmpty(node.DetailPageUrl) && node.DetailPageUrl.Trim().Length > 0 && !node.DetailPageUrl.StartsWith("/.html"))
                {
                    string tableName = UserModel.GetOne(node.UserModelId).TableName;
                    Detailsdt = TemplateEngineManage.Instance.GetAllData(nodeId, tableName);
                }


                if (!string.IsNullOrEmpty(node.ListPageUrl) && node.ListPageUrl.Trim().Length > 0)
                {
                    for (int j = 1,
                        ListPageCount = -1;
                        ListPageCount == -1 || j < ListPageCount + 1;
                        j++)
                    {
                        param_arr[0] = new Hashtable();
                        param_arr[0].Add("nodeid", nodeId);
                        param_arr[0].Add("pageindex", j);
                        string pageStr = node.ListPageUrl.Substring(node.ListPageUrl.LastIndexOf('/') + 1).Split('.')[0];
                        param_arr[0].Add("pagestr", pageStr);
                        string listPageUrl = node.ListPageUrl;
                        if (j > 1)
                        {
                            listPageUrl = node.ListPageUrl.Replace("/" + pageStr + ".", "/" + pageStr + "_" + j + ".");
                        }
                        new TemplateHelper().BuilderHTML(node.ListTemplateId, listPageUrl, param_arr, ref ListPageCount);
                    }
                }


                if (!string.IsNullOrEmpty(node.DetailPageUrl) && node.DetailPageUrl.Trim().Length > 0 && !node.DetailPageUrl.StartsWith("/.html") && id > 0)
                {
                    param_arr[0] = new Hashtable();
                    param_arr[0].Add("nodeid", nodeId);
                    param_arr[0].Add("id", id);
                    new TemplateHelper().BuilderHTML(node.DetailTemplateId, node.DetailPageUrl, param_arr, ref temp);

                }

            }
        }
    }
}
