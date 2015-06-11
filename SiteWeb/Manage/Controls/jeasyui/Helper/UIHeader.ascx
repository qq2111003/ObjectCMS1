<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UIHeader.ascx.cs" Inherits="UserControls.Controls.jeasyui.UIHeader" %>
<%@ Import Namespace="ObjectCMS.Common" %>
<link href="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/jquery-easyui/themes/gray/easyui.css")%>"
    rel="stylesheet" type="text/css" />
<link href="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/jquery-easyui/themes/icon.css")%>"
    rel="stylesheet" type="text/css" />
<script src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/js/jquery-1.8.0.min.js")%>"
    type="text/javascript"></script>
<script src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/jquery-easyui/jquery.easyui.min.js")%>"
    type="text/javascript"></script>
<script src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/plugin/jquery-easyui/locale/easyui-lang-zh_CN.js")%>"
    type="text/javascript"></script>
<script src="<%=StringMethod.GetRelativePath(Request.Path, "/Manage/js/common.js")%>"
    type="text/javascript"></script>
