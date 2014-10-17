Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Public Module DataFieldExtensions
    ''' <summary>
    ''' 将 <see cref="DataField" /> 的 <see cref="TextBox" /> 控件替换为另一个控件并更新绑定。
    ''' </summary>
    ''' <param name="field">将替换其 <see cref="TextBox"/> 的 <see cref="DataField"/>。</param>
    ''' <param name="newControl">将设置为 <see cref="DataField.Content" /> 的新控件。</param>
    ''' <param name="dataBindingProperty">将用于数据绑定的控件属性。</param>        
    ''' <param name="bindingSetupFunction">
    ''' 一个可选 <c>Action</c>，可以用于在将新生成的绑定应用于 <paramref name="newControl"/> 之前更改该绑定的参数。
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

        ' 通过复制现有绑定并将其传递给用户提供的函数(如果有)来构造新绑定
        Dim newBinding As Binding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy()

        If bindingSetupFunction IsNot Nothing Then
            bindingSetupFunction(newBinding)
        End If

        ' 替换字段
        newControl.SetBinding(dataBindingProperty, newBinding)
        field.Content = newControl
    End Sub

    ''' <summary>
    ''' 通过从另一个 <see cref="Binding"/> 对象复制所有属性来创建新 <see cref="Binding"/> 对象。
    ''' </summary>
    ''' <param name="binding">将从其复制属性值的 <see cref="Binding"/></param>
    ''' <returns>新 <see cref="Binding"/> 对象。</returns>
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

