<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fastReg.aspx.cs" Inherits="SiteWeb.plugin.fastReg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        *
        {
            margin: 0;
            padding: 0;
        }
        .float
        {
            height: 424px;
            width: 279px;
            position: absolute;
            right: 0;
            top: 100px;
        }
        .float_l
        {
            background: #710900;
            height: 420px;
            width: 229px;
            border: 1px solid #290300;
            float: left;
        }
        .float_r
        {
            background: url(images/hover_06.jpg) no-repeat;
            height: 189px;
            width: 46px;
            float: right;
        }
        .error
        {
            background: #fec23b;
            height: 20xp;
            width: 184px;
            border: 1px solid #290300:font-family:微软雅黑;
            font-size: 14px;
            color: #FF0000;
            position: absolute;
            top: -26px;
            line-height: 22px;
            text-align: center;
        }
        .fl_up
        {
            background: url(images/up_03.jpg) no-repeat;
            height: 94px;
            width: 229px;
        }
        .fl_down
        {
            font-family: 微软雅黑;
            font-size: 14px;
            color: #d0a08b;
            position: relative;
        }
        .inputli
        {
            height: 22px;
            margin-bottom: 13px;
            overflow: hidden;
            line-height: 22px;
        }
        .tittle
        {
            float: left;
            width: 56px;
        }
        .reinput
        {
            background: #ffffeb;
            border: 1px solid #640a00;
            float: left;
            height: 20px;
            width: 124px;
        }
        .yz_reinput
        {
            background: #ffffeb;
            border: 1px solid #640a00;
            float: left;
            height: 20px;
            width: 59px;
        }
        .yzimg
        {
            height: 18px;
            width: 61px;
            float: left;
            line-height: 22px;
            margin-left: 4px;
            margin-top: 1px;
        }
        .checkdiv
        {
            margin-top: 17px;
            margin-bottom: 24px;
        }
        .checkdiv a
        {
            color: #d0a08b;
        }
        .now
        {
            background: url(images/reg_sub.jpg) no-repeat;
            height: 48px;
            width: 154px;
            margin-left: 15px; ;display:block;}
        .now:hover
        {
            background-position: 0 -48px;
        }
        .rebox
        {
            padding: 0 0 0 24px;
        }
        .rebox1
        {
            padding: 0 0 0 15px;
        }
        .re_p1
        {
            height: 100px;
            position: relative;
        }
        .h1
        {
            font-family: 微软雅黑;
            font-size: 16px;
            color: #ffdb4a;
            height: 24px;
        }
        .download
        {
            background: url(images/down_07.jpg) no-repeat;
            height: 47px;
            width: 147px;
            display: block;
            margin-left: 26px;
        }
        .p11
        {
            font-family: 微软雅黑;
            font-size: 14px;
            color: #deb4a1;
            height: 24px;
            width: 210px;
            line-height: 36px;
        }
        .showName
        {
            color: #ffdb4a;
        }
        .re_p2
        {
            height: 62px;
        }
        .hotpi1
        {
            color: #ffdb4a;
        }
        .cfm
        {
            background: url(images/fcm_14.jpg);
            height: 46px;
            width: 151px;
            display: block;
            margin-left: 26px;
        }
        .cfm1
        {
            background: url(images/mb.jpg);
            height: 49px;
            width: 151px;
            display: block;
            margin-left: 26px;
        }
    </style>
    <script src="js/jquery-144.min.js" type="text/javascript"></script>
    <script src="js/formcheck.js" type="text/javascript"></script>
</head>
<body>
    <div class="fl_up">
    </div>
    <div class="fl_down">
        <div class="rebox regbox" style="">
            <div class="error errorTipDiv" style="display: none;">
                请填写正确的身份信息</div>
            <div class="inputli">
                <div class="tittle">
                    账&nbsp;号：</div>
                <div class="reinput">
                    <input id="txtUserName" maxlength="12" autocomplete="off" class="reinput" />
                </div>
            </div>
            <div class="inputli">
                <div class="tittle">
                    密&nbsp;码：</div>
                <div class="reinput">
                    <input type="password" class="reinput" name="" id="txtPassword" />
                    <input id="copyPass" autocomplete="off" maxlength="12" class="reinput" type="text"
                        style="display: none;" />
                </div>
            </div>
            <div class="inputli">
                <div class="tittle">
                    邮&nbsp;箱：</div>
                <div class="reinput">
                    <input id="txtEmail" autocomplete="off" class="reinput" />
                </div>
            </div>
            <div class="inputli">
                <div class="tittle">
                    姓&nbsp;名：</div>
                <div class="reinput">
                    <input id="txtRealName" autocomplete="off" class="reinput" />
                </div>
            </div>
            <div class="inputli">
                <div class="tittle">
                    身份证：</div>
                <div class="reinput">
                    <input id="txtIdCard" autocomplete="off" class="reinput" />
                </div>
            </div>
            <div class="inputli">
                <div class="tittle">
                    验证码：</div>
                <div class="yz_reinput">
                    <input id="txtValidateCode" maxlength="6" class="yz_reinput" />
                </div>
                <div class="yzimg">
                    <script language="javascript" type="text/javascript">
                        document.write('<img id="vcode" class="yzimg" src="/HttpHandler/AuthCodeHandler.ashx?r=' + Math.random() + '" title="点击更换一张"/>');
                    </script>
                </div>
            </div>
            <div class="checkdiv">
                <input type="checkbox" id="chkProtocol" class="reg_checkin" checked="checked">&nbsp;接受[<a
                    target="_blank" href="http://acc.object.com.cn/Protocol/regprotocol.aspx
    ">服务条款</a>]和[<a target="_blank" href="http://acc.object.com.cn/Protocol/privacy.html 
    ">隐私条款</a>]
            </div>
            <input class="now" type="submit" style="border: 0;" id="btnSubmit" value="" />
        </div>
        <div class="rebox1 regcuccbox" style="display: none;">
            <div class="re_p1">
                <div class="h1">
                    恭喜你！注册成功！</div>
                <div class="p11">
                    您的账号是：<span class="showName"></span></div>
                <div class="p11">
                    下载体验吧：</div>
            </div>
            <a href="#" target="_blank" class="download"></a>
            <div class="re_p2">
                <div class="p11">
                    您的账号目前安全级别为：<span class="hotpi1 red"></span></div>
                <div class="p11">
                    建议您进行以下操作：</div>
            </div>
            <a href="http://acc.object.com.cn/AccountManage/PreventLossInfo.aspx" target="_blank"
                class="cfm"></a><a href="http://acc.object.com.cn/SafeManage/SecurityInfo.aspx" target="_blank"
                    class="cfm1"></a>
        </div>
    </div>
    <script src="js/append.js" type="text/javascript"></script>
</body>
</html>
