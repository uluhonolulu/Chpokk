<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Url_Edit.ascx.cs" Inherits="$safeprojectname$.Url_EditField" %>

$if$ ($targetframeworkversion$ >= 4.5)
<asp:TextBox ID="TextBox1" runat="server" Text='<%# FieldValueEditString %>' Columns="10" TextMode="Url"></asp:TextBox>
$else$
<asp:TextBox ID="TextBox1" runat="server" Text='<%# FieldValueEditString %>' Columns="10"></asp:TextBox>
$endif$

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="DDControl DDValidator" ControlToValidate="TextBox1" Display="Static" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="DDControl DDValidator" ControlToValidate="TextBox1" Display="Static" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="DDControl DDValidator" ControlToValidate="TextBox1" Display="Static" />

