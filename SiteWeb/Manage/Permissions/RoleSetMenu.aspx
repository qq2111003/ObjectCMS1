<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleSetMenu.aspx.cs" Inherits="SiteWeb.Manage.Permissions.RoleSetMenu" %>

<%@ Register Src="../Controls/jeasyui/TreeGrid.ascx" TagName="TreeGrid" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>菜单管理</title>
</head>
<body id="Body1" runat="server">
    <script type="text/javascript">
        var checkall = function (obj) {
            if ($(obj).attr('checked')) {
                $('input[type=checkbox]').attr('checked', 'checked');
            } else {
                $('input[type=checkbox]').removeAttr('checked');
            }
        }
        var checkleftall = function (obj, id) {
            if ($(obj).attr('checked')) {
                $('input[name=ctrl' + id + ']').attr('checked', 'checked');
            } else {
                $('input[name=ctrl' + id + ']').removeAttr('checked');
            }
        }
        var formData = {};
        function submitResult() {
            $.each($('input[type=checkbox][name=ctrlid]:checked'), function () {
                if (formData['ctrlid']) {
                    formData['ctrlid'] += ',' + $(this).val();
                } else {
                    formData['ctrlid'] = $(this).val();
                }
            });
            var checkedRows = $('input[name=flag]:checked');
            var checkedmenuids = [];
            for (var i = 0; i < checkedRows.length; i++) {
                checkedmenuids.push($(checkedRows[i]).val());
            }
            formData['checkedmenuids'] = checkedmenuids.join(',');
            formData['method'] = 'SubmitMethod';
            $.post('RoleSetMenu.aspx?id=<%=Request.QueryString["id"] %>', formData, function (str) {
                if (str == 1) {
                    parent.Message.show('角色权限设置成功', '提示');
                    parent.UIDialog.Close();
                }
            });
        }
    </script>
    <uc1:TreeGrid ID="TreeGrid1" runat="server" />
    <form style="display: none;">
    <input type="submit" id="dosubmit" onclick="submitResult();return false;" />
    </form>
</body>
</html>
