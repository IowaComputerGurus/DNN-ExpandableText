<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditExpandableTextHtml.ascx.cs" Inherits="ICG.Modules.ExpandableTextHtml.EditExpandableTextHtml" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>

<div class="dnnForm icgETHEdit dnnClear">
    <h2 class="dnnFormSectionHead"><%= Localization.GetString("ItemContent", this.LocalResourceFile) %></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblContentId" runat="server" suffix=":" />
            <asp:Literal ID="litContentId" runat="server" Mode="PassThrough" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblTitle" runat="server" ControlName="txtTitle" Suffix=":" CssClass="dnnLeft dnnFormRequired" />
            <dnn:TextEditor ID="txtTitle" runat="server" Height="200" Width="100%"  />
	        <asp:RequiredFieldValidator ID="valTitle" resourcekey="valTitle.ErrorMessage" ControlToValidate="txtTitle"
	            cssclass="dnnFormMessage dnnFormError" Display="Dynamic" runat="server"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblBody" runat="server" ControlName="txtBody" Suffix=":" CssClass="dnnLeft dnnFormRequired" />
            <dnn:TextEditor ID="txtBody" runat="server" Height="300" Width="100%" />
	        <asp:RequiredFieldValidator ID="valBody" resourcekey="valBody.ErrorMessage" ControlToValidate="txtBody"
	            cssclass="dnnFormMessage dnnFormError" Display="Dynamic" runat="server"></asp:RequiredFieldValidator>
        </div>
    </fieldset>
    <h2 class="dnnFormSectionHead"><%= Localization.GetString("PublishOptions", this.LocalResourceFile) %></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblIsExpanded" runat="server" ControlName="chkIsExpanded" Suffix=":" />
            <asp:CheckBox ID="chkIsExpanded" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPublishDate" runat="server" ControlName="txtPublishDate" Suffix=":" cssClass="dnnFormRequired" />
            <asp:TextBox ID="txtPublishDate" runat="server" MaxLength="10" Columns="12" />
            <asp:CompareValidator ID="PublishDateFormat" runat="server" ControlToValidate="txtPublishDate" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" Operator="DataTypeCheck" Type="Date" resourcekey="DateField" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblRequiredRole" runat="server" ControlName="ddlRequiredRole" Suffix=":" />
            <asp:DropDownList ID="ddlRequiredRole" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSortOrder" runat="server" ControlName="txtSortOrder" Suffix=":" cssClass="dnnFormRequired" />
            <asp:TextBox ID="txtSortOrder" MaxLength="10" runat="server" Columns="4" CssClass="dnnFormRequired"></asp:TextBox>
	        <asp:RequiredFieldValidator ID="valSortOrder" runat="server" Display="dynamic" ControlToValidate="txtSortOrder"
	            CssClass="dnnFormMessage dnnFormError" resourcekey="valSortOrder.ErrorMessage"></asp:RequiredFieldValidator>
	        <asp:CompareValidator ID="SortOrderFormat" runat="server" Display="dynamic" ControlToValidate="txtSortOrder"
	            CssClass="dnnFormMessage dnnFormError" resourcekey="SortOrderFormat.ErrorMessage" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
        </div>
    </fieldset> 

    <ul class="dnnActions dnnClear">
        <li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" resourcekey="cmdUpdate" runat="server"  text="Update" OnClick="cmdUpdate_Click" /></li>
        <li><asp:linkbutton cssclass="dnnSecondaryAction" id="cmdCancel" resourcekey="cmdCancel" runat="server" text="Cancel" causesvalidation="False" OnClick="cmdCancel_Click" /></li>
        <li><asp:linkbutton cssclass="dnnSecondaryAction" id="cmdDelete" resourcekey="cmdDelete" runat="server" text="Delete" causesvalidation="False" OnClick="cmdDelete_Click" /></li>
    </ul>

</div>

<script language="javascript" type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function setUpEditEntry() {
            $('#<%= cmdDelete.ClientID %>').dnnConfirm({
                text: '<%= Localization.GetString("DeleteItem.Text", LocalResourceFile) %>',
                yesText: '<%= Localization.GetString("Yes.Text", Localization.SharedResourceFile) %>',
                noText: '<%= Localization.GetString("No.Text", Localization.SharedResourceFile) %>',
                title: '<%= Localization.GetString("Confirm.Text", Localization.SharedResourceFile) %>'
            });
        }

        $(document).ready(function () {
            setUpEditEntry();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpEditEntry();
            });
        });
    } (jQuery, window.Sys));
</script>