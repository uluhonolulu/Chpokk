Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Public Module DataFieldExtensions
    ''' <summary>
    ''' Заменяет элемент управления <see cref="TextBox" /> для <see cref="DataField" /> другим элементом управления и обновляет привязку.
    ''' </summary>
    ''' <param name="field">Объект <see cref="DataField"/>, элемент управления <see cref="TextBox"/> которого нужно заменить.</param>
    ''' <param name="newControl">Новый элемент управления, который должен быть назначен в качестве <see cref="DataField.Content" />.</param>
    ''' <param name="dataBindingProperty">Свойство элемента управления, которое служит для привязки данных.</param>        
    ''' <param name="bindingSetupFunction">
    ''' Необязательный параметр <c>Action</c>, который может быть использован для изменения параметров во вновь созданной привязке, прежде чем она будет применена к <paramref name="newControl"/>.
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

        ' Создание новой привязки путем копирования существующей с дальнейшей ее передачей функции, указанной пользователем (если указана)
        Dim newBinding As Binding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy()

        If bindingSetupFunction IsNot Nothing Then
            bindingSetupFunction(newBinding)
        End If

        ' Замена поля
        newControl.SetBinding(dataBindingProperty, newBinding)
        field.Content = newControl
    End Sub

    ''' <summary>
    ''' Создает новый объект <see cref="Binding"/>, копируя все свойства из другого объекта <see cref="Binding"/>.
    ''' </summary>
    ''' <param name="binding"><see cref="Binding"/>, из которого будут скопированы значения свойств</param>
    ''' <returns>Новый объект <see cref="Binding"/>.</returns>
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

