Imports System.Diagnostics.CodeAnalysis
Imports System.Security.Principal
Imports System.Transactions
Imports System.Web.Routing
Imports DotNetOpenAuth.AspNet
Imports Microsoft.Web.WebPages.OAuth
Imports WebMatrix.WebData

<Authorize()> _
<InitializeSimpleMembership()>
Public Class AccountController
    Inherits System.Web.Mvc.Controller

    '
    ' POST: /Account/JsonLogin

    <AllowAnonymous()> _
    <HttpPost()> _
    Public Function JsonLogin(model As LoginModel, returnUrl As String) As JsonResult
        If ModelState.IsValid Then
            If WebSecurity.Login(model.UserName, model.Password, persistCookie:= model.RememberMe) Then
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe)
                Return Json(New With {.success = True, .redirect = returnUrl})
            Else
                ModelState.AddModelError("", "The user name or password provided is incorrect.")
            End If
        End If

        ' If we got this far, something failed
        Return Json(New With { .errors = GetErrorsFromModelState() })
    End Function

    '
    ' POST: /Account/LogOff

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Public Function LogOff() As ActionResult
        WebSecurity.Logout()

        Return RedirectToAction("Index", "Home")
    End Function

    '
    ' POST: /Account/JsonRegister

    <AllowAnonymous()> _
    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Public Function JsonRegister(model As RegisterModel, returnUrl As String) As ActionResult
        If ModelState.IsValid Then
            ' Attempt to register the user
            Try
                WebSecurity.CreateUserAndAccount(model.UserName, model.Password)
                WebSecurity.Login(model.UserName, model.Password)

                InitiateDatabaseForNewUser(model.UserName)

                FormsAuthentication.SetAuthCookie(model.UserName, createPersistentCookie:=False)
                Return Json(New With {.success = True, .redirect = returnUrl})
            Catch e As MembershipCreateUserException
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode))
            End Try
        End If

        ' If we got this far, something failed
        Return Json(New With {.errors = GetErrorsFromModelState()})
    End Function

    ' <summary>
    ' Initiate a new todo list for new user
    ' </summary>
    ' <param name="userName"></param>
    Private Shared Sub InitiateDatabaseForNewUser(userName As String)
        Dim db As New TodoItemContext()
        Dim todoList As New TodoList()
        todoList.UserId = userName
        todoList.Title = "My Todo List #1"
        todoList.Todos = New List(Of TodoItem)()
        db.TodoLists.Add(todoList)
        db.SaveChanges()

        todoList.Todos.Add(new TodoItem() With { .Title = "Todo item #1", .TodoListId = todoList.TodoListId, .IsDone = False })
        todoList.Todos.Add(new TodoItem() With { .Title = "Todo item #2", .TodoListId = todoList.TodoListId, .IsDone = False })
        db.SaveChanges()
    End Sub

    '
    ' POST: /Account/Disassociate

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Public Function Disassociate(ByVal provider As String, ByVal providerUserId As String) As ActionResult
        ' Wrap in a transaction to prevent the user from accidentally disassociating all their accounts at one time.

        Dim ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId)
        Dim message As ManageMessageId? = Nothing

        ' Only disassociate the account if the currently logged in user is the owner
        If ownerAccount = User.Identity.Name Then
            ' Use a transaction to prevent the user from deleting their last login credential
            Using scope As New TransactionScope(TransactionScopeOption.Required, New TransactionOptions With {.IsolationLevel = IsolationLevel.Serializable})
                Dim hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name))
                If hasLocalAccount OrElse OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1 Then
                    OAuthWebSecurity.DeleteAccount(provider, providerUserId)
                    scope.Complete()
                    message = ManageMessageId.RemoveLoginSuccess
                End If
            End Using
        End If

        Return RedirectToAction("Manage", New With {.Message = message})
    End Function

    '
    ' GET: /Account/Manage

    Public Function Manage(ByVal message As ManageMessageId?) As ActionResult
        ViewData("StatusMessage") =
            If(message = ManageMessageId.ChangePasswordSuccess, "Your password has been changed.", _
                If(message = ManageMessageId.SetPasswordSuccess, "Your password has been set.", _
                    If(message = ManageMessageId.RemoveLoginSuccess, "The external login was removed.", _
                        "")))

        ViewData("HasLocalPassword") = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name))
        ViewData("ReturnUrl") = Url.Action("Manage")
        Return View()
    End Function

    '
    ' POST: /Account/Manage

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Public Function Manage(ByVal model As LocalPasswordModel) As ActionResult
        Dim hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name))
        ViewData("HasLocalPassword") = hasLocalAccount
        ViewData("ReturnUrl") = Url.Action("Manage")
        If hasLocalAccount Then
            If ModelState.IsValid Then
                ' ChangePassword will throw an exception rather than return false in certain failure scenarios.
                Dim changePasswordSucceeded As Boolean

                Try
                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword)
                Catch e As Exception
                    changePasswordSucceeded = False
                End Try

                If changePasswordSucceeded Then
                    Return RedirectToAction("Manage", New With {.Message = ManageMessageId.ChangePasswordSuccess})
                Else
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.")
                End If
            End If
        Else
            ' User does not have a local password so remove any validation errors caused by a missing
            ' OldPassword field
            Dim state = ModelState("OldPassword")
            If state IsNot Nothing Then
                state.Errors.Clear()
            End If

            If ModelState.IsValid Then
                Try
                    WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword)
                    Return RedirectToAction("Manage", New With {.Message = ManageMessageId.SetPasswordSuccess})
                Catch e As Exception
                    ModelState.AddModelError("", e)
                End Try
            End If
        End If

        ' If we got this far, something failed, redisplay form 
        Return View(model)
    End Function

    '
    ' POST: /Account/ExternalLogin

    <HttpPost()> _
    <AllowAnonymous()> _
    <ValidateAntiForgeryToken()> _
    Public Function ExternalLogin(ByVal provider As String, ByVal returnUrl As String) As ActionResult
        Return New ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", New With {.ReturnUrl = returnUrl}))
    End Function

    '
    ' GET: /Account/ExternalLoginCallback

    <AllowAnonymous()> _
    Public Function ExternalLoginCallback(ByVal returnUrl As String) As ActionResult
        Dim result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", New With {.ReturnUrl = returnUrl}))
        If Not result.IsSuccessful Then
            Return RedirectToAction("ExternalLoginFailure")
        End If

        If OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie:=False) Then
            Return RedirectToLocal(returnUrl)
        End If

        If User.Identity.IsAuthenticated Then
            ' If the current user is logged in add the new account
            OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name)
            Return RedirectToLocal(returnUrl)
        Else
            ' User is new, ask for their desired membership name
            Dim loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId)
            ViewData("ProviderDisplayName") = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName
            ViewData("ReturnUrl") = returnUrl
            Return View("ExternalLoginConfirmation", New RegisterExternalLoginModel With {.UserName = result.UserName, .ExternalLoginData = loginData})
        End If
    End Function

    '
    ' POST: /Account/ExternalLoginConfirmation

    <HttpPost()> _
    <AllowAnonymous()> _
    <ValidateAntiForgeryToken()> _
    Public Function ExternalLoginConfirmation(ByVal model As RegisterExternalLoginModel, ByVal returnUrl As String) As ActionResult
        Dim provider As String = Nothing
        Dim providerUserId As String = Nothing

        If User.Identity.IsAuthenticated OrElse Not OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, provider, providerUserId) Then
            Return RedirectToAction("Manage")
        End If

        If ModelState.IsValid Then
            ' Insert a new user into the database
            Using db As New UsersContext()
                Dim user = db.UserProfiles.FirstOrDefault(Function(u) u.UserName.ToLower() = model.UserName.ToLower())
                ' Check if user already exists
                If user Is Nothing Then
                    ' Insert name into the profile table
                    db.UserProfiles.Add(New UserProfile With {.UserName = model.UserName})
                    db.SaveChanges()

                    InitiateDatabaseForNewUser(model.UserName)

                    OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName)
                    OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie:=False)

                    Return RedirectToLocal(returnUrl)
                Else
                    ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.")
                End If
            End Using
        End If

        ViewData("ProviderDisplayName") = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName
        ViewData("ReturnUrl") = returnUrl
        Return View(model)
    End Function

    '
    ' GET: /Account/ExternalLoginFailure

    <AllowAnonymous()> _
    Public Function ExternalLoginFailure() As ActionResult
        Return View()
    End Function

    <AllowAnonymous()> _
    <ChildActionOnly()> _
    Public Function ExternalLoginsList(ByVal returnUrl As String) As ActionResult
        ViewData("ReturnUrl") = returnUrl
        Return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData)
    End Function

    <ChildActionOnly()> _
    Public Function RemoveExternalLogins() As ActionResult
        Dim accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name)
        Dim externalLogins = New List(Of ExternalLogin)()
        For Each account As OAuthAccount In accounts
            Dim clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider)

            externalLogins.Add(New ExternalLogin With { _
                .Provider = account.Provider, _
                .ProviderDisplayName = clientData.DisplayName, _
                .ProviderUserId = account.ProviderUserId _
            })
        Next

        ViewData("ShowRemoveButton") = externalLogins.Count > 1 OrElse OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name))
        Return PartialView("_RemoveExternalLoginsPartial", externalLogins)
    End Function

