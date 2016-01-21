<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="ICG.Modules.ExpandableTextHtml.Settings" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>

<div class="dnnForm icgETHSettings dnnClear" id="icgETHSettings">
    <h2 id="dnnPanel-DisplayOptions" class="dnnFormSectionHead"><a href=""><%=Localization.GetString("DisplayOptions", this.LocalResourceFile) %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="lblSortOrder" runat="server" controlname="cboSortOrder" suffix=":"></dnn:label>
            <asp:DropDownList ID="ddlSortOrder" runat="server" >
                <asp:ListItem Text="Title Ascending (Default)" Value="ORDER BY title"></asp:ListItem>
                <asp:ListItem Text="Title Descending" Value="ORDER BY title DESC"></asp:ListItem>
                <asp:ListItem Text="Last Modified Date Ascending" Value="ORDER BY LastUpdated"></asp:ListItem>
                <asp:ListItem Text="Last Modified Date Descending" Value="ORDER BY LastUpdated DESC"></asp:ListItem>
                <asp:ListItem Text="Sort Order Ascending" Value="ORDER BY SortOrder"></asp:ListItem>
                <asp:ListItem Text="Sort Order Descending" Value="ORDER BY SortOrder DESC"></asp:ListItem>
                <asp:ListItem Text="Random Order" Value="ORDER BY NEWID()"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblExpandOnPrint" runat="server" ControlName="chkExpandOnPrint" Suffix=":" />
            <asp:CheckBox ID="chkExpandOnPrint" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblShowExpandCollapseAll" runat="server" ControlName="chkShowExpandCollapseAll" Suffix=":" />
            <asp:CheckBox ID="chkShowExpandCollapseAll" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblDefaultShowLimit" runat="server" ControlName="ddlDefaultShowLimit" Suffix=":" />
            <asp:DropDownList ID="ddlDefaultShowLimit" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddlDefaultShowLimit_SelectedIndexChanged">
                <asp:ListItem Text="All" Value="-1" />
                <asp:ListItem Text="5 Entries" Value="5" />
                <asp:ListItem Text="10 Entries" Value="10" />
                <asp:ListItem Text="15 Entries" Value="15" />
                <asp:ListItem Text="20 Entries" Value="20" />
            </asp:DropDownList>
        </div>
        <div class="dnnFormItem" id="divShowAll" runat="server">
            <dnn:Label ID="lblShowAllText" runat="server" ControlName="txtShowAllText" Suffix=":" />
            <asp:TextBox ID="txtShowAllText" runat="server" Width="200px" MaxLength="255" />
        </div>
    </fieldset>

    <h2 id="dnnPanel-StyleOptions" class="dnnFormSectionHead"><a href=""><%= Localization.GetString("StyleOptions", this.LocalResourceFile) %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblTitleCssClass" runat="server" ControlName="txtTitleCssClass" Suffix=":" />
            <asp:TextBox ID="txtTitleCssClass" runat="server" MaxLength="100" CssClass="dnnFormRequired"></asp:TextBox>
            <asp:RequiredFieldValidator ID="TitleCssClassRequired" runat="server" ControlToValidate="txtTitleCssClass"
                cssclass="dnnFormError dnnFormMessage" Display="Dynamic" resourcekey="TitleCssClassRequired.Text"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblContentCssClass" runat="server" ControlName="txtContentCssClass" Suffix=":" />
            <asp:TextBox ID="txtContentCssClass" runat="server" MaxLength="100" CssClass="dnnFormRequired"></asp:TextBox>
            <asp:RequiredFieldValidator ID="ContentCssClassRequired" runat="server" ControlToValidate="txtContentCssClass"
                CssClass="dnnFormError dnnFormMessage" Display="dynamic" resourcekey="ContentCssClassRequired.Text"></asp:RequiredFieldValidator>
        </div>
    </fieldset>

    <h2 id="dnnPanel-OptionalHeader" class="dnnFormSectionHead" ><a href=""><%= Localization.GetString("OptionalHeader", this.LocalResourceFile) %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblHeader" runat="server" ControlName="txtHeader" Suffix=":" />
            <dnn:TextEditor ID="txtHeader" runat="server" Width="100%" Height="400" />
        </div>
    </fieldset>
</div>

<script language="javascript" type="text/javascript">

    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function setUpEditEntry() {
            $('#icgETHEntry').dnnPanels();

        }

        $(document).ready(function () {
            setUpEditEntry();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpEditEntry();
            });
        });
    } (jQuery, window.Sys));
    </script>
