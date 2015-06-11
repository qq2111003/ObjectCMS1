<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataList.aspx.cs" Inherits="SiteWeb.Manage.Model.DataList" %>

<%@ Register Src="../Controls/jeasyui/DataGrid.ascx" TagName="DataGrid" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<script type="text/javascript">
    function ChangeSort(obj) {
        AjaxController.Post("DataEdit.aspx", { method: 'ChangeSort', nodeid: '<%=nodeId %>', id: $(obj).attr("dataid"), value: $(obj).val() }, function () {

        });
    }
</script>
<body id="Body1" runat="server">
    <uc1:DataGrid ID="DataGrid1" runat="server" />
</body>
</html>
