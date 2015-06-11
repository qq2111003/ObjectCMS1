<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CodeEditor.ascx.cs"
    Inherits="UserControls.Controls.jeasyui.Form.CodeEditor" %>
<%@ Import Namespace="ObjectCMS.Common" %>
<link rel="stylesheet" href="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/CodeMirror/lib/codemirror.css")%>" />
<link rel="stylesheet" href="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/CodeMirror/theme/rubyblue.css")%>" />
<script type="text/javascript" src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/CodeMirror/lib/codemirror.js")%>"></script>
<script type="text/javascript" src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/CodeMirror/mode/xml/xml.js")%>"></script>
<script type="text/javascript" src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/CodeMirror/mode/javascript/javascript.js")%>"></script>
<script type="text/javascript" src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/CodeMirror/mode/css/css.js")%>"></script>
<script type="text/javascript" src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/CodeMirror/mode/htmlmixed/htmlmixed.js")%>"></script>
<style>
    .CodeMirror
    {
        border-top: 1px solid black;
        border-bottom: 1px solid black;
    }
</style>
<asp:TextBox ID="tb_CodeEditor" runat="server" TextMode="MultiLine" Width="98%" Height="400"></asp:TextBox>
<script type="text/javascript">
    var editor = CodeMirror.fromTextArea(document.getElementById("<%=tb_CodeEditor.ClientID %>"), { mode: "text/html", tabMode: "indent", autoMatchParens: true, lineWrapping: true, lineNumbers: true, theme: 'rubyblue' });
</script>
