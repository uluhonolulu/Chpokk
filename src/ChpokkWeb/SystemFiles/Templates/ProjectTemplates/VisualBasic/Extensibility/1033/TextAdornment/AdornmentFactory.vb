Imports System.ComponentModel.Composition
Imports Microsoft.VisualStudio.Text.Editor
Imports Microsoft.VisualStudio.Utilities

''' <summary>
''' Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
''' that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
''' </summary>
<Export(GetType(IWpfTextViewCreationListener))>
<ContentType("text")>
<TextViewRole(PredefinedTextViewRoles.Document)>
NotInheritable Class EditorAdornmentFactory
    Implements IWpfTextViewCreationListener

    ''' <summary>
    ''' Defines the adornment layer for the scarlet adornment. This layer is ordered 
    ''' after the selection layer in the Z-order
    ''' </summary>
    <Export(GetType(AdornmentLayerDefinition))>
    <Name("$safeprojectname$")>
    <Order(After:=PredefinedAdornmentLayers.Selection, Before:=PredefinedAdornmentLayers.Text)>
    Private _editorAdornmentLayer As AdornmentLayerDefinition

	''' <summary>
	''' Creates a $safeprojectname$ manager when a textview is created
	''' </summary>
	''' <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
	Public Sub TextViewCreated(ByVal textView As IWpfTextView) Implements IWpfTextViewCreationListener.TextViewCreated

        Dim temp$safeprojectname$ = New $safeprojectname$(textView)

    End Sub
End Class
