Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Public Module DataFieldExtensions
    ''' <summary>
    ''' 用另一個控制項取代 <see cref="DataField" />'s <see cref="TextBox" /> 控制項並且更新繫結。
    ''' </summary>
    ''' <param name="field">要取代其 <see cref="TextBox"/> 的 <see cref="DataField"/>。</param>
    ''' <param name="newControl">您要設定成 <see cref="DataField.Content" /> 的新控制項。</param>
    ''' <param name="dataBindingProperty">要使用於資料繫結之控制項的屬性。</param>        
    ''' <param name="bindingSetupFunction">
    ''' 選擇性 <c>Action</c>，可用來變更剛產生的繫結上的參數，然後再將它套用到 <paramref name="newControl"/>。
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

        ' 複製現有的繫結來建構新繫結，並且傳遞給使用者指定的函式 (如果有)
        Dim newBinding As Binding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy()

        If bindingSetupFunction IsNot Nothing Then
            bindingSetupFunction(newBinding)
        End If

        ' 取代欄位
        newControl.SetBinding(dataBindingProperty, newBinding)
        field.Content = newControl
    End Sub

    ''' <summary>
    ''' 從另一個 <see cref="Binding"/> 物件複製所有屬性來建立新 <see cref="Binding"/> 物件。
    ''' </summary>
    ''' <param name="binding">要複製屬性值的來源 <see cref="Binding"/></param>
    ''' <returns>新的 <see cref="Binding"/> 物件。</returns>
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

