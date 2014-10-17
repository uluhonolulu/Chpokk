Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls


<DefaultProperty("Text"), ToolboxData("<{0}:ServerControl1 runat=server></{0}:ServerControl1>")>
Public Class ServerControl1
    Inherits WebControl

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)>
    Property Text() As String
        Get
            Dim s As String = CStr(ViewState("Text"))
            If s Is Nothing Then
                Return "[" & Me.ID & "]"
            Else
                Return s
            End If
        End Get

        Set(ByVal Value As String)
            ViewState("Text") = Value
        End Set
    End Property

    Protected Overrides Sub RenderContents(ByVal output As HtmlTextWriter)
        output.Write(Text)
    End Sub

End Class
