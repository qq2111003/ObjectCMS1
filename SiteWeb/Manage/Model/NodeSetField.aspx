<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NodeSetField.aspx.cs" Inherits="SiteWeb.Manage.Model.NodeSetField" %>

<%@ Register Src="../Controls/jeasyui/DataGrid.ascx" TagName="DataGrid" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml" class="panel-fit">
<head id="Head1">
    <title></title>
    <link href="../plugin/jquery-easyui/themes/gray/easyui.css" rel="stylesheet" type="text/css">
    <link href="../plugin/jquery-easyui/themes/icon.css" rel="stylesheet" type="text/css">
    <script src="../js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <style type="text/css">
        .datagrid-cell-c1-Id
        {
            width: 20px;
        }
        .datagrid-cell-c1-FieldName
        {
            width: 76px;
        }
        .datagrid-cell-c1-FieldType
        {
            width: 76px;
        }
        .datagrid-cell-c1-Title
        {
            width: 76px;
        }
        
        .datagrid-cell-c1-flag
        {
            width: 24px;
        }
        .datagrid-cell-c1-ShowInList
        {
            width: 48px;
        }
        .datagrid-cell-c1-ReWriteTitle
        {
            width: 76px;
        }
        .datagrid-cell-c1-ListWidth
        {
            width: 48px;
        }
        .datagrid-cell-c1-Sort
        {
            width: 53px;
        }
        .datagrid-cell-c1-DefVal
        {
            width: 53px;
        }
        .datagrid-cell-c1-OtherAttr
        {
            width: 76px;
        }
        .datagrid-cell-c1-Validator
        {
            width: 53px;
        }
        .datagrid-cell-c1-Tip
        {
            width: 53px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("input[name=flag]").change(function () {
                if ($(this).attr("checked")) {
                    $(this).parent().parent().parent().find('input:not([name=flag],[name=ListWidth_' + $(this).val() + '])').removeAttr("disabled");
                } else {
                    $(this).parent().parent().parent().find('input:not([name=flag],[name=ListWidth_' + $(this).val() + '])').attr("disabled", "disabled").removeAttr("checked");
                }
                $.each($("input[name=ShowInList]"), function (a) {
                    if ($(this).attr("checked")) {
                        $('input[name=ListWidth_' + $(this).val() + ']').removeAttr("disabled");
                    } else {
                        $('input[name=ListWidth_' + $(this).val() + ']').attr("disabled", "disabled")
                    }
                });
            });
            $("input[name=ShowInList]").change(function () {
                if ($(this).attr("checked")) {
                    $('input[name=ListWidth_' + $(this).val() + ']').removeAttr("disabled");
                } else {
                    $('input[name=ListWidth_' + $(this).val() + ']').attr("disabled", "disabled")
                }
            });

            $.each($("input[name=flag]"), function (a) {
                if ($(this).attr("checked")) {
                    $(this).parent().parent().parent().find('input:not([name=flag],[name=ListWidth_' + $(this).val() + '])').removeAttr("disabled");
                } else {
                    $(this).parent().parent().parent().find('input:not([name=flag],[name=ListWidth_' + $(this).val() + '])').attr("disabled", "disabled").removeAttr("checked");
                }
                $.each($("input[name=ShowInList]"), function (a) {
                    if ($(this).attr("checked")) {
                        $('input[name=ListWidth_' + $(this).val() + ']').removeAttr("disabled");
                    } else {
                        $('input[name=ListWidth_' + $(this).val() + ']').attr("disabled", "disabled")
                    }
                });
            });
            $.each($("input[name=ShowInList]"), function (a) {
                if ($(this).attr("checked")) {
                    $('input[name=ListWidth_' + $(this).val() + ']').removeAttr("disabled");
                } else {
                    $('input[name=ListWidth_' + $(this).val() + ']').attr("disabled", "disabled")
                }
            });



        });
    </script>
</head>
<body id="Body1" class="panel-noscroll">
    <form id="from1" runat="server">
    <div class="panel datagrid" style="width: 870px;">
        <div class="datagrid-wrap panel-body panel-body-noheader" title="" style="width: 865px;
            height: 414px;">
            <div class="datagrid-view" style="width: 870px; height: 414px;">
                <div class="datagrid-view2" style="width: 870px;">
                    <div class="datagrid-header" style="width: 868px; height: 24px;">
                        <div class="datagrid-header-inner" style="display: block;">
                            <table class="datagrid-htable" border="0" cellspacing="0" cellpadding="0" style="height: 25px;">
                                <tbody>
                                    <tr class="datagrid-header-row">
                                        <td field="Id">
                                            <div class="datagrid-cell datagrid-sort-ASC" style="width: 20px; text-align: center;">
                                                <span>#</span></div>
                                        </td>
                                        <td field="FieldName">
                                            <div class="datagrid-cell" style="width: 76px; text-align: left;">
                                                <span>名称</span></div>
                                        </td>
                                        <td field="FieldType">
                                            <div class="datagrid-cell" style="width: 76px; text-align: left;">
                                                <span>类型</span></div>
                                        </td>
                                        <td field="Title">
                                            <div class="datagrid-cell" style="width: 76px; text-align: left;">
                                                <span>标题</span></div>
                                        </td>
                                        <td field="flag">
                                            <div class="datagrid-cell" style="width: 24px; text-align: left;">
                                                <span>开启</span></div>
                                        </td>
                                        <td field="ReWriteTitle">
                                            <div class="datagrid-cell" style="width: 76px; text-align: center;">
                                                <span>重写标题</span><span class="datagrid-sort-icon">&nbsp;</span></div>
                                        </td>
                                        <td field="Sort">
                                            <div class="datagrid-cell" style="width: 53px; text-align: center;">
                                                <span>排序</span><span class="datagrid-sort-icon">&nbsp;</span></div>
                                        </td>
                                        <td field="DefVal">
                                            <div class="datagrid-cell" style="width: 53px; text-align: center;">
                                                <span>默认值</span><span class="datagrid-sort-icon">&nbsp;</span></div>
                                        </td>
                                        <td field="OtherAttr">
                                            <div class="datagrid-cell" style="width: 76px; text-align: center;">
                                                <span>附加参数</span><span class="datagrid-sort-icon">&nbsp;</span></div>
                                        </td>
                                        <td field="Validator">
                                            <div class="datagrid-cell" style="width: 53px; text-align: center;">
                                                <span>验证</span><span class="datagrid-sort-icon">&nbsp;</span></div>
                                        </td>
                                        <td field="Tip">
                                            <div class="datagrid-cell" style="width: 53px; text-align: center;">
                                                <span>提示</span><span class="datagrid-sort-icon">&nbsp;</span></div>
                                        </td>
                                        <td field="ShowInList">
                                            <div class="datagrid-cell" style="width: 48px; text-align: left;">
                                                <span>列表显示</span></div>
                                        </td>
                                        <td field="ListWidth">
                                            <div class="datagrid-cell" style="width: 48px; text-align: center;">
                                                <span>列表宽度</span><span class="datagrid-sort-icon">&nbsp;</span></div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="datagrid-body" style="width: 865px; margin-top: 0px; height: 389px; overflow-x: hidden;">
                        <table class="datagrid-btable" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <%=sb.ToString()%>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
