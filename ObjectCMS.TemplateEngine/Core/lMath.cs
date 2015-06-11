using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using ObjectCMS.Common;

namespace ObjectCMS.TemplateEngine.Core
{
    public class lMath
    {
        public static string MathOperation(string TemplateHTML)
        {
            Regex regex = new Regex(@"\[(\d+)([\+\-\*/%])(\d+)\]", RegexOptions.IgnoreCase);
            Match m = regex.Match(TemplateHTML);
            bool IsNeedFind = m.Success;
            while (m.Success)
            {
                TemplateHTML = TemplateHTML.IReplace(m.Result("$0"), StrToOpration(m.Result("$1"), m.Result("$2"), m.Result("$3"), out IsNeedFind));

                m = m.NextMatch();
            }
            if (IsNeedFind)
                return MathOperation(TemplateHTML);
            else
                return TemplateHTML;
        }
        public static string StrToOpration(string Num1, string Opration, string Num2, out bool IsNeedFind)
        {
            IsNeedFind = true;
            switch (Opration)
            {
                case "+":
                    return (double.Parse(Num1) + double.Parse(Num2)).ToString();
                case "-":
                    return (double.Parse(Num1) - double.Parse(Num2)).ToString();
                case "*":
                    return (double.Parse(Num1) * double.Parse(Num2)).ToString();
                case "/":
                    return (double.Parse(Num1) / double.Parse(Num2)).ToString();
                case "%":
                    return (double.Parse(Num1) % double.Parse(Num2)).ToString();
                default:
                    IsNeedFind = false;
                    return "[" + Num1 + Opration + Num2 + "]";

            }
        }
    }
}
