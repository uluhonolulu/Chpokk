Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Public Module DataFieldExtensions
    ''' <summary>
    ''' Ersetzt das <see cref="TextBox" />-Steuerelement eines <see cref="DataField" /> mit einem anderen Steuerelement und aktualisiert die Bindungen.
    ''' </summary>
    ''' <param name="field">Das <see cref="DataField"/>, dessen <see cref="TextBox"/> ersetzt wird.</param>
    ''' <param name="newControl">Das neue Steuerelement, das als <see cref="DataField.Content" /> festgelegt wird.</param>
    ''' <param name="dataBindingProperty">Die Eigenschaft des Steuerelements, die für die Datenbindung verwendet wird.</param>        
    ''' <param name="bindingSetupFunction">
    ''' Eine optionale <c>Action</c>, die verwendet werden kann, um Parameter der neu erzeugten Bindung zu ändern, bevor diese auf <paramref name="newControl"/> angewendet wird.
    ''' </param>
    <Extension()> _
    Public Sub ReplaceTextBox(ByVal field As DataField, ByVal newControl As FrameworkElement, ByVal dataBindingProperty As DependencyProperty, _
                              Optional ByVal bindingSetupFunction As Action(Of Binding) = Nothing)
        If field Is Nothing Then
            Throw New ArgumentNullException("field")
        End If

        If newControl Is Nothing Then
            Throw New ArgumentNullException("newControl")
        End If

        ' Erstellt eine neue Bindung, indem die bestehende Bindung kopiert und ggf. an eine vom Benutzer angegebene Funktion übergeben wird.
        Dim newBinding As Binding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy()

        If bindingSetupFunction IsNot Nothing Then
            bindingSetupFunction(newBinding)
        End If

        ' Feld ersetzen
        newControl.SetBinding(dataBindingProperty, newBinding)
        field.Content = newControl
    End Sub

    ''' <summary>
    ''' Erstellt ein neues <see cref="Binding"/>-Objekt, indem alle Eigenschaften eines anderen <see cref="Binding"/>-Objekts kopiert werden.
    ''' </summary>
    ''' <param name="binding"><see cref="Binding"/>, von der die Eigenschaftswerte kopiert werden</param>
    ''' <returns>Ein neues <see cref="Binding"/>-Objekt.</returns>
    <Extension()> _
    Public Function CreateCopy(ByVal binding As Binding) As binding
        If binding Is Nothing Then
            Throw New ArgumentNullException("binding")
        End If

        Dim newBinding As New Binding()

        newBinding.BindsDirectlyToSource = binding.BindsDirectlyToSource
        newBinding.Converter = binding.Converter
        newBinding.ConverterParameter = binding.ConverterParameter
        newBinding.ConverterCulture = binding.ConverterCulture
        newBinding.Mode = binding.Mode
        newBinding.NotifyOnValidationError = binding.NotifyOnValidationError
        newBinding.Path = binding.Path
        newBinding.UpdateSourceTrigger = binding.UpdateSourceTrigger
        newBinding.ValidatesOnExceptions = binding.ValidatesOnExceptions

        If binding.ElementName IsNot Nothing Then
            newBinding.ElementName = binding.ElementName
        ElseIf binding.RelativeSource IsNot Nothing Then
            newBinding.RelativeSource = binding.RelativeSource
        Else
            newBinding.Source = binding.Source
        End If

        Return newBinding
    End Function
End Module

