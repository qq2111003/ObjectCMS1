<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProcessBar.ascx.cs"
    Inherits="SiteWeb.Manage.Controls.jeasyui.ProcessBar" %>
<div id="<%=this.ClientID %>_processbar" class="easyui-progressbar" style="width: <%=Width %>px;">
</div>
<script type="text/javascript">
    var go = function () {
        $.post('<%=DataUrl %>', { method: 'currPercent' }, function (data) {
            if (data < 100) {
                $('#<%=this.ClientID %>_processbar').progressbar('setValue', data);
                setTimeout("go()", 500);
            }
        });
    }
</script>
