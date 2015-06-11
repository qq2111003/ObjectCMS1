<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuManage.aspx.cs" Inherits="SiteWeb.Manage.Menu.MenuManage" %>

<%@ Register Src="../Controls/jeasyui/TreeGrid.ascx" TagName="TreeGrid" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>菜单管理</title>
</head>
<body id="Body1" runat="server">
    <uc1:TreeGrid ID="TreeGrid1" runat="server" />
</body>
</html>
