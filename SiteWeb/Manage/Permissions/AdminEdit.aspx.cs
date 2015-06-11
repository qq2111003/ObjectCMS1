using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserControls.Controls.jeasyui;
using ObjectCMS.Model.System;
using UserControls.Controls.jeasyui.Form;
using ObjectCMS.Common;

namespace SiteWeb.Manage.Permissions
{
    public partial class AdminEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region  始终加载
            int id = Convert.ToInt32(Request["id"]);

            //设置form初始值 BB
            if (id == 0)
            {
                this.Visible = this.HasPermission(5, "add");
                FormBuilder1.Entity = new SysAdmin() { LastLoginTime = DateTime.Now, LoginTimes = 0, RPassword = "", Enable = true };
            }
            else
            {
                this.Visible = this.HasPermission(5, "edit");
                SysAdmin m = SysAdmin.GetOne(id);
                m.RPassword = "";
                m.Password = "*********";
                FormBuilder1.Entity = m;
            }

            //字段配置  类型\提示信息\验证方式等           
            FormBuilder1.Items = new List<FormItem>()
            {
                new FormItem(){Title = "用 户 名", FieldName = "Name", FieldType =  FieldTypeInHTML.SingleLine,FieldModel= new ModelSingleLine(){
                    ReadOnly=id!=0,
                    ValidateTypes= new List<FieldValidate>(){
                        FieldValidate.required
                    },
                    OnBlur=(text)=>{
                        if(id==0){
                            var l = SysAdmin.GetALL("Name='" + text.Replace("'", "''") + "'", "id desc");
                            if (l != null && l.Count > 0)
                            {
                                return new EventResult() { Text = "$.messager.alert('错误','" + text.Replace("'", "\\'") + "已被使用!','error');$('#FormBuilder1_SingleLine0_tb_SingleLine').val('');", RunJs = true };
                            }
                            else
                            {
                                return new EventResult() { Text = text.ToLower() };
                            }
                        }
                        return new EventResult() { Text = text.ToLower() };
                    }
                }},
                new FormItem(){Title = "密　　码", FieldName = "Password", FieldType = FieldTypeInHTML.SingleLine, FieldModel= new ModelSingleLine(){
                    TextModel = TextBoxMode.Password
                }},
                new FormItem(){Title = "确认密码", FieldName = "RPassword", FieldType = FieldTypeInHTML.SingleLine, FieldModel= new ModelSingleLine(){
                    TextModel = TextBoxMode.Password
                }}
                
            };
            #endregion

            if (IsPostBack)
            {

                SysAdmin a = (SysAdmin)FormBuilder1.Entity;

                if (a.Password != "*********" && a.Password != a.RPassword)
                {
                    Response.Write("<script>parent.Message.show('重复密码错误！','提示');</script>");
                }
                else
                {
                    if (id == 0)                //添加模式
                    {
                        a.Password = MD5.EncryptStringMD5(a.Password);
                        a.Insert();
                        Response.Write("<script>parent.Message.show('添加成功','提示');try{parent.DataGrid1Reload();}catch(e){}parent.UIDialog.Close();</script>");
                        Response.End();
                    }
                    else                        //修改模式
                    {
                        //需要额外给id赋值

                        if (a.Password == "*********")
                        {
                            a.Password = null;
                        }
                        else
                        {
                            a.Password = MD5.EncryptStringMD5(a.Password);
                        }
                        a.Id = id;
                        a.Update();
                        Response.Write("<script>parent.Message.show('修改成功','提示');try{parent.DataGrid1Reload();}catch(e){}parent.UIDialog.Close();</script>");
                        Response.End();
                    }
                }
            }
        }
    }    
}