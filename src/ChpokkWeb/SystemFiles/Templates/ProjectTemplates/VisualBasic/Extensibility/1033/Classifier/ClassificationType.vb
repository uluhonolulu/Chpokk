Imports System.ComponentModel.Composition
Imports Microsoft.VisualStudio.Text.Classification
Imports Microsoft.VisualStudio.Utilities

Module $safeprojectname$TypeDef

    ''' <summary>
    ''' Defines the "$safeprojectname$" classification type.
    ''' </summary>
    <Export(GetType(ClassificationTypeDefinition))>
    <Name("$safeprojectname$")>
    Private _$safeprojectname$Type As ClassificationTypeDefinition

End Module
