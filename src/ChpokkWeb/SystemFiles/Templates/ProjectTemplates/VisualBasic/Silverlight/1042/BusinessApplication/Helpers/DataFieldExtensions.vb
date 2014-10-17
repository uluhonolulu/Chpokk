Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Public Module DataFieldExtensions
    ''' <summary>
    ''' <see cref="DataField" />의 <see cref="TextBox" /> 컨트롤을 다른 컨트롤로 바꾸고 바인딩을 업데이트합니다.
    ''' </summary>
    ''' <param name="field"><see cref="TextBox"/>가 표시되는 <see cref="DataField"/>입니다.</param>
    ''' <param name="newControl"><see cref="DataField.Content" />로 설정할 새 컨트롤입니다.</param>
    ''' <param name="dataBindingProperty">데이터를 바인딩하는 데 사용되는 컨트롤 속성입니다.</param>        
    ''' <param name="bindingSetupFunction">
    ''' 새로 생성된 바인딩이 <paramref name="newControl"/>에 적용되기 전에 바인딩에 대한 매개 변수를 변경하는 데 사용할 수 있는 선택적 <c>Action</c>입니다.
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

        ' 기존 바인딩을 복사한 후 사용자가 지정한 함수(있는 경우)로 전달하여 새 바인딩을 구성합니다.
        Dim newBinding As Binding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy()

        If bindingSetupFunction IsNot Nothing Then
            bindingSetupFunction(newBinding)
        End If

        ' 필드 바꾸기
        newControl.SetBinding(dataBindingProperty, newBinding)
        field.Content = newControl
    End Sub

    ''' <summary>
    ''' 다른 <see cref="Binding"/> 개체에서 모든 속성을 복사하여 새 <see cref="Binding"/> 개체를 만듭니다.
    ''' </summary>
    ''' <param name="binding">속성 값을 복사할 <see cref="Binding"/>입니다.</param>
    ''' <returns>새 <see cref="Binding"/> 개체입니다.</returns>
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

