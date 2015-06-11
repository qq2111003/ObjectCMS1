<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleManage.aspx.cs" Inherits="SiteWeb.Manage.Permissions.RoleManage" %>

<%@ Register Src="../Controls/jeasyui/DataGrid.ascx" TagName="DataGrid" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色管理</title>
</head>
<body runat="server">
    <uc1:DataGrid ID="DataGrid1" runat="server" />
</body>
</html>
