<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NodeDataEdit.aspx.cs" Inherits="SiteWeb.Manage.Model.NodeDataEdit" %>

<%@ Register Src="../Controls/jeasyui/FormBuilder.ascx" TagName="FormBuilder" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="TreeGrid1_tb" class="datagrid-toolbar" style="">
            <a id="TreeGrid1_btn_add" href="javascript:;" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true"
                onclick="$('#form1').submit();" style="float: left;">保存</a><div class="datagrid-btn-separator">
                </div>
            <a id="TreeGrid1_btn_del" href="javascript:;" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true"
                onclick="$('input[type=reset]').click();" style="float: left;">重置</a></div>
        <uc1:FormBuilder ID="FormBuilder1" runat="server" />
        <input type="reset" style="display: none;" />
    </div>
    </form>
</body>
</html>
