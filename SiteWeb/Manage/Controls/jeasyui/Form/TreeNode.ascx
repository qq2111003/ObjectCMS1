<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreeNode.ascx.cs" Inherits="UserControls.Controls.jeasyui.Form.TreeNode" %>
<input id="<%=this.ClientID %>_TreeNode" value="<%=SelectedValue %>" name="<%=this.ClientID.Replace("_", "$") %>$TreeNode" class="easyui-combotree"
    data-options="url:'<%=DataUrl %>'" style="width: 200px;" />
