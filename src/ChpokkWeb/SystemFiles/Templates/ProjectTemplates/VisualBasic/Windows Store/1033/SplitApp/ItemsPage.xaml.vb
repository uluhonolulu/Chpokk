' The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

''' <summary>
''' A page that displays a collection of item previews.  In the Split Application this page
''' is used to display and select one of the available groups.
''' </summary>
Public NotInheritable Class ItemsPage
    Inherits Common.LayoutAwarePage

    ''' <summary>
    ''' Populates the page with content passed during navigation.  Any saved state is also
    ''' provided when recreating a page from a prior session.
    ''' </summary>
    ''' <param name="navigationParameter">The parameter value passed to <see cref="Frame.Navigate"/>
    ''' when this page was initially requested.
    ''' </param>
    ''' <param name="pageState">A dictionary of state preserved by this page during an earlier
    ''' session.  This will be null the first time a page is visited.</param>
    Protected Overrides Sub LoadState(navigationParameter As Object, pageState As Dictionary(Of String, Object))

        ' TODO: Create an appropriate data model for your problem domain to replace the sample data
        Dim sampleDataGroups As IEnumerable(Of Data.SampleDataGroup) = Data.SampleDataSource.GetGroups(DirectCast(navigationParameter, String))
        Me.DefaultViewModel("Items") = sampleDataGroups
    End Sub

    ''' <summary>
    ''' Invoked when an item is clicked.
    ''' </summary>
    ''' <param name="sender">The GridView (or ListView when the application is snapped)
    ''' displaying the item clicked.</param>
    ''' <param name="e">Event data that describes the item clicked.</param>
    Private Sub ItemView_ItemClick(sender As Object, e As ItemClickEventArgs)

        ' Navigate to the appropriate destination page, configuring the new page
        ' by passing required information as a navigation parameter
        Dim groupId As String = DirectCast(e.ClickedItem, Data.SampleDataGroup).UniqueId
        Me.Frame.Navigate(GetType(SplitPage), groupId)
    End Sub

End Class
