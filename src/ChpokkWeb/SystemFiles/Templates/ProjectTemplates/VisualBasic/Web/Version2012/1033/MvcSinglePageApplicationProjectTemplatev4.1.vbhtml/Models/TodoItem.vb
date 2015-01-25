Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Linq

''' <summary>
''' Todo item entity
''' </summary>
Public Class TodoItem
    Public Property TodoItemId As Integer
    
    <Required()>
    Public Property Title As String
    Public Property IsDone As Boolean

    <ForeignKey("TodoList")>
    Public Property TodoListId As Integer
    Public Overridable Property TodoList As TodoList
End Class