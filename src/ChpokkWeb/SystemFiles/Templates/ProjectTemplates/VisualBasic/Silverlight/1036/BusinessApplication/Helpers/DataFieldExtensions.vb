Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Public Module DataFieldExtensions
    ''' <summary>
    ''' Remplace le contrôle <see cref="TextBox" /> d'un <see cref="DataField" /> par un autre contrôle et met à jour les liaisons.
    ''' </summary>
    ''' <param name="field"><see cref="DataField"/> dont <see cref="TextBox"/> sera remplacé.</param>
    ''' <param name="newControl">Nouveau contrôle que vous allez définir en tant que <see cref="DataField.Content" />.</param>
    ''' <param name="dataBindingProperty">Propriété du contrôle qui sera utilisée pour la liaison de données.</param>        
    ''' <param name="bindingSetupFunction">
    ''' <c>Action</c> optionnelle que vous pouvez utiliser pour modifier des paramètres sur la liaison qui vient d'être créée avant qu'elle soit appliquée à <paramref name="newControl"/>.
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

        ' Construire une nouvelle liaison en en copiant une existante, et en la passant à une fonction donnée par l'utilisateur (le cas échéant)
        Dim newBinding As Binding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy()

        If bindingSetupFunction IsNot Nothing Then
            bindingSetupFunction(newBinding)
        End If

        ' Remplacer le champ
        newControl.SetBinding(dataBindingProperty, newBinding)
        field.Content = newControl
    End Sub

    ''' <summary>
    ''' Crée un nouvel objet <see cref="Binding"/> en copiant toutes les propriétés d'un autre objet <see cref="Binding"/>.
    ''' </summary>
    ''' <param name="binding"><see cref="Binding"/> à partir duquel les valeurs de propriétés seront copiées</param>
    ''' <returns>Nouvel objet <see cref="Binding"/>.</returns>
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

