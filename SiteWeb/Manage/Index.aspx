<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="SiteWeb.Manage.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>网站内容管理系统</title>
    <link href="plugin/jquery-easyui/themes/gray/easyui.css" id="jeasyui_maincss" rel="stylesheet"
        type="text/css" />
    <link href="plugin/jquery-easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #layout_west a
        {
            color: #888;
            text-decoration: none;
        }
        #layout_west a:hover
        {
            color: #666;
        }
        #layout_west_center ul
        {
            width: 100%;
            padding: 0;
        }
        #layout_west_center li
        {
            list-style: none;
            line-height: 31px;
            margin: 0px;
            width: 100%;
        }
        #layout_west_center li a
        {
            margin-left: 20px;
        }
        .image-grid li
        {
            width: 50px;
            height: 70px;
            padding-left: 5px;
            float: left;
            text-align: center;
            border: 1px #FFF solid;
            font-family: "Helvetica Neue" , sans-serif;
            color: #686F74;
            overflow: hidden;
        }
        img
        {
            border: none;
        }
        .image-grid a
        {
            outline: none;
            blr: expression(this.onFocus=this.blur());
        }
    </style>
    <script src="js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="plugin/jquery-easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="js/common.js" type="text/javascript"></script>
    <script type="text/javascript">

        function addTab(title, url) {
            if ($('#tabs').tabs('exists', title + '<a style="display: none;">' + url + '</a>')) {
                $('#tabs').tabs('select', title + '<a style="display: none;">' + url + '</a>'); //选中并刷新
                var currTab = $('#tabs').tabs('getSelected');
                var url = $(currTab.panel('options').content).attr('src');
                if (url != undefined && currTab.panel('options').title != 'Home') {
                    $('#tabs').tabs('update', {
                        tab: currTab,
                        options: {
                            content: createFrame(url)
                        }
                    })
                }
            } else {
                var content = createFrame(url);
                $('#tabs').tabs('add', {
                    title: title + '<a style="display: none;">' + url + '</a>',
                    content: content,
                    closable: true
                });
            }
            tabClose();
        }
        function createFrame(url) {
            var s = '<iframe scrolling="auto" frameborder="0" id="' + url + '"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
            return s;
        }

        function tabClose() {
            /*双击关闭TAB选项卡*/
            $(".tabs-inner").dblclick(function () {
                var subtitle = $(this).children(".tabs-closable").html();
                $('#tabs').tabs('close', subtitle);
            })
            /*为选项卡绑定右键*/
            $(".tabs-inner").bind('contextmenu', function (e) {
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });

                var subtitle = $(this).children(".tabs-closable").html();

                $('#mm').data("currtab", subtitle);
                $('#tabs').tabs('select', subtitle);
                return false;
            });
        }
        //绑定右键菜单事件
        function tabCloseEven() {
            //刷新
            $('#mm-tabupdate').click(function () {
                var currTab = $('#tabs').tabs('getSelected');
                var url = $(currTab.panel('options').content).attr('src');
                if (url != undefined && currTab.panel('options').title != 'Home') {
                    $('#tabs').tabs('update', {
                        tab: currTab,
                        options: {
                            content: createFrame(url)
                        }
                    })
                }
            })
            //关闭当前
            $('#mm-tabclose').click(function () {
                var currtab_title = $('#mm').data("currtab");
                alert(currtab_title);
                $('#tabs').tabs('close', currtab_title);
            })
            //全部关闭
            $('#mm-tabcloseall').click(function () {
                $('.tabs-inner span').each(function (i, n) {
                    var t = $(n).html();
                    if (t != 'Home') {
                        $('#tabs').tabs('close', t);
                    }
                });
            });
            //关闭除当前之外的TAB
            $('#mm-tabcloseother').click(function () {
                var prevall = $('.tabs-selected').prevAll();
                var nextall = $('.tabs-selected').nextAll();
                if (prevall.length > 0) {
                    prevall.each(function (i, n) {
                        var t = $('a:eq(0) span', $(n)).html();
                        if (t != 'Home') {
                            $('#tabs').tabs('close', t);
                        }
                    });
                }
                if (nextall.length > 0) {
                    nextall.each(function (i, n) {
                        var t = $('a:eq(0) span', $(n)).html();
                        if (t != 'Home') {
                            $('#tabs').tabs('close', t);
                        }
                    });
                }
                return false;
            });
        }

    </script>
    <script type="text/javascript" language="javascript">


        $(function () {
            function InitLeftChannel() {
                tabCloseEven();
                $('#left_channel li').click(function () {
                    $('#left_channel li').css({ 'border-color': '#FFF', 'background-color': '#FFF' });
                    $(this).parent().css({ 'border-color': '#FFF', 'background-color': '#B5D3F9' });
                    $('#left_channel li').removeClass('select-mark');
                    $(this).parent().addClass('select-mark');
                    var href = $(this).find('a').attr('rel');
                    var title = $(this).find('a').text();
                    addTab(title, href);
                });
                $('#layout_west li').hover(function () {
                    if (!$(this).hasClass("select-mark"))
                        $(this).css({ 'border-color': '#FFF', 'background-color': '#C5E3F9' });
                }, function () {
                    if (!$(this).hasClass("select-mark"))
                        $(this).css({ 'border-color': '#FFF', 'background-color': '#FFF' });

                });
            }
            //加载root频道
            AjaxController.Post("Index.aspx", { method: 'GetChannel', pid: 0 }, function (msg) {
                var obj = eval('(' + msg + ')');
                $.each(obj, function (key, value) {
                    $('#list').append(['<li>',
                            '<a href="javascript:;" rel="', value.Id, '">',
                                '<img src="' + value.Ico + '" width="50" height="50" alt="', value.Title, '">',
                                '<span>',
                                    value.Title,
                                '</span>',
                            '</a>',
                        '</li>'].join(''));
                });
                $('#list a').click(function () {
                    $('#layout_west .panel-header .panel-title').eq(0).html($(this).find('span').html()).css({ 'font-size': '16px', 'font-weight': 'bold' });
                    AjaxController.Post("Index.aspx", { method: 'GetChannelWithSon', pid: $(this).attr("rel") }, function (msg) {
                        //alert(msg);
                        var obj = eval('(' + msg + ')');
                        var leftChannelHtml = '<div id="left_channel" class="easyui-accordion" fit="true" border="false">';
                        $.each(obj, function (key, value) {
                            leftChannelHtml += '<div title="' + value.Title + '">';
                            leftChannelHtml += '<ul>';
                            if (value.Target) {
                                $.each(eval('(' + value.Target + ')'), function (sonKey, sonValue) {
                                    leftChannelHtml += ['<li><a ref="13" href="javascript:;" rel="', sonValue.Url, '">', sonValue.Title, '</a> </li>'].join('');
                                })
                            }
                            leftChannelHtml += '</ul>';
                            leftChannelHtml += '</div>';
                        });
                        leftChannelHtml += '</div>';
                        $('#left_channel').replaceWith(leftChannelHtml);
                        $.parser.parse('#layout_west_center');
                        InitLeftChannel();
                    });
                });
                $('#list a').last().click();
                InitLeftChannel();
            });

            //初始化站点选择select
            AjaxController.Post("Index.aspx", { method: 'GetAllSite' }, function (msg) {
                $('#siteselect').html(msg);
                $('#siteselect').change(function () {
                    AjaxController.Post("Index.aspx", { method: 'ChangeSite', sitemark: $('#siteselect').val() }, function (msg) {
                        window.location.href = location.href;
                    });
                })
            });
        });
        
    </script>
