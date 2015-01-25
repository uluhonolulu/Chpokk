@ModelType $safeprojectname$.LoginModel
@Code
    ViewData("Title") = "My Todo List"
End Code
@Functions
    Public Function GetAntiForgeryToken() As String
        Dim cookieToken As String = String.Empty
        Dim formToken As String = String.Empty

        AntiForgery.GetTokens(Nothing, cookieToken, formToken)
        Return cookieToken & ":" & formToken
    End Function
End Functions

<header>
    <h1>@ViewData("Title")</h1>
</header>

<div id="main-content">
@If User.Identity.IsAuthenticated Then
    @<p class="error" data-bind="text: error"></p>
    @<input id="antiForgeryToken" type="hidden" value="@GetAntiForgeryToken()" />
    @<button data-bind="click: addTodoList">Add Todo list</button>

    @<section id="lists" data-bind="foreach: todoLists, visible: todoLists().length > 0">
        <article class="todoList">
            <header>
                <form data-bind="validate: true">
                    <input class="required" data-bind="value: title, selected: isEditingListTitle, blurOnEnter: true" />
                </form>
            </header>

            <a href="#" class="deletelist" data-bind="click: $parent.deleteTodoList">X</a>

            <ul data-bind="foreach: todos">
                <li>
                    <input type="checkbox" data-bind="checked: isDone" />
                    <input class="todoItemInput" type="text" data-bind="value: title, disable: isDone, blurOnEnter: true" />
                    <a href="#" data-bind="click: $parent.deleteTodo">X</a>
                    <p class="error" data-bind="visible: errorMessage, text: errorMessage"></p>
                </li>
            </ul>

            <form data-bind="submit: addTodo">
                <input class="addTodo" type="text" data-bind="value: newTodoTitle, placeholder: 'Type here to add', blurOnEnter: true, event: { blur: addTodo }" />
            </form>

            <p class="error" data-bind="visible: errorMessage, text: errorMessage"></p>
        </article>
    </section>
Else
    @<div class="todoList" id="loginPanel">
        <section id="localLoginPanel">
            <h2>Log in</h2>
            @Html.Partial("_Login")
        </section>
        <section id="socialLoginPanel">
            <h2>Log in using another service</h2>
            @Html.Action("ExternalLoginsList", "Account", New With { .ReturnUrl = ViewData("ReturnUrl") })
        </section>
        <p>First time here? <a id="showRegister">Sign up</a></p>
    </div>
    @<div class="todoList" id="registerPanel">
        <h2>Sign up</h2>
        @Html.Partial("_Register")
        <p>Already signed up? <a id="showLogin">Log in</a></p>
    </div>
End If
</div>

@If User.Identity.IsAuthenticated Then
    @Section scripts 
        @Scripts.Render("~/bundles/knockout")
        @Scripts.Render("~/bundles/jqueryval")
        @Scripts.Render("~/bundles/todo")
    End Section
End If