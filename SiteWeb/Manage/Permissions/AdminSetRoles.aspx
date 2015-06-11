<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminSetRoles.aspx.cs"
    Inherits="SiteWeb.Manage.Permissions.AdminSetRoles" %>

<%@ Register Src="../Controls/jeasyui/DataGrid.ascx" TagName="DataGrid" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>分配角色</title>
</head>
<body id="Body1" runat="server">
    <uc1:DataGrid ID="DataGrid1" runat="server" />
    <form style="display: none;">
    <input type="submit" id="dosubmit" onclick="$('#DataGrid1_btn_add').click();return false;" />
    </form>
</body>
</html>
