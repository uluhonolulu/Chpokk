Imports System.ComponentModel.Composition
Imports Microsoft.VisualStudio.Text.Editor
Imports Microsoft.VisualStudio.Utilities

''' <summary>
''' Export a <see cref="IWpfTextViewMarginProvider"/>, which returns an instance of the margin for the editor
''' to use.
''' </summary>
<Export(GetType(IWpfTextViewMarginProvider))>
<Name($safeprojectname$.MarginName)>
<Order(After := PredefinedMarginNames.HorizontalScrollBar)>
<MarginContainer(PredefinedMarginNames.Bottom)>
<ContentType("text")>
<TextViewRole(PredefinedTextViewRoles.Interactive)> 
NotInheritable Class MarginFactory
    Implements IWpfTextViewMarginProvider
    
    Public Function CreateMargin(ByVal textViewHost As IWpfTextViewHost, ByVal containerMargin As IWpfTextViewMargin) As IWpfTextViewMargin Implements IWpfTextViewMarginProvider.CreateMargin
        Return New $safeprojectname$(textViewHost.TextView)
    End Function
    
End Class
