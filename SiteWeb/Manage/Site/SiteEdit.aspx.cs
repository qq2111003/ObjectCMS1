using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.System;
using UserControls.Controls.jeasyui;
using UserControls.Controls.jeasyui.Form;

namespace SiteWeb.Manage.Site
{
    public partial class SiteEdit : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);

            //设置form初始值 BB
            if (id == 0)
            {
                this.Visible = this.HasPermission(20, "add");
                FormBuilder1.Entity = new SysSite();
            }
            else
            {
                this.Visible = this.HasPermission(20, "edit");
                SysSite m = SysSite.GetOne(id);
                FormBuilder1.Entity = m;
            }

            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = new List<FormItem>()
            {
                new FormItem(){Title = "站 点 名", FieldName = "Name", FieldType =  FieldTypeInHTML.SingleLine,FieldModel= new ModelSingleLine(){
                    ReadOnly=id!=0,
                    ValidateTypes= new List<FieldValidate>(){
                        FieldValidate.required
                    },
                    OnBlur=(text)=>{
                        if(id==0){
                            var l = SysSite.GetALL("Name='" + text.Replace("'", "''") + "'", "id desc");
                            if (l != null && l.Count > 0)
                            {
                                return new EventResult() { Text = "$.messager.alert('错误','" + text.Replace("'", "\\'") + "已存在!','error');$('#FormBuilder1_SingleLine0_tb_SingleLine').val('');", RunJs = true };
                            }
                            else
                            {
                                return new EventResult() { Text = text.ToLower() };
                            }
                        }
                        return new EventResult() { Text = text.ToLower() };
                    }
                }},
                new FormItem(){Title = "站点标识", FieldName = "SiteMark", FieldType = FieldTypeInHTML.SingleLine},                
            };
            #endregion

            if (IsPostBack)
            {

                SysSite a = (SysSite)FormBuilder1.Entity;

                if (id == 0)
                {
                    ObjectCMS.BLL.SiteManage.Instance.CreateSite(a);
                }
                else
                {

                }
            }
        }
    }
}