<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewExpandableTextHtml.ascx.cs" Inherits="ICG.Modules.ExpandableTextHtml.ViewExpandableTextHtml" %>

<asp:Repeater ID="rptListing" runat="server" OnItemDataBound="rptListing_ItemDataBound">
    <HeaderTemplate>
        <asp:Literal ID="litHeader" runat="server" Mode="PassThrough"></asp:Literal>
    </HeaderTemplate>
    <ItemTemplate>
        <asp:Literal ID="litItem" runat="server" Mode="PassThrough"></asp:Literal>
    </ItemTemplate>
    <FooterTemplate>
        <asp:Literal ID="litFooter" runat="server" Mode="PassThrough"></asp:Literal>
    </FooterTemplate>   
</asp:Repeater>

<p runat="server" id="pShowAll" visible="false">
<asp:HyperLink ID="hlShowAll" runat="server" CssClass="CommandButton" />
</p>