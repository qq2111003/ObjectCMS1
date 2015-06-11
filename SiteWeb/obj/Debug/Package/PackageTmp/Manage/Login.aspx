<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SiteWeb.Manage.Login" %>
<script>
    if (parent.location.href != location.href) {
        parent.location.href = '/Manage/Login.aspx';
    }
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" type="image/ico" href="/images/favicon.ico" />
    <title>登录到目标在线网站内容管理系统</title>
    <link href="css/styles.css" type="text/css" media="screen" rel="stylesheet" />
    <link href="css/jquery-ui-1.8.16.custom.css" rel="stylesheet">
    <script src="js/jquery-1.8.0.min.js"></script>
    <script src="js/jquery-ui-1.8.16.custom.min.js"></script>
    <script type="text/javascript" src="js/jquery.keyboard.extension-typing.js"></script>
    <link type="text/css" href="css/keyboard.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.keyboard.js"></script>
</head>
<body id="login">
    <div id="wrappertop">
    </div>
    <div id="wrapper">
        <div id="content">
            <div id="header">
                <h1>
                    <a href="">
                        <!--����Logo <img src="logo.png"   height="50"  width="100"  alt="logo">-->
                    </a>
                </h1>
            </div>
            <div id="darkbanner" class="banner320">
                <h2>
                    网站管理</h2>
            </div>
            <div id="darkbannerwrap">
            </div>
            <form id="form1" runat="server">
            <fieldset class="form">
                <p>
                    <label class="loginlabel" for="user_name">
                        用户名:</label>
                    <asp:TextBox ID="user_name" runat="server" class="logininput ui-keyboard-input ui-widget-content ui-corner-all"></asp:TextBox>
                </p>
                <p>
                    <label class="loginlabel" for="user_password">
                        密 码:</label>
                    <span>
                        <asp:TextBox ID="user_password" runat="server" class="logininput" TextMode="Password"></asp:TextBox><img
                            id="passwd" class="tooltip" alt="Click to open the virtual keyboard" title="Click to open the virtual keyboard"
                            src="images/keyboard.png" /></span>
                </p>
                <p id="check_p" runat="server" visible="false">
                    <label class="loginlabel" for="user_name">
                        验证码:</label>
                    <asp:TextBox ID="check_code" runat="server" class="logininput ui-keyboard-input ui-widget-content ui-corner-all"
                        Style="width: 60px"></asp:TextBox>
                    <img src="Login.aspx?method=GetValiCode" id="CodeImg" onclick="document.getElementById('CodeImg').src = 'Login.aspx?method=GetValiCode&'+Math.random()" />
                    <span style="cursor: pointer;" onclick="document.getElementById('CodeImg').src = 'Login.aspx?method=GetValiCode&'+Math.random()">
                        刷新</span>
                </p>
                <button id="loginbtn" type="button" class="positive" name="Submit">
                    <img src="images/key.png" alt="Announcement" />登录</button>
            </fieldset>
            </form>
        </div>
    </div>
    <div id="wrapperbottom_branding">
        <div id="wrapperbottom_branding_text">
            Copyright
        </div>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#user_password').keyboard({
                    openOn: null,
                    stayOpen: true,
                    layout: 'qwerty'
                }).addTyping();
                $('#passwd').click(function () {
                    $('#user_password').getkeyboard().reveal();
                })

                $(".logininput").blur(function () {
                    if ($(this).val() == "") {
                        $(this).css("border-color", "red");
                    }
                    else
                        $(this).css("border-color", "#D9D6C4");
                })

                $("#loginbtn").click(function () {
                    var k = 0;
                    $(".logininput").each(function (i, obj) {
                        if ($(obj).val() == "") {
                            k++;
                            $(this).css("border-color", "red");
                            $(this).focus();
                            return false;
                        }
                    });
                    if (k != 0) return;

                    $("#loginbtn").html("Loading....  <img src='images/loading.gif' alt='请稍等' /> ");
                    $("#loginbtn").attr("disabled", "disabled");
                    $('#form1').submit();
                })
            });
        
        </script>
</body>
