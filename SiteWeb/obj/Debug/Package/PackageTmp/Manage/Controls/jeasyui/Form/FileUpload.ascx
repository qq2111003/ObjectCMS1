<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.ascx.cs"
    Inherits="UserControls.Controls.jeasyui.Form.FileUpload" %>
    <%@ Import Namespace="ObjectCMS.Common" %>
<asp:TextBox ID="tb_FilePath" runat="server" ReadOnly="true" Width="200"></asp:TextBox><input
    type="button" id="<%=this.ClientID %>_btn_upload" value="选择文件" />
<script type="text/javascript">
    KindEditor.ready(function (K) {
        var <%=this.ClientID %>editor = K.editor({
            uploadJson: '<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Other/upload_json.ashx")%>',
            fileManagerJson: '<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Other/file_manager_json.ashx")%>',
            allowFileManager: true
        });
        K('#<%=this.ClientID %>_btn_upload').click(function () {
            <%=this.ClientID %>editor.loadPlugin('insertfile', function () {
                <%=this.ClientID %>editor.plugin.fileDialog({
                    fileUrl : K('#<%=this.tb_FilePath.ClientID %>').val(),
                    clickFn: function (url, title) {
                        K('#<%=this.tb_FilePath.ClientID %>').val(url);
                        <%=this.ClientID %>editor.hideDialog();
                    }
                });
            });
        });
    });
</script>
