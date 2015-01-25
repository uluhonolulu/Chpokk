Imports System
Imports System.Data
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http

<Authorize()> _
<ValidateHttpAntiForgeryToken()>
Public Class TodoController
    Inherits ApiController
    Private db As New TodoItemContext()

    ' PUT api/Todo/5
    Public Function PutTodoItem(id As Integer, todoItemDto As TodoItemDto) As HttpResponseMessage 
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = todoItemDto.TodoItemId Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        Dim todoItem As TodoItem = todoItemDto.ToEntity()
        Dim todoList As TodoList = db.TodoLists.Find(todoItem.TodoListId)
        If todoList Is Nothing Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        If Not todoList.UserId = User.Identity.Name Then
            ' Trying to modify a record that does not belong to the user
            Return Request.CreateResponse(HttpStatusCode.Unauthorized)
        End If

        ' Need to detach to avoid duplicate primary key exception when SaveChanges is called
        db.Entry(todoList).State = EntityState.Detached
        db.Entry(todoItem).State = EntityState.Modified

        Try
            db.SaveChanges()
        Catch e As DbUpdateConcurrencyException
            Return Request.CreateResponse(HttpStatusCode.InternalServerError)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK)
    End Function

    ' POST api/Todo
    Public Function PostTodoItem(todoItemDto As TodoItemDto) As HttpResponseMessage 
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        Dim todoList As TodoList = db.TodoLists.Find(todoItemDto.TodoListId)
        If todoList Is Nothing Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        If Not todoList.UserId = User.Identity.Name Then
            ' Trying to add a record that does not belong to the user
            Return Request.CreateResponse(HttpStatusCode.Unauthorized)
        End If

        Dim todoItem As TodoItem = todoItemDto.ToEntity()

        ' Need to detach to avoid loop reference exception during JSON serialization
        db.Entry(todoList).State = EntityState.Detached
        db.TodoItems.Add(todoItem)
        db.SaveChanges()
        todoItemDto.TodoItemId = todoItem.TodoItemId

        Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, todoItemDto)
        response.Headers.Location = New Uri(Url.Link("DefaultApi", New With { .id = todoItemDto.TodoItemId }))
        Return response
    End Function

    ' DELETE api/Todo/5
    Public Function DeleteTodoItem(id As Integer) As HttpResponseMessage 
        Dim todoItem As TodoItem = db.TodoItems.Find(id)
        If todoItem Is Nothing Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        If Not db.Entry(todoItem.TodoList).Entity.UserId = User.Identity.Name Then
            ' Trying to delete a record that does not belong to the user
            Return Request.CreateResponse(HttpStatusCode.Unauthorized)
        End If

        Dim todoItemDto As New TodoItemDto(todoItem)
        db.TodoItems.Remove(todoItem)

        Try
            db.SaveChanges()
        Catch e As DbUpdateConcurrencyException
            return Request.CreateResponse(HttpStatusCode.InternalServerError)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, todoItemDto)
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class