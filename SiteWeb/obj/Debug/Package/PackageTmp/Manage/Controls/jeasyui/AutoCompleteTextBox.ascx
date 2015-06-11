<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoCompleteTextBox.ascx.cs"
    Inherits="LogSystem.Controls.jeasyui.AutoCompleteTextBox" %>
<div>
    <input type="text" id="key" runat="server" style="display: inline-block; white-space: nowrap;
        font-size: 12px; margin: 0; padding: 0; border: 1px solid #d3d3d3; background: #fff;
        width: 198px;" placeholder="" autocomplete="off">
    <div style="overflow: auto; position: absolute; border: 1px solid #99BBE8; width: 198px;
        height: 120px; display: none; background-color: #FFF; opacity: 0.95;">
    </div>
    <script type="text/javascript">
        $(function () {
            $('#<%=this.ClientID %>_key').keyup(function () {
                if ($(this).val().length == 0) {
                    $(this).next().hide();
                    return;
                }
                AjaxController.Post("<%=this.DataUrl %>", { '<%=this.key.ClientID %>': $(this).val() }, function (str) {
                    data = eval('(' + str + ')');
                    var dataHtml = '';
                    if (!data.length || data.length == 0) {
                        $('#<%=this.key.ClientID %>').next().hide();
                        return;
                    }
                    $.each(data, function (a, b) {
                        dataHtml += '<div style="font-size: 12px;padding: 3px;padding-right: 0px;">' + b + '</div>';
                    });
                    $('#<%=this.key.ClientID %>').next().html(dataHtml);

                    $('#<%=this.key.ClientID %>').next().children().hover(function () {
                        $(this).css('backgroundColor', '#EBF3FD').siblings().css('backgroundColor', '');
                    }).click(function () {
                        $('#<%=this.key.ClientID %>').val($(this).html());
                        $('#<%=this.key.ClientID %>').next().hide();
                    });
                    $('#<%=this.key.ClientID %>').next().show();
                });
            });
        });
    </script>
</div>
