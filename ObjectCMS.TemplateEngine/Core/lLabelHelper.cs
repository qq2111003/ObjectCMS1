using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;
using System.IO;
using ObjectCMS.Common;
using ObjectCMS.TemplateEngine.Common;
using ObjectCMS.Model;

namespace ObjectCMS.TemplateEngine.Core
{
    /// <summary>
    /// 标签处理类
    /// </summary>
    public static class lLabelHelper
    {
        /// <summary>
        /// 唯一入口: 在模板中找到标签
        /// </summary>
        /// <param name="TemplateHTML">模板HTML</param>
        /// <param name="param_arr">Hashtable[] param_arr</param>
        /// <param name="ListPageCount">总页数</param>
        /// <returns></returns>
        public static string ReplaceLabeltoData(string TemplateHTML, Hashtable[] param_arr, ref int ListPageCount)
        {
            TemplateHTML = lInclude.ReplaceInclude(TemplateHTML, param_arr);

            //替换页面中的数据标签
            if (TemplateHTML.Contains("{"))
            {
                TemplateHTML = GetDataHTMLbyLabelHTML("detail", TemplateHTML, param_arr, ref ListPageCount);
            }


            #region lif.DecodeIf
            
            TemplateHTML = lif.DecodeIf(lMath.MathOperation(lSys.GetSysLabelByHTML(lParam.ParamToHTML(TemplateHTML, param_arr))));

            #endregion



            Regex regex = new Regex(@"\[obj:([^ \[\]]+) *([^\[\]]+)*\]", RegexOptions.IgnoreCase);
            Match m = regex.Match(TemplateHTML);
            bool HasReplaced = false;
            while (m.Success)
            {
                Hashtable template_param = new Hashtable();
                string LabelNameInDB = m.Result("$1");  //数据库中的标签名
                // 提取参数 存入param
                template_param = ParamController.AddParam(m.Result("$2"), template_param);
                param_arr[1] = template_param;
                //HasReplaced = true;//是否需要递归查找
                m.Result("$0");
                //替换第一层标签为真实数据
                TemplateHTML = TemplateHTML.IReplace(m.Result("$0"), GetLabelbyName(LabelNameInDB, param_arr, out HasReplaced, ref ListPageCount));
                m = m.NextMatch();
            }

            //处理数据 if math
            //是否需要替换下一层
            if (HasReplaced)
                return ReplaceLabeltoData(TemplateHTML, param_arr, ref ListPageCount);
            else
                return TemplateHTML;
        }

        #region 由标签名从数据库中取得标签内容

        /// 由标签名从数据库中取得标签内容
        /// </summary>
        /// <param name="LabelName">标签名</param>
        /// <param name="param_arr">参数(从FrontChannel获得)</param>
        /// <param name="HasReplaced">是否需要继续替换</param>
        /// <param name="ListPageCount">返回总页数</param>
        /// <returns></returns>
        public static string GetLabelbyName(string LabelName, Hashtable[] param_arr, out bool HasReplaced, ref int ListPageCount)
        {
            #region 2012-11-23 给标签体加缓存
            string Label = "";

            var tag = TeTags.GetOne("TagName='" + LabelName + "'");
            if (tag != null)
            {

                Label = tag.TagHTML;
                HasReplaced = true;//标签有效 模板内容更新,有必要进行下一层查找
            }
            else
            {

                Label = LabelName;//标签无效 未记录 原文本返回
                HasReplaced = false;
            }




            #endregion

            Regex regexLabel = new Regex(@"(?s)<obj:(\w+)(.*?)>(.+?)</obj:\1+\>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match mLabel = regexLabel.Match(Label);
             
            while (mLabel.Success)
            {
                Hashtable label_param = new Hashtable();

                //取得标签中的参数 添加到
                label_param = ParamController.AddParam(mLabel.Result("$2"), label_param);
                param_arr[2] = label_param;                //
                Label = Label.IReplace(mLabel.Result("$0"), GetDataHTMLbyLabelHTML(mLabel.Result("$1"), mLabel.Result("$3"), param_arr, ref ListPageCount));
                mLabel = mLabel.NextMatch();
            }
            param_arr[2] = null;
            return Label;
        }
        #endregion

        #region 根据标签类型 把标签内容替换为数据库数据

        /// <summary>
        /// 根据标签类型 把标签内容替换为数据库数据
        /// </summary>
        /// <param name="LabelType"></param>
        /// <param name="LabelHTML"></param>
        /// <param name="param"></param>
        /// <param name="ListPageCount"></param>
        /// <returns></returns>
        public static string GetDataHTMLbyLabelHTML(string LabelType, string LabelHTML, Hashtable[] param_arr, ref int ListPageCount)
        {
            if (ListPageCount == -1)
            {
                ListPageCount = 1;
            }

            switch (LabelType)
            {
                case "for":
                    return lFor.ReplaceLabeltoData(LabelHTML, param_arr);
                case "page":
                    return lPage.ReplaceLabeltoData(LabelHTML, param_arr, ref ListPageCount);
                case "detail":
                    return lDetail.ReplaceLabeltoData(LabelHTML, param_arr);
                case "prevnext":
                    return lPrevNext.ReplaceLabeltoData(LabelHTML, param_arr);
                default:
                    return LabelHTML;
            }
        }
        #endregion




    }
}
