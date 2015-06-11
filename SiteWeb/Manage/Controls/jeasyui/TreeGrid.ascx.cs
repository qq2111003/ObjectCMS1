using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Text;

namespace UserControls.Controls.jeasyui
{
    /// <summary>
    /// 继承自DataGrid
    /// </summary>
    public partial class TreeGrid : DataGrid
    {
        #region 属性

        private string _TreeField = "Id";

        public string TreeField
        {
            get { return _TreeField; }
            set { _TreeField = value; }
        }
        private string _ParentField = "ParentId";

        public string ParentField
        {
            get { return _ParentField; }
            set { _ParentField = value; }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 格式化DataTable数据
        /// </summary>
        public override List<Dictionary<string, object>> FormatAll(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
            //格式化后的数据用List<Dictionary<string, string>>存储
            List<Dictionary<string, object>> FormatedData = new List<Dictionary<string, object>>();
            Dictionary<string, object> dic;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dic = new Dictionary<string, object>();
                dic.Add("_parentId", dt.Rows[i][ParentField]); /////////////////
                for (int j = 0; j < Columns.Count; j++)
                {
                    if (dt.Columns.Contains(Columns[j].FieldName))
                    {
                        //如果此列有委托函数 则进行格式化
                        if (Columns[j].FuncFormater != null)
                        {
                            dic.Add(Columns[j].FieldName, Columns[j].FuncFormater(dt.Rows[i], dt.Rows[i][Columns[j].FieldName], i));
                        }
                        else
                        {
                            dic.Add(Columns[j].FieldName, dt.Rows[i][Columns[j].FieldName]);
                        }
                    }
                }
                FormatedData.Add(dic);
            }
            return FormatedData;
        }
        /// <summary>
        /// 格式化List<Entity>数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public override List<Dictionary<string, object>> FormatAll(IList list)
        {
            if (list == null || list.Count < 1)
            {
                return null;
            }
            //取出第一个实体的所有Propertie
            Type entityType = list[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //生成DataTable的structure

            List<Dictionary<string, object>> FormatedData = new List<Dictionary<string, object>>();
            Dictionary<string, object> dic;

            for (int i = 0; i < list.Count; i++)
            {
                dic = new Dictionary<string, object>();
                PropertyInfo parentFieldPi = list[i].GetType().GetProperty(ParentField);
                object parentFieldCell = parentFieldPi == null ? null : parentFieldPi.GetValue(list[i], null);
                dic.Add("_parentId", parentFieldCell);
                for (int j = 0; j < Columns.Count; j++)
                {
                    PropertyInfo pi = list[i].GetType().GetProperty(Columns[j].FieldName);
                    object cell = pi == null ? null : pi.GetValue(list[i], null);

                    //如果此列有委托函数 则进行格式化
                    if (Columns[j].FuncFormater != null)
                    {
                        dic.Add(Columns[j].FieldName, Columns[j].FuncFormater(list[i], cell, i));
                    }
                    else
                    {
                        dic.Add(Columns[j].FieldName, cell);
                    }

                }
                FormatedData.Add(dic);
            }
            return FormatedData;
        }                
        #endregion
    }
}