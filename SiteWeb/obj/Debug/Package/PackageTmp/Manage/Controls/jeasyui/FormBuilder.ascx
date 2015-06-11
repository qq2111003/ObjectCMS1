<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormBuilder.ascx.cs"
    Inherits="UserControls.Controls.jeasyui.FormBuilder" %>
    <%@ Import Namespace="ObjectCMS.Common" %>
<link href="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Plugin/ValidationEngine/css/validationEngine.jquery.css")%>" rel="stylesheet"
    type="text/css" />
<style type="text/css">
    .form-table
    {
        width: 99%;
        background: #D5D5D5;
        border-spacing: 1px;
        color: #585858;
        font-size: 12px;
        font-family: Arial;
    }
    .form-table input:not(.combo-text), textarea, select
    {
        border: 1px solid #CCCCCC;
    }
    .form-table .td_bg
    {
        background: #FFFFFF;
    }
    .form-table .row-over
    {
        background: #F8FAFC;
    }
    .form-table .row-selected
    {
        background: #EBF3FD;
    }
</style>
<script src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Plugin/ValidationEngine/languages/jquery.validationEngine-zh_CN.js")%>"
    type="text/javascript"></script>
<script src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Plugin/ValidationEngine/jquery.validationEngine.js")%>" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $('.td_bg').hover(function () {
            $(this).addClass('row-over');
        }, function () {
            $(this).removeClass('row-over');
        }).click(function () {
            $(this).addClass("row-selected").siblings().removeClass("row-selected");
        });
        $("#<%=this.Page.Form.ClientID %>").validationEngine({ promptPosition: "bottomLeft", scroll: false });
    });
</script>
<asp:Table ID="tbl_Warpper" runat="server" class="form-table">
</asp:Table>
