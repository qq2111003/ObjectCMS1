using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Web.Script.Serialization;
using System.Reflection;
using ObjectCMS.Common;

namespace UserControls.Controls.jeasyui.Form
{
    public partial class TreeNode : System.Web.UI.UserControl
    {
        #region 成员
        private JavaScriptSerializer _jssl = new JavaScriptSerializer();
        #endregion

        private string _IdField = "Id";

        public string IdField
        {
            get { return _IdField; }
            set { _IdField = value; }
        }

        private string _ParentField = "ParentId";

        public string ParentField
        {
            get { return _ParentField; }
            set { _ParentField = value; }
        }
        private string _TextField = "Title";

        public string TextField
        {
            get { return _TextField; }
            set { _TextField = value; }
        }

        public string SelectedValue { get; set; }

        public object DataSource { get; set; }

        /// <summary>
        /// 数据地址
        /// </summary>
        private string _DataUrl;
        public string DataUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_DataUrl))
                {
                    return _DataUrl;
                }
                else
                {
                    string Url = Request.Url.ToString();
                    if (Url.IndexOf('?') > 0)
                    {
                        return Url + "&uc=" + this.ClientID;
                    }
                    return Url + "?uc=" + this.ClientID;
                }
            }
            set { _DataUrl = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.FindControl("UIHeader") == null)
            {
                System.Web.UI.Control header = Page.LoadControl(StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Helper/UIHeader.ascx"));
                header.ID = "UIHeader";
                this.Page.Header.Controls.Add(header);
            }
            if (Request["uc"] != null && Request["uc"] == this.ClientID)
            {
                LoadData();
            }
        }

        public List<TreeModel> GetTreeData(int id)
        {
            List<TreeModel> TreeData = new List<TreeModel>();
            DataView dv = new DataView((DataTable)DataSource);
            dv.RowFilter = ParentField + "=" + id;
            for (int i = 0; i < dv.Count; i++)
            {
                TreeModel tm = new TreeModel();
                tm.id = int.Parse(dv[i][IdField].ToString());
                tm.text = dv[i][TextField].ToString();
                tm.children = GetTreeData(tm.id);
                TreeData.Add(tm);
            }            
            return TreeData;
        }

        /// <summary>
        /// list<实体>对象转datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable(IList entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                return null;
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //生成DataTable的structure
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        public void LoadData()
        {
            List<TreeModel> data;
            if (DataSource is IList)
            {
                DataSource = ListToDataTable((IList)DataSource);
                data = GetTreeData(0);
            }
            else if (DataSource is DataTable)
            {
                data = GetTreeData(0);
            }
            else
            {
                data = null;
            }
            List<TreeModel> RootNode = new List<TreeModel>() { new TreeModel() { id = 0, text = "根节点", children = data } };

            Response.Write(_jssl.Serialize(RootNode));
            Response.End();
        }
    }
    public class TreeModel
    {
        public int id { get; set; }
        public string text { get; set; }
        public List<TreeModel> children { get; set; }
    }
}