#Region "Helpers"
    Private Function RedirectToLocal(ByVal returnUrl As String) As ActionResult
        If Url.IsLocalUrl(returnUrl) Then
            Return Redirect(returnUrl)
        Else
            Return RedirectToAction("Index", "Home")
        End If
    End Function

    Public Enum ManageMessageId
        ChangePasswordSuccess
        SetPasswordSuccess
        RemoveLoginSuccess
    End Enum

    Friend Class ExternalLoginResult
        Inherits System.Web.Mvc.ActionResult

        Private ReadOnly _provider As String
        Private ReadOnly _returnUrl As String

        Public Sub New(ByVal provider As String, ByVal returnUrl As String)
            _provider = provider
            _returnUrl = returnUrl
        End Sub

        Public ReadOnly Property Provider() As String
            Get
                Return _provider
            End Get
        End Property

        Public ReadOnly Property ReturnUrl() As String
            Get
                Return _returnUrl
            End Get
        End Property

        Public Overrides Sub ExecuteResult(ByVal context As ControllerContext)
            OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl)
        End Sub
    End Class
    
    Private Function GetErrorsFromModelState() As IEnumerable(Of String)
        Return ModelState.SelectMany(Function(x) x.Value.Errors.Select(Function(e) e.ErrorMessage))
    End Function

    Public Function ErrorCodeToString(ByVal createStatus As MembershipCreateStatus) As String
        ' See http://go.microsoft.com/fwlink/?LinkID=177550 for
        ' a full list of status codes.
        Select Case createStatus
            Case MembershipCreateStatus.DuplicateUserName
                Return "User name already exists. Please enter a different user name."

            Case MembershipCreateStatus.DuplicateEmail
                Return "A user name for that e-mail address already exists. Please enter a different e-mail address."

            Case MembershipCreateStatus.InvalidPassword
                Return "The password provided is invalid. Please enter a valid password value."

            Case MembershipCreateStatus.InvalidEmail
                Return "The e-mail address provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.InvalidAnswer
                Return "The password retrieval answer provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.InvalidQuestion
                Return "The password retrieval question provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.InvalidUserName
                Return "The user name provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.ProviderError
                Return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator."

            Case MembershipCreateStatus.UserRejected
                Return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator."

            Case Else
                Return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator."
        End Select
    End Function
#End Region

End Class
