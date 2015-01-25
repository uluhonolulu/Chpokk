Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Linq

Public Class TodoItemDto
    ''' <summary>
    ''' Data transfer object for <see cref="TodoItem"/>
    ''' </summary>
    Public Sub New()
    
    End Sub

    Public Sub New(item As TodoItem)
        TodoItemId = item.TodoItemId
        Title = item.Title
        IsDone = item.IsDone
        TodoListId = item.TodoListId
    End Sub

    <Key()>
    Public Property TodoItemId As Integer

    <Required()>
    Public Property Title As String

    Public Property IsDone As Boolean

    Public Property TodoListId As Integer

    Public Function ToEntity() As TodoItem 
        Return New TodoItem With
        {
            .TodoItemId = TodoItemId,
            .Title = Title,
            .IsDone = IsDone,
            .TodoListId = TodoListId
        }
    End Function
End Class
