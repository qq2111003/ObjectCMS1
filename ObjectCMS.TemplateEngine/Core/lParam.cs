using System;
using System.Collections.Generic;
using System.Text;
using ObjectCMS.Common;
using System.Text.RegularExpressions;
using System.Collections;
using ObjectCMS.TemplateEngine.Common;

namespace ObjectCMS.TemplateEngine.Core
{
    public class lParam
    {
        public static string ParamToHTML(string TemplateHTML, Hashtable[] param_arr)
        {
            Regex regexParam = new Regex(@"\{param\.([^\}]\w*)\}", RegexOptions.IgnoreCase);
            Match mParam = regexParam.Match(TemplateHTML);
            while (mParam.Success)
            {
                var val = ParamController.GetParam(mParam.Result("$1"), param_arr);
                TemplateHTML = TemplateHTML.IReplace(mParam.Result("$0"), val == null ? "" : val);
                mParam = mParam.NextMatch();
            }
            return TemplateHTML;
        }
    }
}
