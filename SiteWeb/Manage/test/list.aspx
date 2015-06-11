<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="SiteWeb.Manage.test.list" %>

<%@ Register Src="../Controls/jeasyui/DataGrid.ascx" TagName="DataGrid" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:DataGrid ID="DataGrid1" runat="server" />
    </form>
</body>
</html>
