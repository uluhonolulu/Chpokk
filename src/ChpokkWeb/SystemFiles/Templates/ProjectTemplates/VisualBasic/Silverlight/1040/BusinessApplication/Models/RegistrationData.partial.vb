Imports System
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace Web

    Partial Public Class RegistrationData
        Private _CurrentOperation As OperationBase

        ''' <summary>
        ''' Ottiene o imposta una funzione che restituisce la password.
        ''' </summary>
        Friend Property PasswordAccessor() As Func(Of String)

        ''' <summary>
        ''' Ottiene e imposta la password.
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

                ' Non archiviare la password in un campo privato poiché non deve essere memorizzata in testo normale.
                ' L'oggetto PasswordAccessor fornito funge invece da archivio di backup per il valore.

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        ''' <summary>
        ''' Ottiene o imposta una funzione che restituisce la conferma della password.
        ''' </summary>
        Friend Property PasswordConfirmationAccessor() As Func(Of String)

        ''' <summary>
        ''' Ottiene e imposta la stringa di conferma della password.
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

                ' Non archiviare la password in un campo privato poiché non deve essere memorizzata in testo normale.
                ' L'oggetto PasswordAccessor fornito funge invece da archivio di backup per il valore.

                Me.RaisePropertyChanged("PasswordConfirmation")
            End Set
        End Property

        ''' <summary>
        ''' Ottiene o imposta l'operazione di registrazione o di accesso corrente.
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
        ''' Ottiene un valore che indica se è in corso la registrazione o l'accesso dell'utente.
        ''' </summary>
        <Display(AutoGenerateField:=False)> _
        Public ReadOnly Property IsRegistering() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentOperation)) AndAlso (Not Me.CurrentOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' Metodo di supporto per la modifica dell'operazione corrente.
        ''' Utilizzato per generare notifiche di modifica delle proprietà appropriate.
        ''' </summary>
        ''' <param name="sender">Mittente dell'evento.</param>
        ''' <param name="e">Argomenti dell'evento.</param>
        Private Sub CurrentOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsRegistering")
        End Sub

        ''' <summary>
        ''' Verifica che la password e la conferma corrispondano.
        ''' Se non corrispondono, viene aggiunto un errore di convalida.
        ''' </summary>
        Private Sub CheckPasswordConfirmation()
            ' Se non è stata ancora immessa una delle due password, non verificare la corrispondenza tra i campi.
            ' L'attributo Required verificherà che sia stato immesso un valore in entrambi i campi.
            If String.IsNullOrWhiteSpace(Me.Password) OrElse String.IsNullOrWhiteSpace(Me.PasswordConfirmation) Then
                Exit Sub
            End If

            ' Se i valori sono diversi, aggiungere un errore di convalida con entrambi i membri specificati.
            If Me.Password <> Me.PasswordConfirmation Then
                Me.ValidationErrors.Add(New ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, New String() {"PasswordConfirmation", "Password"}))
            End If
        End Sub

        ''' <summary>
        ''' Eseguire la logica dopo avere immesso il valore UserName
        ''' </summary>
        ''' <param name="userName">Nome utente immesso.</param>
        ''' <remarks>
        ''' Consentire al form di indicare quando il valore è stato immesso completamente.
        ''' L'utilizzo del metodo OnUserNameChanged può far sì che la chiamata venga effettuata prima che l'utente abbia terminato di immettere il valore nel form.
        ''' </remarks>
        Friend Sub UserNameEntered(ByVal userName As String)
            ' Riempire automaticamente FriendlyName in modo da corrispondere a UserName per le nuove entità quando non viene specificato un nome descrittivo
            If String.IsNullOrWhiteSpace(Me.FriendlyName) Then
                Me.FriendlyName = userName
            End If
        End Sub

        ''' <summary>
        ''' Crea un nuovo oggetto <see cref="LoginParameters"/> inizializzato con i dati di questa entità (IsPersistent utilizzerà false come impostazione predefinita).
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, False, Nothing)
        End Function
    End Class
End Namespace