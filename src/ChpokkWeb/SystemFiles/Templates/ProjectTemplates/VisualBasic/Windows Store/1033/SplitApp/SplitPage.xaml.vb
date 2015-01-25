' The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

''' <summary>
''' A page that displays a group title, a list of items within the group, and details for
''' the currently selected item.
''' </summary>
Public NotInheritable Class SplitPage
    Inherits Page

    Public ReadOnly Property NavigationHelper As Common.NavigationHelper
        Get
            Return Me._navigationHelper
        End Get
    End Property
    Private _navigationHelper As Common.NavigationHelper

    ''' <summary>
    ''' This can be changed to a strongly typed view model.
    ''' </summary>
    Public ReadOnly Property DefaultViewModel As Common.ObservableDictionary
        Get
            Return Me._defaultViewModel
        End Get
    End Property
    Private _defaultViewModel As New Common.ObservableDictionary()

    Public Sub New()
        InitializeComponent()
        Me._navigationHelper = New Common.NavigationHelper(Me)
        AddHandler Me._navigationHelper.LoadState, AddressOf NavigationHelper_LoadState
        AddHandler Me._navigationHelper.SaveState, AddressOf NavigationHelper_SaveState
        AddHandler Me.itemListView.SelectionChanged, AddressOf ItemListView_SelectionChanged
        Me.NavigationHelper.GoBackCommand = New Common.RelayCommand(AddressOf Me.GoBack, AddressOf Me.CanGoBack)

        AddHandler Window.Current.SizeChanged, AddressOf Winow_SizeChanged
        Me.InvalidateVisualState()
    End Sub

    ''' <summary>
    ''' Populates the page with content passed during navigation.  Any saved state is also
    ''' provided when recreating a page from a prior session.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event; typically <see cref="NavigationHelper"/>
    ''' </param>
    ''' <param name="e">Event data that provides both the navigation parameter passed to
    ''' <see cref="Frame.Navigate"/> when this page was initially requested and
    ''' a dictionary of state preserved by this page during an earlier
    ''' session.  The state will be null the first time a page is visited.</param>
    Private Async Sub NavigationHelper_LoadState(sender As Object, e As Common.LoadStateEventArgs)

        ' TODO: Create an appropriate data model for your problem domain to replace the sample data
        Dim group As Data.SampleDataGroup = Await Data.SampleDataSource.GetGroupAsync(DirectCast(e.NavigationParameter, String))
        Me.DefaultViewModel("Group") = group
        Me.DefaultViewModel("Items") = group.Items

        If e.PageState Is Nothing Then
            Me.itemListView.SelectedItem = Nothing
            ' When this is a new page, select the first item automatically unless logical page
            ' navigation is being used (see the logical page navigation #region below.)
            If Not Me.UsingLogicalPageNavigation() AndAlso Me.itemsViewSource.View IsNot Nothing Then
                Me.itemsViewSource.View.MoveCurrentToFirst()
            End If
        Else

            ' Restore the previously saved state associated with this page
            If e.PageState.ContainsKey("SelectedItem") AndAlso Me.itemsViewSource.View IsNot Nothing Then
                Dim selectedItem As Data.SampleDataItem = Await Data.SampleDataSource.GetItemAsync(DirectCast(e.PageState("SelectedItem"), String))
                Me.itemsViewSource.View.MoveCurrentTo(selectedItem)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Preserves state associated with this page in case the application is suspended or the
    ''' page is discarded from the navigation cache.  Values must conform to the serialization
    ''' requirements of <see cref="Common.SuspensionManager.SessionState"/>.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event; typically <see cref="NavigationHelper"/>
    ''' </param>
    ''' <param name="e">Event data that provides an empty dictionary to be populated with 
    ''' serializable state.</param>
    Private Sub NavigationHelper_SaveState(sender As Object, e As Common.SaveStateEventArgs)
        If Me.itemsViewSource.View IsNot Nothing Then
            Dim selectedItem As Data.SampleDataItem = DirectCast(Me.itemsViewSource.View.CurrentItem, Data.SampleDataItem)
            If selectedItem IsNot Nothing Then e.PageState("SelectedItem") = selectedItem.UniqueId
        End If
    End Sub

#Region "Logical page navigation"

    ' The split page is designed so that when the Window does have enough space to show
    ' both the list and the details, only one pane will be shown at at time.
    '
    ' This is all implemented with a single physical page that can represent two logical
    ' pages.  The code below achieves this goal without making the user aware of the
    ' distinction.

    Private Const MinimumWidthForSupportingTwoPanes As Integer = 768

    ''' <summary>
    ''' Invoked to determine whether the page should act as one logical page or two.
    ''' </summary>
    ''' <returns>True when the view state in question is portrait or snapped, false
    ''' otherwise.</returns>

    Private Function UsingLogicalPageNavigation() As Boolean
        Return Window.Current.Bounds.Width < MinimumWidthForSupportingTwoPanes
    End Function

    Private Sub Winow_SizeChanged(sender As Object, e As Windows.UI.Core.WindowSizeChangedEventArgs)
        Me.InvalidateVisualState()
    End Sub

    Private Sub ItemListView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Me.UsingLogicalPageNavigation() Then
            Me.InvalidateVisualState()
        End If
    End Sub

    Private Function CanGoBack() As Boolean
        If Me.UsingLogicalPageNavigation() AndAlso Me.itemListView.SelectedItem IsNot Nothing Then
            Return True
        Else
            Return Me.NavigationHelper.CanGoBack()
        End If
    End Function
    Private Sub GoBack()
        If Me.UsingLogicalPageNavigation() AndAlso Me.itemListView.SelectedItem IsNot Nothing Then
            ' When logical page navigation is in effect and there's a selected item that
            ' item's details are currently displayed.  Clearing the selection will return to
            ' the item list.  From the user's point of view this is a logical backward
            ' navigation.
            Me.itemListView.SelectedItem = Nothing
        Else
            Me.NavigationHelper.GoBack()
        End If
    End Sub

    Private Sub InvalidateVisualState()
        Dim visualState As String = DetermineVisualState()
        VisualStateManager.GoToState(Me, visualState, False)
        Me.NavigationHelper.GoBackCommand.RaiseCanExecuteChanged()
    End Sub


    ''' <summary>
    ''' Invoked to determine the name of the visual state that corresponds to an application
    ''' view state.
    ''' </summary>
    ''' <returns>The name of the desired visual state.  This is the same as the name of the
    ''' view state except when there is a selected item in portrait and snapped views where
    ''' this additional logical page is represented by adding a suffix of _Detail.</returns>
    ''' <remarks></remarks>
    Private Function DetermineVisualState() As String
        If (Not UsingLogicalPageNavigation()) Then
            Return "PrimaryView"
        End If

        ' Update the back button's enabled state when the view state changes
        Dim logicalPageBack As Boolean = Me.UsingLogicalPageNavigation() AndAlso Me.itemListView.SelectedItem IsNot Nothing

        If logicalPageBack Then Return "SinglePane_Detail"
        Return "SinglePane"
    End Function

#End Region

#Region "NavigationHelper registration"

    ''' The methods provided in this section are simply used to allow
    ''' NavigationHelper to respond to the page's navigation methods.
    ''' 
    ''' Page specific logic should be placed in event handlers for the  
    ''' <see cref="Common.NavigationHelper.LoadState"/>
    ''' and <see cref="Common.NavigationHelper.SaveState"/>.
    ''' The navigation parameter is available in the LoadState method 
    ''' in addition to page state preserved during an earlier session.

    Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedTo(e)
    End Sub

    Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedFrom(e)
    End Sub
#End Region

End Class
