<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextBox.ascx.cs" Inherits="UserControls.Controls.jeasyui.Form.TextBox" %>
<asp:TextBox ID="tb_SingleLine" runat="server"></asp:TextBox>
<script type="text/javascript">
    $(function () {
        <%=WriteEventJs() %>
    });
</script>
