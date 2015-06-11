using System;
using System.Collections.Generic;
using ObjectCMS.Common;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Diagnostics;
using ObjectCMS.Model;

namespace ObjectCMS.TemplateEngine.Core
{
    public class lInclude
    {
        public static string ReplaceInclude(string TemplateHTML, Hashtable[] param_arr)
        {
            Regex regexParam = new Regex("<!--\\#include\\ +?templateid=\"(\\d+?)\"\\ *?-->", RegexOptions.IgnoreCase);
            Match mParam = regexParam.Match(TemplateHTML);
            while (mParam.Success)
            {
                int templateId = mParam.Result("$1").ToInt();
                var t = TeTemplates.GetOne(templateId);
                TemplateHTML = TemplateHTML.IReplace(mParam.Result("$0"), t.TemplateHTML);
                mParam = mParam.NextMatch();
            }
            return TemplateHTML;
        }
    }
}
