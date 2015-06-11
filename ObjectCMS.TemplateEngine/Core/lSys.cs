using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using ObjectCMS.Common;
using System.Text.RegularExpressions;
using ObjectCMS.BLL;

namespace ObjectCMS.TemplateEngine.Core
{
    public class lSys
    {
        public static string GetSysLabelByHTML(string labelHTML)
        {
            Regex regexSys = new Regex(@"\{(\w+?)\.(\d+? *)\.(\w+?)([ ][^\.\{]+)*\}", RegexOptions.IgnoreCase);
            Match mSys = regexSys.Match(labelHTML);
            while (mSys.Success)
            {
                var val = ModelManage.Instance.DataReader(mSys.Result("$2").ToInt(), mSys.Result("$1"), mSys.Result("$3"));

                if (val != null && val.ContainsKey(mSys.Result("$3")))
                {
                    labelHTML = labelHTML.IReplace(mSys.Result("$0"), val[mSys.Result("$3")].ToString());
                }
                else
                {
                    labelHTML = labelHTML.IReplace(mSys.Result("$0"), "");
                }
                mSys = mSys.NextMatch();
            }

            return labelHTML;
        }
    }
}
