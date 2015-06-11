<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UIConfig.aspx.cs" Inherits="UserControls.Plugin.jeasyui.UIConfig" %>

<%@ Register Src="UIHeader.ascx" TagName="UIHeader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UI配置</title>
    <uc1:UIHeader ID="UIHeader1" runat="server" />
    <script type="text/javascript">
        var data = <%=LoadData() %>;
        $(function () {
            $('#tt').propertygrid("loadData", data);
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="tt" class="easyui-propertygrid" data-options="fitColumns:true,scrollbarSize:0">
        </table>
    </div>
    </form>
</body>
</html>
