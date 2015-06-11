using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.ModelConfig;
using UserControls.Controls.jeasyui;

namespace SiteWeb.Manage.Model
{
    public partial class FieldTypeEdit : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);

            //设置form初始值 BB
            if (id == 0)
            {
                this.Visible = this.HasPermission(12, "add");
                FormBuilder1.Entity = new UserModelFieldType();
            }
            else
            {
                this.Visible = this.HasPermission(12, "edit");
                UserModelFieldType m = UserModelFieldType.GetOne(id);
                FormBuilder1.Entity = m;
            }

            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = new List<FormItem>()
            {
                new FormItem(){Title = "类型标识", FieldName = "TypeName", FieldType =  FieldTypeInHTML.SingleLine,FieldModel= new ModelSingleLine(){
                    ValidateTypes= new List<FieldValidate>(){
                        FieldValidate.required
                    }
                }},  
                 new FormItem(){Title = "类型标识", FieldName = "DBType", FieldType =  FieldTypeInHTML.Select,FieldModel= new ModelSelect(){
                    Items = new[]{"varchar(50)","varchar(100)","varchar(200)","varchar(500)","varchar(max)","int","bit","datetime","text","ntext"}
                }}, 
                new FormItem(){Title = "图标", FieldName = "Ico", FieldType =  FieldTypeInHTML.ImgUpload}
            };
            #endregion

            if (IsPostBack)
            {

                UserModelFieldType a = (UserModelFieldType)FormBuilder1.Entity;


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