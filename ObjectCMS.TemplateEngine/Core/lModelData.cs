using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using System.Diagnostics;
using ObjectCMS.TemplateEngine.Common;
using ObjectCMS.Common;
using ObjectCMS.Model.ModelConfig;

namespace ObjectCMS.TemplateEngine.Core
{
    public static class lModelData
    {
        private static string isfx, fxcode;

        public static DataTable dtfx;
        public static string ReplaceLabeltoData(string labelHTML, DataRow dr, Node node, Hashtable[] param_arr)
        {


            #region 替换数据库对应的数据

            #region \{([a-zA-Z_]+)([ ][^\{]+){0,1}\}
            Regex regex1 = new Regex(@"\{([a-zA-Z_]+)([ ][^\{]+){0,1}\}", RegexOptions.IgnoreCase);
            Match m1 = regex1.Match(labelHTML);
            while (m1.Success)
            {
                DataTable dt = dr.Table;
                string filed = m1.Result("$1");
                List<string[]> ls = ParamController.StrToParam(m1.Result("$2"));
                string value = "";

                try
                {
                    value = dr[filed].ToString();
                }
                catch (Exception)
                {
                    value = dr["id"].ToString();
                }

                for (int i = 0; i < ls.Count; i++)
                {
                    value = StrMethod(ls[i], value);
                }

                labelHTML = labelHTML.Replace(m1.Result("$0"), value);
                m1 = m1.NextMatch();
            }
            #endregion


            #region \{(content|class)\.(\w+(?:\.)*(?:\w+)*)([ ][^\{]+){0,1}\}
            Regex regex = new Regex(@"\{(content|node)\.(\w+(?:\.)*(?:\w+)*)([ ][^\{]+){0,1}\}", RegexOptions.IgnoreCase);
            Match m = regex.Match(labelHTML);
            while (m.Success)
            {
                string value="";
                if (m.Result("$1").ToLower() == "content")
                {
                    value = dr[m.Result("$2")].ToString();
                }
                else if (m.Result("$1").ToLower() == "node")
                {
                    var currNode = Node.GetOne(dr["NodeId"].ToInt());
                    var valObj = currNode.GetValue(m.Result("$2"));
                    value = valObj != null ? valObj.ToString() : "";
                }
                string filed = (m.Result("$1").ToLower() == "content" ? "" : m.Result("$1")) + m.Result("$2");
                List<string[]> ls = ParamController.StrToParam(m.Result("$3"));                

                //处理字段参数
                for (int i = 0; i < ls.Count; i++)
                {
                    value = StrMethod(ls[i], value);
                }

                labelHTML = labelHTML.Replace(m.Result("$0"), value);

                m = m.NextMatch();
            }
            #endregion

            #endregion

            return labelHTML;
        }


        public static string StrMethod(string[] param, string FieldHTML)
        {
            string key = param[0];
            string value = param[1];

            if (key == "sub" && new Regex(@"^[+-]?\d*$", RegexOptions.IgnoreCase).Match(value).Success)
            {
                return FieldHTML.CutString(value.ToInt());
            }
            else if (key == "format" && value.ToLower() == "linetobr")
            {
                return FieldHTML.Replace("\r\n", "<br/>");
            }
            else if (key == "format" && !new Regex(@"[^yMdhmsf -/\\\.:年月日时分秒]", RegexOptions.IgnoreCase).Match(value).Success)
            {
                return FieldHTML.ToFormatDate(value);
            }
            else if (key == "trim")
            {
                if (value.ToLower() == "left")
                {
                    return FieldHTML.TrimEnd();
                }
                else if (value.ToLower() == "right")
                {
                    return FieldHTML.TrimStart();
                }
                else
                {
                    return FieldHTML.Trim();
                }
            }
            else
            {
                return FieldHTML;
            }

        }


    }
}
