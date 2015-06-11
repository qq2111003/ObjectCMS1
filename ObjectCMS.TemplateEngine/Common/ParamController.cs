using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace ObjectCMS.TemplateEngine.Common
{
    public class ParamController
    {
        /// <summary>
        /// 无序模式
        /// </summary>
        /// <param name="param_str"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Hashtable AddParam(string param_str, Hashtable param)
        {
            if (param_str.Length > 0)
            {
                Regex regexParam = new Regex("(\\w+?)=([\"']*)(.+?)(\\2)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                //Regex regexParam = new Regex("(\\w+?)=([\"']*)([\\w =']+)(\\2)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Match mParam = regexParam.Match(param_str);

                while (mParam.Success)
                {
                    if (param.Contains(mParam.Result("$1")))
                    {
                        param[mParam.Result("$1")] = mParam.Result("$3");
                    }
                    else
                    {
                        param.Add(mParam.Result("$1"), mParam.Result("$3"));
                    }
                    mParam = mParam.NextMatch();
                }
            }
            return param;
        }
        /// <summary>
        /// 有序模式
        /// </summary>
        /// <param name="param_str"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<string[]> StrToParam(string param_str)
        {
            Regex regexParam = new Regex("(\\w+?)=([\"']*)(.+?)(\\2)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match mParam = regexParam.Match(param_str);
            List<string[]> arr = new List<string[]>();
            string[] each_param;
            while (mParam.Success)
            {
                each_param = new string[2];
                each_param[0] = mParam.Result("$1");
                each_param[1] = mParam.Result("$3");
                arr.Add(each_param);
                mParam = mParam.NextMatch();
            }
            return arr;
        }

        public static string GetParam(string key, Hashtable[] param_arr)
        {
            if (param_arr != null)
            {
                if (param_arr[2] != null && param_arr[2][key] != null)
                {
                    return param_arr[2][key].ToString();
                }
                else if (param_arr[1] != null && param_arr[1][key] != null)
                {
                    return param_arr[1][key].ToString();
                }
                else if (param_arr[0] != null && param_arr[0][key] != null)
                {
                    return param_arr[0][key].ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
