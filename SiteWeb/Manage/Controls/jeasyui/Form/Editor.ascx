<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="UserControls.Controls.jeasyui.Form.Editor" %>
<%@ Import Namespace="ObjectCMS.Common" %>
<asp:TextBox ID="tb_Editor" runat="server" TextMode="MultiLine" Width="98%" Height="400"></asp:TextBox>
<script type="text/javascript">
    $(function () {
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('#<%=this.tb_Editor.ClientID %>', {
                uploadJson: '<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Other/upload_json.ashx")%>',
                fileManagerJson: '<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Other/file_manager_json.ashx")%>',
                allowFileManager: true,
                afterChange: function () {
                    K.sync('#<%=this.tb_Editor.ClientID %>');
                }
            });
        });
    });
</script>
