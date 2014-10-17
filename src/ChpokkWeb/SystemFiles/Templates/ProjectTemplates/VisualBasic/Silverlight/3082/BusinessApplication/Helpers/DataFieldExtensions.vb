Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Public Module DataFieldExtensions
    ''' <summary>
    ''' Sustituye un control <see cref="TextBox" /> de <see cref="DataField" /> por otro control y actualiza los enlaces.
    ''' </summary>
    ''' <param name="field"><see cref="DataField"/> cuyo <see cref="TextBox"/> se va a sustituir.</param>
    ''' <param name="newControl">Nuevo control que va a establecer como <see cref="DataField.Content" />.</param>
    ''' <param name="dataBindingProperty">Propiedad del control que se utilizará para el enlace de datos.</param>        
    ''' <param name="bindingSetupFunction">
    ''' <c>Action</c> opcional que se puede utilizar para cambiar parámetros en el enlace recién generado antes de aplicarlo a <paramref name="newControl"/>.
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

        ' Construir nuevo enlace al copiar el existente y pasarlo a una función proporcionada por el usuario (si la hubiera)
        Dim newBinding As Binding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy()

        If bindingSetupFunction IsNot Nothing Then
            bindingSetupFunction(newBinding)
        End If

        ' Sustituir campo
        newControl.SetBinding(dataBindingProperty, newBinding)
        field.Content = newControl
    End Sub

    ''' <summary>
    ''' Crea un nuevo objeto <see cref="Binding"/> al copiar todas las propiedades de otro objeto <see cref="Binding"/>.
    ''' </summary>
    ''' <param name="binding"><see cref="Binding"/> del que se copiarán los valores de las propiedades</param>
    ''' <returns>Nuevo objeto <see cref="Binding"/>.</returns>
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

