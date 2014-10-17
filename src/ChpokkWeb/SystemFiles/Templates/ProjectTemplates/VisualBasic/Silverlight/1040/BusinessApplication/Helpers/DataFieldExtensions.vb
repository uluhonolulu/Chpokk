Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Public Module DataFieldExtensions
    ''' <summary>
    ''' Sostituisce il controllo <see cref="TextBox" /> di un oggetto <see cref="DataField" /> con un altro controllo e aggiorna le associazioni.
    ''' </summary>
    ''' <param name="field">Oggetto <see cref="DataField"/> di cui verrà sostituito il controllo <see cref="TextBox"/>.</param>
    ''' <param name="newControl">Nuovo controllo che verrà impostato come <see cref="DataField.Content" />.</param>
    ''' <param name="dataBindingProperty">Proprietà del controllo che verrà utilizzata per l'associazione dati.</param>        
    ''' <param name="bindingSetupFunction">
    ''' Oggetto <c>Action</c> facoltativo che è possibile utilizzare per modificare i parametri nell'associazione appena generata prima che venga applicato a <paramref name="newControl"/>.
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

        ' Costruire una nuova associazione copiando quella esistente e passandola a una funzione fornita dall'utente, se disponibile
        Dim newBinding As Binding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy()

        If bindingSetupFunction IsNot Nothing Then
            bindingSetupFunction(newBinding)
        End If

        ' Sostituire campo
        newControl.SetBinding(dataBindingProperty, newBinding)
        field.Content = newControl
    End Sub

    ''' <summary>
    ''' Crea un nuovo oggetto <see cref="Binding"/> copiando tutte le proprietà da un altro oggetto <see cref="Binding"/>.
    ''' </summary>
    ''' <param name="binding">Oggetto <see cref="Binding"/> da cui verranno copiati i valori delle proprietà</param>
    ''' <returns>Nuovo oggetto <see cref="Binding"/>.</returns>
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

