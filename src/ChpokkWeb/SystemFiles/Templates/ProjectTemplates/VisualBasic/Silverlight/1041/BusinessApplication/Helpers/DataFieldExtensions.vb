Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Public Module DataFieldExtensions
    ''' <summary>
    ''' <see cref="DataField" /> の <see cref="TextBox" /> コントロールを別のコントロールに置換し、バインディングを更新します。
    ''' </summary>
    ''' <param name="field"><see cref="TextBox"/> が置換される <see cref="DataField"/> です。</param>
    ''' <param name="newControl"><see cref="DataField.Content" /> として設定する新しいコントロールです。</param>
    ''' <param name="dataBindingProperty">データ バインディングに使用されるコントロールのプロパティです。</param>        
    ''' <param name="bindingSetupFunction">
    ''' <paramref name="newControl"/> に適用される前に新しく生成されたバインディングでパラメーターの変更に使用できる、オプションの <c>Action</c> です。
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

        ' 既存のバインディングをコピーして新しいバインディングを構築し、ユーザーが指定した関数 (ある場合) に渡します
        Dim newBinding As Binding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy()

        If bindingSetupFunction IsNot Nothing Then
            bindingSetupFunction(newBinding)
        End If

        ' 置換フィールド
        newControl.SetBinding(dataBindingProperty, newBinding)
        field.Content = newControl
    End Sub

    ''' <summary>
    ''' 別の <see cref="Binding"/> オブジェクトからすべてのプロパティをコピーして、新しい <see cref="Binding"/> オブジェクトを作成します。
    ''' </summary>
    ''' <param name="binding">プロパティ値のコピー元の <see cref="Binding"/> です</param>
    ''' <returns>新しい <see cref="Binding"/> オブジェクトです。</returns>
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

