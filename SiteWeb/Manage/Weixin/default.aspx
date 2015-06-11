<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="WeixinApp._default" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td align="right">
                    appid：
                </td>
                <td>
                    <asp:TextBox ID="_txtAppid" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    appsecret：
                </td>
                <td>
                    <asp:TextBox ID="_txtAppsecret" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="_btnAccessToken" runat="server" Text="获取access_token" OnClick="_btnAccessToken_Click" />
                </td>
            </tr>
        </table>
        <asp:Label ID="_lblMsg" runat="server" Text=""></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <br />
        <table>
            <tr>
                <td align="right">access_token：</td>
                <td>
                    <asp:TextBox ID="_txtacetoken" TextMode="MultiLine" Rows="5" Columns="40" runat="server"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td align="right">自定义菜单内容：</td>
                <td>
                    <asp:TextBox ID="_txtMenu" TextMode="MultiLine" Rows="10" Columns="40" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="_btnSetMenu" runat="server" Text="设置菜单" 
                        onclick="_btnSetMenu_Click"  />
                    <asp:Button ID="_btnDelMenu" runat="server" Text="删除自定义菜单" 
                        onclick="_btnDelMenu_Click"  />
                </td>
            </tr>
        </table>
        <asp:Label ID="_lblMsg2" runat="server" Text=""></asp:Label>        
    </div>
    </form>
</body>
</html>
