using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data;
using System.Web;
using System.IO;
using ObjectCMS.Common;
using ObjectCMS.TemplateEngine.Common;
using ObjectCMS.Model;

namespace ObjectCMS.TemplateEngine.Core
{
    public class TemplateHelper
    {
        public bool BuilderHTML(int templateId, string toFilePath, Hashtable[] param_arr, ref int ListPageCount)
        {
            #region
            string all = "";
            toFilePath = lLabelHelper.ReplaceLabeltoData(toFilePath, param_arr, ref ListPageCount);
            string param = "";


            var template = TeTemplates.GetOne(templateId);
            all = lLabelHelper.ReplaceLabeltoData(template.TemplateHTML, param_arr, ref ListPageCount);
            toFilePath = "/Publish/" + Cookie.GetCookie("curdb") + toFilePath;

            CreateFolder(toFilePath);

            StreamWriter WrirteFile = new StreamWriter(HttpContext.Current.Server.MapPath(toFilePath), false, Encoding.UTF8);//创建文件



            WrirteFile.Write(all);
            WrirteFile.Flush();
            WrirteFile.Close();
            return true;
            #endregion
        }
        public void CreateFolder(string folder)
        {
            string[] folders = folder.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            string f = "";
            for (int i = 0; i < folders.Length - 1; i++)
            {
                f += "/" + folders[i];
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(f)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(f));
                }
            }
        }

    }
}
