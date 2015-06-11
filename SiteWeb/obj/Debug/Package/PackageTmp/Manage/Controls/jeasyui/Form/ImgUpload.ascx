<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImgUpload.ascx.cs" Inherits="UserControls.Controls.jeasyui.Form.ImgUpload" %>
<%@ Import Namespace="ObjectCMS.Common" %>
<asp:TextBox ID="tb_ImgPath" runat="server" ReadOnly="true" Width="200"></asp:TextBox><input
    type="button" id="<%=this.ClientID %>_btn_upload" value="选择图片" />
<script type="text/javascript">
    KindEditor.ready(function (K) {
        var <%=this.ClientID %>editor = K.editor({
            uploadJson: '<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Other/upload_json.ashx")%>',
            fileManagerJson: '<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Controls/jeasyui/Other/file_manager_json.ashx")%>',
            allowFileManager: true
        });
        K('#<%=this.ClientID %>_btn_upload').click(function () {
            <%=this.ClientID %>editor.loadPlugin('image', function () {
                <%=this.ClientID %>editor.plugin.imageDialog({
                    imageUrl: K('#<%=this.tb_ImgPath.ClientID %>').val(),
                    clickFn: function (url, title, width, height, border, align) {
                        K('#<%=this.tb_ImgPath.ClientID %>').val(url);
                        <%=this.ClientID %>editor.hideDialog();
                    }
                });
            });
        });
    });
</script>
