<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KEHelper.ascx.cs" Inherits="UserControls.Controls.jeasyui.Helper.KEHelper" %>
<%@ Import Namespace="ObjectCMS.Common" %>
<link href="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/Kindeditor/themes/default/default.css")%>"
    rel="stylesheet" type="text/css" />
<script charset="utf-8" src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Plugin/Kindeditor/kindeditor-min.js")%>"></script>
<script charset="utf-8" src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/Plugin/Kindeditor/lang/zh_CN.js")%>"></script>
