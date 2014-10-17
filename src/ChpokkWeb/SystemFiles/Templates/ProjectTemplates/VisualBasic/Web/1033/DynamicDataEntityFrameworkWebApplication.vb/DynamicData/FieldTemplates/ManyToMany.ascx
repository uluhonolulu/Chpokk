<%@ Control Language="VB" CodeBehind="ManyToMany.ascx.vb" Inherits="$safeprojectname$.ManyToManyField" %>

<asp:Repeater ID="Repeater1" runat="server">
    <ItemTemplate>
      <asp:DynamicHyperLink runat="server"></asp:DynamicHyperLink>
    </ItemTemplate>
</asp:Repeater>

