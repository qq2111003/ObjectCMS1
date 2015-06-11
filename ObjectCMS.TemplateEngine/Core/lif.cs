using System;
using System.Collections.Generic;
using ObjectCMS.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace ObjectCMS.TemplateEngine.Core
{
    public static class lif
    {
        public static string DecodeIf(string TemplateHTML)
        {
            Regex regex = new Regex(@"\[([^\[\]]+)\?([^\[\]\:]*)\:([^\[\]]*)\]", RegexOptions.IgnoreCase);
            Match m = regex.Match(TemplateHTML);
            bool IsNeedFind = m.Success;
            while (m.Success)
            {
                if (SplitConidition(m.Result("$1")))
                {
                    TemplateHTML = TemplateHTML.IReplace(m.Result("$0"), m.Result("$2"));
                }
                else
                {
                    TemplateHTML = TemplateHTML.IReplace(m.Result("$0"), m.Result("$3"));
                }
                m = m.NextMatch();
            }
            if (IsNeedFind)
                return DecodeIf(TemplateHTML);
            else
                return TemplateHTML;
        }
        public static bool SplitConidition(string str)
        {
            if (str.IndexOf("&&") >= 0)
            {
                string[] con_arr = str.Split(new string[] { "&&" }, StringSplitOptions.None);
                bool returnvalue = true;
                for (int i = 0; i < con_arr.Length; i++)
                {
                    if (returnvalue)
                    {
                        returnvalue = StrToBool(con_arr[i]);
                    }
                }
                return returnvalue;
            }
            else if (str.IndexOf("||") >= 0)
            {
                string[] con_arr = str.Split(new string[] { "||" }, StringSplitOptions.None);

                for (int i = 0; i < con_arr.Length; i++)
                {
                    if (StrToBool(con_arr[i]))
                    {
                        return true;
                    }
                }
                return false;

            }
            else
            {
                return StrToBool(str);
            }

        }
        public static bool StrToBool(string str)
        {
            if (str.IndexOf("==") > 0)
            {
                string[] arr_str = str.Split(new string[] { "==" }, StringSplitOptions.None);
                return arr_str[0] == arr_str[1];
            }
            else if (str.IndexOf("!=") > 0)
            {
                string[] arr_str = str.Split(new string[] { "!=" }, StringSplitOptions.None);
                return double.Parse(arr_str[0]) != double.Parse(arr_str[1]);
            }
            else if (str.IndexOf(">=") > 0)
            {
                string[] arr_str = str.Split(new string[] { ">=" }, StringSplitOptions.None);
                return double.Parse(arr_str[0]) >= double.Parse(arr_str[1]);
            }
            else if (str.IndexOf("<=") > 0)
            {
                string[] arr_str = str.Split(new string[] { "<=" }, StringSplitOptions.None);
                return double.Parse(arr_str[0]) <= double.Parse(arr_str[1]);
            }
            else if (str.IndexOf(">") > 0)
            {
                string[] arr_str = str.Split(new string[] { ">" }, StringSplitOptions.None);
                return double.Parse(arr_str[0]) > double.Parse(arr_str[1]);
            }
            else if (str.IndexOf("<") > 0)
            {
                string[] arr_str = str.Split(new string[] { "<" }, StringSplitOptions.None);
                return double.Parse(arr_str[0]) < double.Parse(arr_str[1]);
            }
            else
            {
                return false;
            }
        }
    }
}
