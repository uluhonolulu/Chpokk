Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http

<Authorize()>
Public Class TodoListController
    Inherits ApiController

    Private db As New TodoItemContext()

    ' GET api/TodoList
    Public Function GetTodoLists() As IEnumerable(Of TodoListDto)
        Return db.TodoLists.Include("Todos") _
            .Where(Function(u) u.UserId = User.Identity.Name) _
            .OrderByDescending(Function(u) u.TodoListId) _
            .AsEnumerable() _
            .Select(Function(todoList) New TodoListDto(todoList))
    End Function

    ' GET api/TodoList/5
    Public Function GetTodoList(id As Integer) As TodoListDto
        Dim todoList As TodoList = db.TodoLists.Find(id)
        If todoList Is Nothing Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If

        If Not todoList.UserId = User.Identity.Name Then
            ' Trying to modify a record that does not belong to the user
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized))
        End If

        Return New TodoListDto(todoList)
    End Function

    ' PUT api/TodoList/5
    <ValidateHttpAntiForgeryToken()>
    Public Function PutTodoList(id As Integer, todoListDto As TodoListDto) As HttpResponseMessage
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = todoListDto.TodoListId Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        Dim todoList As TodoList = todoListDto.ToEntity()
        If Not db.Entry(todoList).Entity.UserId = User.Identity.Name Then
            ' Trying to modify a record that does not belong to the user
            Return Request.CreateResponse(HttpStatusCode.Unauthorized)
        End If

        db.Entry(todoList).State = EntityState.Modified

        Try
            db.SaveChanges()
        Catch e As DbUpdateConcurrencyException
            Return Request.CreateResponse(HttpStatusCode.InternalServerError)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK)
    End Function

    ' POST api/TodoList
    <ValidateHttpAntiForgeryToken()>
    Public Function PostTodoList(todoListDto As TodoListDto) As HttpResponseMessage 
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        todoListDto.UserId = User.Identity.Name
        Dim todoList As TodoList = todoListDto.ToEntity()
        db.TodoLists.Add(todoList)
        db.SaveChanges()
        todoListDto.TodoListId = todoList.TodoListId

        Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, todoListDto)
        response.Headers.Location = New Uri(Url.Link("DefaultApi", New With { .id = todoListDto.TodoListId }))
        Return response
    End Function

    ' DELETE api/TodoList/5
    <ValidateHttpAntiForgeryToken()>
    public Function DeleteTodoList(id As Integer) As HttpResponseMessage 
        Dim todoList As TodoList = db.TodoLists.Find(id)
        If todoList Is Nothing Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        If Not db.Entry(todoList).Entity.UserId = User.Identity.Name
            ' Trying to delete a record that does not belong to the user
            Return Request.CreateResponse(HttpStatusCode.Unauthorized)
        End If

        Dim todoListDto As TodoListDto = New TodoListDto(todoList)
        db.TodoLists.Remove(todoList)

        Try
            db.SaveChanges()
        Catch e As DbUpdateConcurrencyException
            Return Request.CreateResponse(HttpStatusCode.InternalServerError)
        End Try

        return Request.CreateResponse(HttpStatusCode.OK, todoListDto)
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class
