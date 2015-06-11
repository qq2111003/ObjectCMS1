using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserControls.Controls;
using ObjectCMS.BLL;
using ObjectCMS.Model.ModelConfig;
using System.Data;
using System.Text;

namespace SiteWeb.Manage.Model
{
    public partial class NodeSetField :PageBase
    {

        int nodeId = 0;
        protected StringBuilder sb = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.HasPermission(10, "fieldset");
            if (!int.TryParse(Request.QueryString["id"], out nodeId))
            {

            }
            if (!IsPostBack)
            {
                var dt = ModelManage.Instance.GetAllColumn(nodeId);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.AppendLine("                                <tr class=\"datagrid-row  " + (i == 0 ? "datagrid-row-alt" : "") + "\">");
                    sb.AppendLine("                                    <td field=\"Id\">");
                    sb.AppendLine("                                        <div style=\"text-align: center; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-Id\">");
                    sb.AppendLine("                                            " + dt.Rows[i]["Id"] + "</div>");
                    sb.AppendLine("                                    </td>");
                    sb.AppendLine("                                    <td field=\"FieldName\">");
                    sb.AppendLine("                                        <div style=\"text-align: left; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-FieldName\">");
                    sb.AppendLine("                                            " + dt.Rows[i]["FieldName"] + "</div>");
                    sb.AppendLine("                                    </td>");
                    sb.AppendLine("                                    <td field=\"FieldType\">");
                    sb.AppendLine("                                        <div style=\"text-align: left; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-FieldType\">");
                    sb.AppendLine("                                            " + dt.Rows[i]["FieldType"] + "</div>");
                    sb.AppendLine("                                    </td>");
                    sb.AppendLine("                                    <td field=\"Title\">");
                    sb.AppendLine("                                        <div style=\"text-align: left; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-Title\">");
                    sb.AppendLine("                                            " + dt.Rows[i]["Title"] + "</div>");
                    sb.AppendLine("                                    </td>");
                    sb.AppendLine("                                    <td field=\"flag\">");
                    sb.AppendLine("                                        <div style=\"text-align: left; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-flag\">");
                    sb.AppendLine("                                            <input type=\"checkbox\" name=\"flag\" value=\"" + dt.Rows[i]["Id"] + "\" " + (Convert.ToInt32(dt.Rows[i]["flag"]) == 1 ? "checked=\"checked\"" : "") + "></div>");
                    sb.AppendLine("                                    </td>");
                    sb.AppendLine("                                    <td field=\"ReWriteTitle\">");
                    sb.AppendLine("                                        <div style=\"text-align: center; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-ReWriteTitle\">");
                    sb.AppendLine("                                            <input type=\"text\" name=\"ReWriteTitle_" + dt.Rows[i]["Id"] + "\" style=\"width: 70px; border: solid 1px #ccc;\" value=\"" + (Convert.IsDBNull(dt.Rows[i]["ReWriteTitle"]) || string.IsNullOrEmpty(dt.Rows[i]["ReWriteTitle"].ToString()) ? dt.Rows[i]["Title"] : dt.Rows[i]["ReWriteTitle"]) + "\"></div>");
                    sb.AppendLine("                                    </td>");
                    sb.AppendLine("                                    <td field=\"Sort\">");
                    sb.AppendLine("                                        <div style=\"text-align: center; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-Sort\">");
                    sb.AppendLine("                                            <input type=\"text\" name=\"Sort_" + dt.Rows[i]["Id"] + "\" style=\"width: 45px; border: solid 1px #ccc; text-align: right;\"");
                    sb.AppendLine("                                                value=\"" + (Convert.IsDBNull(dt.Rows[i]["Sort"]) ? i.ToString() : dt.Rows[i]["Sort"]) + "\"></div>");
                    sb.AppendLine("                                    </td>");

                    sb.AppendLine("                                    <td field=\"DefVal\">");
                    sb.AppendLine("                                        <div style=\"text-align: center; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-DefVal\">");
                    sb.AppendLine("                                            <input type=\"text\" name=\"DefVal_" + dt.Rows[i]["Id"] + "\" style=\"width: 45px; border: solid 1px #ccc; text-align: right;\"");
                    sb.AppendLine("                                                value=\"" + (Convert.IsDBNull(dt.Rows[i]["DefVal"]) ? "" : dt.Rows[i]["DefVal"]) + "\"></div>");
                    sb.AppendLine("                                    </td>");

                    sb.AppendLine("                                    <td field=\"OtherAttr\">");
                    sb.AppendLine("                                        <div style=\"text-align: center; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-OtherAttr\">");
                    sb.AppendLine("                                            <input type=\"text\" name=\"OtherAttr_" + dt.Rows[i]["Id"] + "\" style=\"width: 70px; border: solid 1px #ccc; text-align: right;\"");
                    sb.AppendLine("                                                value=\"" + (Convert.IsDBNull(dt.Rows[i]["OtherAttr"]) ? "" : dt.Rows[i]["OtherAttr"]) + "\"></div>");
                    sb.AppendLine("                                    </td>");

                    sb.AppendLine("                                    <td field=\"Validator\">");
                    sb.AppendLine("                                        <div style=\"text-align: center; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-Validator\">");
                    sb.AppendLine("                                            <input type=\"text\" name=\"Validator_" + dt.Rows[i]["Id"] + "\" style=\"width: 45px; border: solid 1px #ccc; text-align: right;\"");
                    sb.AppendLine("                                                value=\"" + (Convert.IsDBNull(dt.Rows[i]["Validator"]) ? "" : dt.Rows[i]["Validator"]) + "\"></div>");
                    sb.AppendLine("                                    </td>");

                    sb.AppendLine("                                    <td field=\"Tip\">");
                    sb.AppendLine("                                        <div style=\"text-align: center; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-Tip\">");
                    sb.AppendLine("                                            <input type=\"text\" name=\"Tip_" + dt.Rows[i]["Id"] + "\" style=\"width: 45px; border: solid 1px #ccc; text-align: right;\"");
                    sb.AppendLine("                                                value=\"" + (Convert.IsDBNull(dt.Rows[i]["Tip"]) ? "" : dt.Rows[i]["Tip"]) + "\"></div>");
                    sb.AppendLine("                                    </td>");
                    sb.AppendLine("                                    <td field=\"ShowInList\">");
                    sb.AppendLine("                                        <div style=\"text-align: left; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-ShowInList\">");
                    sb.AppendLine("                                            <input type=\"checkbox\" name=\"ShowInList\" value=\"" + dt.Rows[i]["Id"] + "\" " + (!Convert.IsDBNull(dt.Rows[i]["ShowInList"]) && Convert.ToInt32(dt.Rows[i]["ShowInList"]) == 1 ? "checked=\"checked\"" : "") + "></div>");
                    sb.AppendLine("                                    </td>");
                    sb.AppendLine("                                    <td field=\"ListWidth\">");
                    sb.AppendLine("                                        <div style=\"text-align: center; height: auto;\" class=\"datagrid-cell datagrid-cell-c1-ListWidth\">");
                    sb.AppendLine("                                            <input type=\"text\" name=\"ListWidth_" + dt.Rows[i]["Id"] + "\" style=\"width: 45px; border: solid 1px #ccc; text-align: right;\"");
                    sb.AppendLine("                                                value=\"" + dt.Rows[i]["ListWidth"] + "\"></div>");
                    sb.AppendLine("                                    </td>");
                    sb.AppendLine("                                </tr>");
                }
            }
            else
            {
                string[] flagVal = Request["flag"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string[] ShowInListVal = Request["ShowInList"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                NodeUserModelField.DeleteByWhere("NodeId=" + nodeId);
                for (int i = 0; i < flagVal.Length; i++)
                {
                    NodeUserModelField numf = new NodeUserModelField()
                    {
                        UserModelFieldId = int.Parse(flagVal[i]),
                        NodeId = nodeId,
                        ShowInList = ShowInListVal.Contains(flagVal[i]),
                        ListWidth = Convert.ToInt32(Request["ListWidth_" + flagVal[i]]),
                        ReWriteTitle = Request["ReWriteTitle_" + flagVal[i]],
                        Sort = Convert.ToInt32(Request["Sort_" + flagVal[i]]),
                        DefVal = Request["DefVal_" + flagVal[i]],
                        OtherAttr = Request["OtherAttr_" + flagVal[i]],
                        Validator = Request["Validator_" + flagVal[i]],
                        Tip = Request["Tip_" + flagVal[i]]
                    };
                    numf.Insert();
                }

                Response.Write("<script>parent.Message.show('设置成功','提示');parent.UIDialog.Close();</script>");
                Response.End();
            }
        }


    }
}