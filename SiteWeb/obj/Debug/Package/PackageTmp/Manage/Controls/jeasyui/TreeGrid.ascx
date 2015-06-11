<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreeGrid.ascx.cs" Inherits="UserControls.Controls.jeasyui.TreeGrid" %>
<script type="text/javascript">
    $(function () {
        $('#<%=this.ClientID %>_treegrid').treegrid({
            title: '<%=this.Title %>',
            fit: true,
            fitColumns: true,
            striped: true,
            animate: true,
            //autoRowHeight:false,
            singleSelect: false,
            url: '<%=this.DataUrl %>',
            idField: '<%=this.IdField %>',
            treeField: '<%=this.TreeField %>',
            showFooter:true,
            rownumbers: false,
            columns: [[ <%=ColumnToString() %>]],
            toolbar:'#<%=this.ClientID %>_tb',
            onLoadSuccess:function(row,data){
                if(data.msg){
                    if(data.msg.Msg){
                        $.messager.show({
                            title: '提示',
                            msg: data.msg.Msg,
                            timeout: 3000,
                            showType: 'fade',
                            style:{
                                left:$('#<%=this.ClientID %>_treegrid').prev().offset().left+$('#<%=this.ClientID %>_treegrid').prev().width()-250,
                                top:$('#<%=this.ClientID %>_treegrid').prev().offset().top+$('#<%=this.ClientID %>_treegrid').prev().height()-100,
                                bottom:''
                            }
                        });
                    }
                    if(data.msg.Jscript){
                        eval(data.msg.Jscript);
                    }
                }
            }            
            <%=EventsToString() %>
        });
    });
    function <%=this.ClientID %>Search(){
        var data = {<%=QueryParamsToString() %>};
        $('#<%=this.ClientID %>_treegrid').treegrid({queryParams:data});
    }
    function <%=this.ClientID %>ServerBtnClick(btnId){        
        var selectedRows = $('#<%=this.ClientID %>_treegrid').treegrid('getSelections');
        var ids = '0';
        $.each(selectedRows,function(i,obj){ids+=","+obj.Id;});
        var data = {<%=QueryParamsToString() %>};
        data.checkedIds = ids;
        data.click = btnId;
        $('#<%=this.ClientID %>_treegrid').treegrid({queryParams:data});        
        $('#<%=this.ClientID %>_treegrid').datagrid('options').queryParams = {};
    }    
    function <%=this.ClientID %>Reload(){
        $('#<%=this.ClientID %>_treegrid').treegrid('reload');
    }
</script>
<%=ToolBarToString() %>
<table id="<%=this.ClientID %>_treegrid">
</table>
