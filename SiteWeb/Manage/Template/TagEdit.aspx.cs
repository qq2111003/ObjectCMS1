using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model;
using UserControls.Controls.jeasyui;

namespace SiteWeb.Manage.Template
{
    public partial class TagEdit : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);

            //设置form初始值 BB
            if (id == 0)
            {
                this.Visible = this.HasPermission(15, "add");
                FormBuilder1.Entity = new TeTags();
            }
            else
            {
                this.Visible = this.HasPermission(15, "edit");
                TeTags m = TeTags.GetOne(id);
                FormBuilder1.Entity = m;
            }

            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = new List<FormItem>()
            {
                new FormItem(){Title = "标签名称", FieldName = "TagName", FieldType =  FieldTypeInHTML.SingleLine,FieldModel= new ModelSingleLine(){
                    ValidateTypes= new List<FieldValidate>(){
                        FieldValidate.required
                    }
                }},  
                new FormItem(){Title = "标签内容", FieldName = "TagHTML", FieldType =  FieldTypeInHTML.CodeEditor, FieldModel = new ModelCodeEditor(){                    
                    Width=Unit.Percentage(99),
                    Height = 240
                }}
            };
            #endregion

            if (IsPostBack)
            {

                TeTags a = (TeTags)FormBuilder1.Entity;


                if (id == 0)                //添加模式
                {
                    a.Insert();
                    Response.Write("<script>parent.Message.show('添加成功','提示');try{parent.DataGrid1Reload();}catch(e){}parent.UIDialog.Close();</script>");
                    Response.End();
                }
                else                        //修改模式
                {
                    //需要额外给id赋值
                    a.Id = id;
                    a.Update();
                    Response.Write("<script>parent.Message.show('修改成功','提示');try{parent.DataGrid1Reload();}catch(e){}parent.UIDialog.Close();</script>");
                    Response.End();
                }

            }
        }
    }
}