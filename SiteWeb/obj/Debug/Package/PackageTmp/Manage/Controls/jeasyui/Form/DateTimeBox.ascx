<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTimeBox.ascx.cs"
    Inherits="UserControls.Controls.jeasyui.Form.DateTimeBox" %>
<asp:TextBox ID="tb_DateTime" runat="server" Width="85px"></asp:TextBox>
<script type="text/javascript">
    $(function () {
        <%=MinMaxDateJs() %>
        <%if(this.DateModel==UserControls.Controls.jeasyui.Form.ShowType.OnlyDate){ %>
        $('#<%=this.tb_DateTime.ClientID %>').datebox({
            onSelect: function (date) {
                if(maxDate&&date.getTime()>=maxDate){
                    alert("超出最大限制");
                    var gotoDate = new Date();
                    gotoDate.setTime(maxDate);
                    $('#<%=this.tb_DateTime.ClientID %>').datebox('setValue', gotoDate.format("yyyy-MM-dd"));
                }
                if(minDate&&date.getTime()<minDate){
                    alert("超出最小限制");
                    var gotoDate = new Date();
                    gotoDate.setTime(minDate);
                    $('#<%=this.tb_DateTime.ClientID %>').datebox('setValue', gotoDate.format("yyyy-MM-dd"));
                }
            }
        });
        <%}else if(this.DateModel==UserControls.Controls.jeasyui.Form.ShowType.OnlyTime){ %>

        $('#<%=this.tb_DateTime.ClientID %>').timespinner({
            <%=MinMaxTimeJs() %>
        });  
        <%}else{ %>
        $('#<%=this.tb_DateTime.ClientID %>').datetimebox({
            showSeconds: false ,
            onSelect: function (date) {
                if(maxDate&&date.getTime()>=maxDate){
                    alert("超出最大限制");
                    var gotoDate = new Date();
                    gotoDate.setTime(maxDate);
                    $('#<%=this.tb_DateTime.ClientID %>').datebox('setValue', gotoDate.format("yyyy-MM-dd hh:mm"));
                }
                if(minDate&&date.getTime()<minDate){
                    alert("超出最小限制");
                    var gotoDate = new Date();
                    gotoDate.setTime(minDate);
                    $('#<%=this.tb_DateTime.ClientID %>').datebox('setValue', gotoDate.format("yyyy-MM-dd hh:mm"));
                }
            }
        });  
        <%} %>
    });
</script>
