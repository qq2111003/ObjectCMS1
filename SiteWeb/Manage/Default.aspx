<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SiteWeb.Manage.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=GBK">
    <title>AOAOV在线网站内容管理系统</title>
    <link href="css/default.css" rel="stylesheet" type="text/css" />
    <link href="plugin/jquery-easyui/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
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
            var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
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

        $(function () {
            tabCloseEven();

            $('.cs-navi-tab').click(function () {
                var $this = $(this);
                var href = $this.attr('src');
                var title = $this.text();
                addTab(title, href);
            });


        });

    </script>
    <script>

        $(document).ready(function () {
            //unset iframe_url cookie
            $(".menuTitle1").click(function () {
                if ($(this).hasClass("activeTitle")) {
                    $(this).removeClass("activeTitle");
                    $(this).next('div').slideUp('fast');
                } else {
                    $(this).addClass("activeTitle");
                    $(this).next('div').slideDown('fast');
                }
            });

        });
    </script>
</head>
<body class="easyui-layout">
    <div region="north" border="true" class="cs-north">
        <div class="cs-north-bg">
            <div class="cs-north-logo">
                AOAOV在线网站内容管理系统[1.0.0.0]</div>
            <div style="float: right; padding: 18px; padding-right: 50px;">
                <a href="Login.aspx">退出</a></div>
            <div style="float: right; padding: 18px;">
                <a href="javascript:;" onclick="OpenWindow('修改密码','Admin/AdminEdit.aspx?id=<%=userid %>',500,330 )">
                    修改密码</a></div>
            <div style="float: right; padding: 18px;">
                欢迎：<span style="width: 200px; margin: 0 auto; color: Red;"><%=Session["AdminManage"]%></span></div>
        </div>
    </div>
    <div region="west" border="true" split="true" title="管理导航" class="cs-west">
        <div class="easyui-accordion" fit="true" border="false">
            <%=LoadChannel()%>
        </div>
    </div>
    <div id="mainPanle" region="center" border="true" border="false">
        <div id="tabs" class="easyui-tabs" fit="true" border="false">
            <div title="Home">
                <div class="cs-home-remark">
                </div>
            </div>
        </div>
    </div>
    <div region="south" border="false" class="cs-south">
        ©2013 北京AOAOV在线科技有限公司
    </div>
    <div id="mm" class="easyui-menu cs-tab-menu">
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
