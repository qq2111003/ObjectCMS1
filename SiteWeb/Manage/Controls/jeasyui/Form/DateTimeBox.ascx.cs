using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Common;

namespace UserControls.Controls.jeasyui.Form
{
    public partial class DateTimeBox : System.Web.UI.UserControl
    {
        #region 属性
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? Date
        {
            get
            {
                DateTime tmp;
                if (!string.IsNullOrEmpty(this.tb_DateTime.Text) && DateTime.TryParse(this.tb_DateTime.Text, out tmp))
                {
                    return tmp;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    if (DateModel == ShowType.OnlyDate)
                    {
                        this.tb_DateTime.Text = value.Value.ToString("yyyy-MM-dd");
                    }
                    else if (DateModel == ShowType.OnlyTime)
                    {
                        tb_DateTime.Width = 50;
                        this.tb_DateTime.Text = value.Value.ToString("HH:mm");
                    }
                    else
                    {
                        tb_DateTime.Width = 120;
                        this.tb_DateTime.Text = value.Value.ToString("yyyy-MM-dd HH:mm");
                    }
                }
                else
                {
                    this.tb_DateTime.Text = "";
                }
            }
        }

        private ShowType _DateModel = ShowType.OnlyDate;

        public ShowType DateModel
        {
            get { return _DateModel; }
            set { _DateModel = value; }
        }
        /// <summary>
        /// 允许选择的最小事件
        /// </summary>
        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get; set; }

        #endregion

        #region 方法
        protected string MinMaxDateJs()
        {
            string jsStr = "";
            if (MinDate != null)
            {
                jsStr += "var minDate=" + (MinDate.Value.ToUniversalTime().Ticks - DateTime.Parse("1970-01-01").Ticks) / 10000 + ";";
            }
            if (MaxDate != null)
            {
                jsStr += "var maxDate=" + (MaxDate.Value.ToUniversalTime().AddDays(1).Ticks - DateTime.Parse("1970-01-01").Ticks) / 10000 + ";";
            }
            return jsStr;
        }

        protected string MinMaxTimeJs()
        {
            string jsStr = "";
            if (MinDate != null)
            {
                jsStr += "min: '" + MinDate.Value.ToString("HH:mm") + "',";
            }
            if (MaxDate != null)
            {
                jsStr += "max: '" + MaxDate.Value.ToString("HH:mm") + "',";
            }
            return jsStr;
        }
        #endregion

        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.FindControl("UIHeader") == null)
            {
                System.Web.UI.Control header = Page.LoadControl(StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Helper/UIHeader.ascx"));
                header.ID = "UIHeader";
                this.Page.Header.Controls.Add(header);
            }


        }
        #endregion
    }

    public enum ShowType
    {
        OnlyDate,
        OnlyTime,
        DateTime
    }
}