</head>
<body id="body1" class="easyui-layout">
    <div id="UIWindowBox">
    </div>
    <div region="north" id="layout_north" split="false" style="height: 55px;">
        <div class="cs-north-bg">
            <div class="cs-north-logo" style="float: left; font-weight: bold; font-size: 32px;
                padding: 4px;">
                网站内容管理系统[1.0.0.0]</div>
            <div style="float: right; padding: 18px; padding-right: 50px;">
                <a href="Login.aspx">退出</a></div>
            <div style="float: right; padding: 18px;">
                <a href="javascript:;" onclick="OpenWindow('修改密码','Permissions/AdminEdit.aspx?id=<%=this.LogonUserId %>',500,330 )">
                    修改密码</a></div>
            <div style="float: right; padding: 18px;">
                欢迎：<span style="width: 200px; margin: 0 auto; color: Red;"><%=this.LogonUserId%></span></div>
            <div style="float: right; padding: 12px;">
                切换站点：<select id="siteselect">
                </select></div>
        </div>
    </div>
    <div region="south" id="layout_south" split="false" style="height: 40px; padding: 10px;
        background: #efefef;">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="center">
                    版权信息
                </td>
            </tr>
        </table>
    </div>
    <div region="west" id="layout_west" split="true" minwidth="240" style="width: 240px;
        padding: 1px; overflow: hidden;">
        <div class="easyui-layout" fit="true">
            <div region="center" id="layout_west_center" split="true" title=" ">
                <div id="left_channel" class="easyui-accordion" fit="true" border="false">
                </div>
            </div>
            <div region="south" id="layout_west_south" style="height: 90px;">
                <ul id="list" class="image-grid" style="height: 60px; padding: 0px;">
                </ul>
            </div>
        </div>
    </div>
    <div id="layout_center" region="center" style="overflow: hidden;">
        <div id="tabs" class="easyui-tabs" fit="true" border="false">
            <div id="sitelist" title="Welcome" style="overflow: hidden;">
                <a href="javascript:void(0);"></a>
            </div>
        </div>
    </div>
    <div id="mm" class="easyui-menu cs-tab-menu" style="display: none;">
        <div id="mm-tabupdate">
            刷新</div>
        <div class="menu-sep">
        </div>
        <div id="mm-tabclose">
            关闭</div>
        <div id="mm-tabcloseother">
            关闭其他</div>
        <div id="mm-tabcloseall">
            关闭全部</div>
    </div>
</body>
</html>
