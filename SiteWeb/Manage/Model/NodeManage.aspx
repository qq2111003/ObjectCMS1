<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NodeManage.aspx.cs" Inherits="SiteWeb.Manage.Model.NodeManage" %>

<%@ Register Src="../Controls/jeasyui/TreeGrid.ascx" TagName="TreeGrid" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body id="Body1" runat="server">
    <script>
        function openBuildAllWindow() {
            var selectedRows = $('#TreeGrid1_treegrid').treegrid('getSelections');
            var ids = '0';
            $.each(selectedRows, function (i, obj) { ids += "," + obj.Id; });
            UIDialog.Init('dobuild', '生成进度', 450, 120, true).Open('BuildHtml.aspx?nodeids=' + ids);
        }
    </script>
    <uc1:TreeGrid ID="TreeGrid1" runat="server" />
</body>
</html>
