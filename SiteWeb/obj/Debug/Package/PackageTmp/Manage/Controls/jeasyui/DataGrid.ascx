<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataGrid.ascx.cs" Inherits="UserControls.Controls.DataGrid" %>
<script type="text/javascript">
    $(function () {
        $('#<%=this.ClientID %>_datagrid').datagrid({
            title: '<%=this.Title %>',
            fit: true,
            fitColumns: true,
            striped: true,
            //autoRowHeight:false,
            url: '<%=this.DataUrl %>',
            idField: 'Id',
            sortName:'<%=this.SortField %>',
            sortOrder:'<%=this.SortOrder %>',
            pagination: <%=this.Pagination.ToString().ToLower() %>,
            pageSize: 25,
            pageList: [25, 50, 100],
            rownumbers: false,
            columns: [[ <%=ColumnToString() %>]],
            toolbar:'#<%=this.ClientID %>_tb',
            onLoadSuccess:function(data){
                if(data.msg){
                    if(data.msg.Msg){
                        $.messager.show({title: '提示',msg: data.msg.Msg,timeout: 3000,showType: 'fade',
                            style:{
                                left:$('#<%=this.ClientID %>_datagrid').prev().offset().left+$('#<%=this.ClientID %>_datagrid').prev().width()-250,
                                top:$('#<%=this.ClientID %>_datagrid').prev().offset().top+$('#<%=this.ClientID %>_datagrid').prev().height()-100,
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
        $($('#<%=this.ClientID %>_datagrid').datagrid('getPager')).pagination(); 
    });
    function <%=this.ClientID %>Search(){
        var data = {<%=QueryParamsToString() %>};
        $('#<%=this.ClientID %>_datagrid').datagrid({queryParams:data});
    }
    function <%=this.ClientID %>ServerBtnClick(btnId){       
        var ids = <%=this.ClientID %>GetSelectedIds().join(',');     
        var data = {<%=QueryParamsToString() %>};
        data.checkedIds = ids;
        data.click = btnId;
        $('#<%=this.ClientID %>_datagrid').datagrid({queryParams:data});        
        $('#<%=this.ClientID %>_datagrid').datagrid('options').queryParams = {};
    }    
    function <%=this.ClientID %>Reload(){
        $('#<%=this.ClientID %>_datagrid').datagrid('reload');
    }
    function <%=this.ClientID %>GetSelectedIds(){        
        var selectedRows = $('#<%=this.ClientID %>_datagrid').datagrid('getSelections');
        var ids = [];
        $.each(selectedRows,function(i,obj){ids.push(obj.Id);});
        return ids;
    }
</script>
<%=ToolBarToString() %>
<table id="<%=this.ClientID %>_datagrid">
</table>
