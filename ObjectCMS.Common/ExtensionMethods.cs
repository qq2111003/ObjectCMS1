using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ObjectCMS.Common
{
    public static class ExtensionMethods
    {
        public static string IReplace(this String source, string oldStr, string newStr)
        {
            return Regex.Replace(source, Regex.Escape(oldStr), newStr, RegexOptions.IgnoreCase);
        }
        public static int ToInt(this Object val)
        {
            return TypeConverter.ObjectToInt(val);
        }
        public static int ToInt(this Object val, int defVal)
        {
            return TypeConverter.ObjectToInt(val, defVal);
        }
        public static int ToInt(this String val)
        {
            return TypeConverter.StrToInt(val);
        }
        public static int ToInt(this String val, int defVal)
        {
            return TypeConverter.StrToInt(val, defVal);
        }
        public static string ToFormatDate(this Object val, string format)
        {
            if (val != null)
            {
                DateTime dateTime;
                if (DateTime.TryParse(val.ToString(), out dateTime))
                    return dateTime.ToString(format, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            return "";
        }
        public static string CutString(this String val, int len)
        {
            return Utils.CutString(val, len);
        }
    }
}
