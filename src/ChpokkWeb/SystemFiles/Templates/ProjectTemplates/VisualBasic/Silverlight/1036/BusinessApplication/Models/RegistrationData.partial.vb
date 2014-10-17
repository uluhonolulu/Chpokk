Imports System
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace Web

    Partial Public Class RegistrationData
        Private _CurrentOperation As OperationBase

        ''' <summary>
        ''' Obtient ou définit une fonction qui retourne le mot de passe.
        ''' </summary>
        Friend Property PasswordAccessor() As Func(Of String)

        ''' <summary>
        ''' Obtient et définit le mot de passe.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=3, Name:="PasswordLabel", Description:="PasswordDescription", ResourceType:=GetType(RegistrationDataResources))> _
        <RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessageResourceName:="ValidationErrorBadPasswordStrength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <StringLength(50, MinimumLength:=7, ErrorMessageResourceName:="ValidationErrorBadPasswordLength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property Password() As String
            Get
                Return If((Me.PasswordAccessor Is Nothing), String.Empty, Me.PasswordAccessor.Invoke())
            End Get

            Set(ByVal value As String)
                Me.ValidateProperty("Password", value)
                Me.CheckPasswordConfirmation()

                ' Ne pas stocker le mot de passe dans un champ privé car il ne doit pas être stocké en mémoire en texte brut.
                ' À la place, le PasswordAccessor fourni sert de magasin de stockage pour la valeur.

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        ''' <summary>
        ''' Obtient ou définit une fonction qui retourne la confirmation du mot de passe.
        ''' </summary>
        Friend Property PasswordConfirmationAccessor() As Func(Of String)

        ''' <summary>
        ''' Obtient et définit la chaîne de confirmation du mot de passe.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=4, Name:="PasswordConfirmationLabel", ResourceType:=GetType(RegistrationDataResources))> _
        Public Property PasswordConfirmation() As String
            Get
                Return If((Me.PasswordConfirmationAccessor Is Nothing), String.Empty, Me.PasswordConfirmationAccessor.Invoke())
            End Get

            Set(ByVal value As String)
                Me.ValidateProperty("PasswordConfirmation", value)
                Me.CheckPasswordConfirmation()

                ' Ne pas stocker le mot de passe dans un champ privé car il ne doit pas être stocké en mémoire en texte brut.
                ' À la place, le PasswordAccessor fourni sert de magasin de stockage pour la valeur.

                Me.RaisePropertyChanged("PasswordConfirmation")
            End Set
        End Property

        ''' <summary>
        ''' Obtient ou définit l'opération d'inscription ou de connexion en cours.
        ''' </summary>
        Friend Property CurrentOperation() As OperationBase
            Get
                Return _CurrentOperation
            End Get
            Set(ByVal value As OperationBase)
                If Not Object.Equals(_CurrentOperation, value) Then
                    If Not IsNothing(_CurrentOperation) Then
                        RemoveHandler _CurrentOperation.Completed, AddressOf Me.CurrentOperationChanged
                    End If

                    _CurrentOperation = value

                    If Not IsNothing(_CurrentOperation) Then
                        AddHandler _CurrentOperation.Completed, AddressOf Me.CurrentOperationChanged
                    End If

                    Me.CurrentOperationChanged(Me, EventArgs.Empty)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Obtient une valeur indiquant si l'utilisateur est actuellement en cours d'inscription ou de connexion.
        ''' </summary>
        <Display(AutoGenerateField:=False)> _
        Public ReadOnly Property IsRegistering() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentOperation)) AndAlso (Not Me.CurrentOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' Méthode d'assistance quand l'opération en cours change.
        ''' Utilisé pour déclencher les notifications de modification de propriété appropriées.
        ''' </summary>
        ''' <param name="sender">Émetteur de l'événement.</param>
        ''' <param name="e">Arguments de event.</param>
        Private Sub CurrentOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsRegistering")
        End Sub

        ''' <summary>
        ''' Vérifie que le mot de passe et la confirmation sont identiques.
        ''' S'ils ne correspondent pas, une erreur de validation est ajoutée.
        ''' </summary>
        Private Sub CheckPasswordConfirmation()
            ' Si l'un des mots de passe n'a pas encore été entré, ne pas tester l'égalité entre les champs.
            ' L'attribut Required garantit qu'une valeur a été entrée pour les deux champs.
            If String.IsNullOrWhiteSpace(Me.Password) OrElse String.IsNullOrWhiteSpace(Me.PasswordConfirmation) Then
                Exit Sub
            End If

            ' Si les valeurs sont différentes, ajouter une erreur de validation aux deux membres spécifiés.
            If Me.Password <> Me.PasswordConfirmation Then
                Me.ValidationErrors.Add(New ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, New String() {"PasswordConfirmation", "Password"}))
            End If
        End Sub

        ''' <summary>
        ''' Exécuter la logique une fois la valeur UserName entrée
        ''' </summary>
        ''' <param name="userName">Nom d'utilisateur qui a été entré.</param>
        ''' <remarks>
        ''' Permettre au formulaire d'indiquer quand la valeur a été complètement entrée.
        ''' L'utilisation de la méthode OnUserNameChanged peut entraîner un appel prématuré avant que l'utilisateur ait terminé d'entrer la valeur dans le formulaire.
        ''' </remarks>
        Friend Sub UserNameEntered(ByVal userName As String)
            ' FriendlyName à remplissage automatique pour faire correspondre UserName avec de nouvelles entrées lorsqu'aucun nom convivial n'est spécifié
            If String.IsNullOrWhiteSpace(Me.FriendlyName) Then
                Me.FriendlyName = userName
            End If
        End Sub

        ''' <summary>
        ''' Crée un nouveau <see cref="LoginParameters"/> initialisé avec les données de cette entité (la valeur par défaut d'IsPersistent est false).
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, False, Nothing)
        End Function
    End Class
End Namespace