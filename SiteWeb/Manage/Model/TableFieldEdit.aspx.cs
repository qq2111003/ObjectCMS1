using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjectCMS.Model.ModelConfig;
using UserControls.Controls.jeasyui;
using ObjectCMS.BLL;

namespace SiteWeb.Manage.Model
{
    public partial class TableFieldEdit : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(11, "fieldset");
            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);
            int userModelId = Convert.ToInt32(Request["UserModelId"]);

            //设置form初始值 
            if (id == 0)
            {
                FormBuilder1.Entity = new UserModelField() { UserModelId = userModelId, Sort = 0 };
            }
            else
            {
                FormBuilder1.Entity = UserModelField.GetOne(id);
            }

            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = new List<FormItem>()
            {
                new FormItem(){Title = "名　　称", FieldName = "Title", FieldType =  FieldTypeInHTML.SingleLine, 
                    FieldModel= new ModelSingleLine(){ValidateTypes=new List<FieldValidate>(){FieldValidate.required}}},
                new FormItem(){Title = "列　　名", FieldName = "FieldName", FieldType = FieldTypeInHTML.SingleLine,
                    FieldModel= new ModelSingleLine(){ValidateTypes=new List<FieldValidate>(){FieldValidate.required}}},                    
                new FormItem(){Title = "字段类型", FieldName = "FieldTypeId", FieldType = FieldTypeInHTML.Select,
                   FieldModel = new ModelSelect(){Items=(from u in UserModelFieldType.GetALL("1=1","Id") select u.Id+"|"+u.TypeName).ToArray()}}
            };
            #endregion

            if (IsPostBack)
            {
                if (id == 0)                //添加模式
                {
                    var entity = (UserModelField)FormBuilder1.Entity;
                    ModelManage.Instance.AddColumn(entity);
                    Response.Write("<script>parent.Message.show('添加成功','提示');parent.DataGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();
                }
                else                        //修改模式
                {
                    //需要额外给id赋值
                    var entity = (UserModelField)FormBuilder1.Entity;
                    entity.Id = id;
                    entity.Update();
                    Response.Write("<script>parent.Message.show('修改成功','提示');parent.DataGrid1Reload();parent.UIDialog.Close();</script>");
                    Response.End();
                }
            }
        }
    }
}