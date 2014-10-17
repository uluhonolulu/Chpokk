Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Web
Imports Newtonsoft.Json

''' <summary>
''' Todo list entity
''' </summary>
Public Class TodoList
    Public Property TodoListId As Integer
    
    <Required()>
    Public Property UserId As String

    <Required()>
    Public Property Title As String

    Public Overridable Property Todos As List(Of TodoItem)
End Class
