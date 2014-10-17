Imports System
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace Web

    Partial Public Class RegistrationData
        Private _CurrentOperation As OperationBase

        ''' <summary>
        ''' Ruft eine Funktion ab, die das Kennwort zurückgibt, bzw. legt diese fest.
        ''' </summary>
        Friend Property PasswordAccessor() As Func(Of String)

        ''' <summary>
        ''' Ruft das Kennwort ab bzw. legt dieses fest.
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

                ' Das Kennwort sollte nicht in einem privaten Feld gespeichert werden, ebenso wie es im Speicher nicht in Klartext gespeichert werden sollte.
                ' Stattdessen dient der angegebene PasswordAccessor als Sicherungsspeicher für den Wert.

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        ''' <summary>
        ''' Ruft eine Funktion ab, die die Kennwortbestätigung zurückgibt, bzw. legt diese fest.
        ''' </summary>
        Friend Property PasswordConfirmationAccessor() As Func(Of String)

        ''' <summary>
        ''' Ruft die Zeichenfolge der Kennwortbestätigung ab, bzw. legt diese fest.
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

                ' Das Kennwort sollte nicht in einem privaten Feld gespeichert werden, ebenso wie es im Speicher nicht in Klartext gespeichert werden sollte.
                ' Stattdessen dient der angegebene PasswordAccessor als Sicherungsspeicher für den Wert.

                Me.RaisePropertyChanged("PasswordConfirmation")
            End Set
        End Property

        ''' <summary>
        ''' Ruft den derzeitigen Registrierungs- oder Anmeldevorgang ab bzw. legt diesen fest.
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
        ''' Ruft einen Wert ab, der angibt, ob der Benutzer gerade registriert oder angemeldet wird.
        ''' </summary>
        <Display(AutoGenerateField:=False)> _
        Public ReadOnly Property IsRegistering() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentOperation)) AndAlso (Not Me.CurrentOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' Hilfsmethode bei einer Änderung des derzeitigen Vorgangs.
        ''' Wird verwendet, um entsprechende Benachrichtigungen für die Änderung der Eigenschaft auszulösen.
        ''' </summary>
        ''' <param name="sender">Der Ereignissender.</param>
        ''' <param name="e">Die event-Argumente.</param>
        Private Sub CurrentOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsRegistering")
        End Sub

        ''' <summary>
        ''' Überprüft, ob Passwort und Passwortbestätigung übereinstimmen.
        ''' Wenn dies nicht der Fall ist, wird ein Validierungsfehler angefügt.
        ''' </summary>
        Private Sub CheckPasswordConfirmation()
            ' Wenn eines der beiden Kennwörter noch nicht eingegeben wurde, diese beiden Felder nicht auf Übereinstimmung prüfen.
            ' Das Required-Attribut stellt sicher, dass in beiden Feldern ein Wert eingegeben wurde.
            If String.IsNullOrWhiteSpace(Me.Password) OrElse String.IsNullOrWhiteSpace(Me.PasswordConfirmation) Then
                Exit Sub
            End If

            ' Falls die Werte nicht übereinstimmen, wird ein Validierungsfehler angefügt, in dem beide Mitglieder angegeben sind.
            If Me.Password <> Me.PasswordConfirmation Then
                Me.ValidationErrors.Add(New ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, New String() {"PasswordConfirmation", "Password"}))
            End If
        End Sub

        ''' <summary>
        ''' Logik durchführen, nachdem der Wert "UserName" eingegeben wurde
        ''' </summary>
        ''' <param name="userName">Der Benutzername, der eingegeben wurde.</param>
        ''' <remarks>
        ''' Zulassen, dass das Formular anzeigt, wann der Wert vollständig eingegeben wurde.
        ''' Die Verwendung der Methode "OnUserNameChanged" kann zu einem verfrühten Aufruf führen, bevor der Benutzer die Eingabe des Wertes im Formular beendet hat.
        ''' </remarks>
        Friend Sub UserNameEntered(ByVal userName As String)
            ' AutoAusfüllen des FriendlyName, damit dieser mit UserName übereinstimmt, wenn bei neuen Entitäten kein Anzeigename angegeben ist
            If String.IsNullOrWhiteSpace(Me.FriendlyName) Then
                Me.FriendlyName = userName
            End If
        End Sub

        ''' <summary>
        ''' Erstellt neue <see cref="LoginParameters"/>, die mit den Daten dieser Entität initialisiert werden (IsPersistent erhält den Standardwert "false").
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, False, Nothing)
        End Function
    End Class
End Namespace