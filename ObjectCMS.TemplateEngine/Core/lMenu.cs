using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;

namespace XHR.Template
{
    public class lMenu
    {
        static mMenu mM = new mMenu();
        static bMenu bM = new bMenu();
        public static string ReplaceLabeltoData(string labelHTML, Hashtable[] param_arr)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds;
            int Top = 0;
            string orderID = ParamController.GetParam("orderID", param_arr);


            if (ParamController.GetParam("parentid", param_arr) != null)//找下级模式 如:头部多级导航
            {
                if (orderID != null)
                {
                    ds = bM.MenuList(ParamController.GetParam("parentid", param_arr).ToInt(), orderID);
                }
                else
                {
                    ds = bM.MenuList(ParamController.GetParam("parentid", param_arr).ToInt());
                }

                //ds = bM.MenuList(ParamController.GetParam("parentid", param_arr).ToInt());
            }
            else//找上级模式 如:面包屑导航
            {
                ds = bM.MenuReader(ParamController.GetParam("id", param_arr).ToInt());
            }
            if (ParamController.GetParam("top", param_arr) != null)
            {
                Top = ParamController.GetParam("top", param_arr).ToInt();
            }
            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "IsDeleted=0";
            dv.Sort = "" + (ParamController.GetParam("sort", param_arr) != null ? ParamController.GetParam("sort", param_arr) : "sort asc") + "";


            string xmlFilePath = "";
            bLanguage bl = new bLanguage();
            DataTable dt_curLanguage = bl.LanguageCurList().Tables[0];
            if (dt_curLanguage.Rows.Count > 0)
                xmlFilePath = dt_curLanguage.Rows[0]["filepath"] + "";


            for (int i = 0; i < dv.Count; i++)
            {
                XHR.Common.XmlDoc xml = new Common.XmlDoc();


                if (xml.IsExistXml(xmlFilePath) && !string.IsNullOrEmpty(xmlFilePath))
                {

                    DataRow dr = dv[i].Row;
                    
                    string[] arr = xml.ReadXml(xmlFilePath, XHR.Common.StringDeal.ToInt(dr["id"] + "", 0));
                    string title = arr[0];
                    string Entitle = arr[1];
                    if (title != "0" && Entitle != "0")
                    {
                        dr["Title"] = Entitle;
                        sb.Append(lModelData.ReplaceLabeltoData(labelHTML.buqufendaxiaoxieReplace("{index}", i + ""), dr, param_arr) + "\n");
                        if (Top != 0 && (i + 1) == Top)
                        {
                            break;
                        }
                    }
                    else
                    {
                        sb.Append(lModelData.ReplaceLabeltoData(labelHTML.buqufendaxiaoxieReplace("{index}", i + ""), dr, param_arr) + "\n");
                        
                        if (Top != 0 && (i + 1) == Top)
                        {
                            break;
                        }
                    }
                    #region nav 数据替换
                    ReplaceNavLabelData(param_arr, sb, dr);
                    #endregion
                }
                else
                {
                    DataRow dr = dv[i].Row;
                    //dr["Title"] = "test" + i;

                    sb.Append(lModelData.ReplaceLabeltoData(labelHTML.buqufendaxiaoxieReplace("{index}", i + ""), dr, param_arr) + "\n");

                    #region nav 数据替换
                    ReplaceNavLabelData(param_arr, sb, dr);
                    #endregion
                    if (Top != 0 && (i + 1) == Top)
                    {
                        break;
                    }
                }
            }

            return sb.ToString().Replace("\r\n", "");
        }

        #region 替换 Nav数据 主要用于处理 栏目链接 和栏目定位问题

        /// <summary>
        /// 替换 Nav数据 主要用于处理 栏目链接 和栏目定位问题
        /// </summary>
        /// <param name="param_arr"></param>
        /// <param name="sb"></param>
        /// <param name="dr"></param>
        private static void ReplaceNavLabelData(Hashtable[] param_arr, StringBuilder sb, DataRow dr)
        {
            //if (ParamController.GetParam("curs", param_arr) != null)
            //{
                if (dr.Table.Columns["id"] != null)
                {
                    string temp = sb.ToString().Replace("{classid}", dr["id"].ToString());
                    Regex reg = new Regex(@"\{nav\.([^\{]+)\}", RegexOptions.IgnoreCase);
                    Match m = reg.Match(temp);
                    while (m.Success)
                    {
                        temp = temp.Replace(m.Result("$0"), dr[m.Result("$1")].ToString());

                        m = m.NextMatch();
                    }
                    if (ParamController.GetParam("model", param_arr) != null)
                    {
                        int parentid = new bMenu().GetParentId(StringDeal.ToInt(ParamController.GetParam(ParamController.GetParam("menuid", param_arr) != null ? "menuid" : "classid", param_arr)));
                        if (StringDeal.ToInt(dr["id"].ToString()) == parentid)
                        {
                            temp = Regex.Replace(temp, @"{cur\.class}", ParamController.GetParam("curs", param_arr), RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            temp = Regex.Replace(temp, @"{cur\.class}", ParamController.GetParam("cur", param_arr), RegexOptions.IgnoreCase);
                        }
                    }
                    sb.Remove(0, sb.Length);
                    sb.Append(temp);
                }
            //}
        } 
        #endregion

    }
}
