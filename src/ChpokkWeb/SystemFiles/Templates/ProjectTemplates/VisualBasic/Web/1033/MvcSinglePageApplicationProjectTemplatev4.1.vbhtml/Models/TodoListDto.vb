Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Web
Imports Newtonsoft.Json

''' <summary>
''' Data transfer object for <see cref="TodoList"/>
''' </summary>
Public Class TodoListDto
    Public Sub New()
    
    End Sub

    Public Sub New (todoList As TodoList)
        TodoListId = todoList.TodoListId
        UserId = todoList.UserId
        Title = todoList.Title
        Todos = new List(Of TodoItemDto)()
        For Each item As TodoItem in todoList.Todos
            Todos.Add(New TodoItemDto(item))
        Next
    End Sub
    
    <Key()>
    Public Property TodoListId As Integer

    <Required()>
    Public Property UserId As String

    <Required()>
    Public Property Title As String

    Public Overridable Property Todos As List(Of TodoItemDto)

    Public Function ToEntity() As TodoList
        Dim todo As New TodoList With 
        {
            .Title = Title,
            .TodoListId = TodoListId,
            .UserId = UserId,
            .Todos = New List(Of TodoItem)()
        }
        
        For Each item As TodoItemDto in Todos
            todo.Todos.Add(item.ToEntity())
        Next

        Return todo
    End Function
End Class