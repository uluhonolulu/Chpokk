Imports System.ComponentModel.Composition
Imports System.Windows.Media
Imports Microsoft.VisualStudio.Text.Classification
Imports Microsoft.VisualStudio.Utilities

''' <summary>
''' Defines an editor format for our $safeprojectname$ type that has a purple background
''' and is underlined.
''' </summary>
<Export(GetType(EditorFormatDefinition))>
<ClassificationType(ClassificationTypeNames:="$safeprojectname$")>
<Name("$safeprojectname$")>
<UserVisible(True)>
<Order(After:=Priority.Default)>
NotInheritable Class $safeprojectname$Format
    Inherits ClassificationFormatDefinition
    
    ''' <summary>
    ''' Defines the visual format for the "$safeprojectname$" classification type
    ''' </summary>
    Public Sub New()
        Me.DisplayName = "$safeprojectname$"
        Me.BackgroundColor = Colors.BlueViolet
        Me.TextDecorations = System.Windows.TextDecorations.Underline
    End Sub
    
End Class
