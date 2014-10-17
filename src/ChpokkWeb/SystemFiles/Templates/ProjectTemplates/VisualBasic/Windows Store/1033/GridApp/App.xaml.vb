' The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

''' <summary>
''' Provides application-specific behavior to supplement the default Application class.
''' </summary>
NotInheritable Class App
    Inherits Application

    ''' <summary>
    ''' Invoked when the application is launched normally by the end user.  Other entry
    ''' points will be used when the application is launched to open a specific file,
    ''' to display search results, and so forth.
    ''' </summary>
    ''' <param name="args">Details about the launch request and process.</param>
    Protected Overrides Async Sub OnLaunched(args As LaunchActivatedEventArgs)
        Dim rootFrame As Frame = Window.Current.Content

        ' Do not repeat app initialization when the Window already has content,
        ' just ensure that the window is active
        If rootFrame Is Nothing Then
            ' Create a Frame to act as the navigation context and associate it with
            ' a SuspensionManager key
            rootFrame = New Frame()
            Common.SuspensionManager.RegisterFrame(rootFrame, "AppFrame")
            If args.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                ' Restore the saved session state only when appropriate
                Try
                    Await Common.SuspensionManager.RestoreAsync()
                Catch ex As Common.SuspensionManagerException
                    ' Something went wrong restoring state.
                    ' Assume there is no state and continue
                End Try
            End If
            ' Place the frame in the current Window
            Window.Current.Content = rootFrame
        End If
        If rootFrame.Content Is Nothing Then
            ' When the navigation stack isn't restored navigate to the first page,
            ' configuring the new page by passing required information as a navigation
            ' parameter
            If Not rootFrame.Navigate(GetType(GroupedItemsPage), "AllGroups") Then
                Throw New Exception("Failed to create initial page")
            End If
        End If
        ' Ensure the current window is active
        Window.Current.Activate()
    End Sub

    ''' <summary>
    ''' Invoked when application execution is being suspended.  Application state is saved
    ''' without knowing whether the application will be terminated or resumed with the contents
    ''' of memory still intact.
    ''' </summary>
    ''' <param name="sender">The source of the suspend request.</param>
    ''' <param name="e">Details about the suspend request.</param>
    Private Async Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()
        Await Common.SuspensionManager.SaveAsync()
        deferral.Complete()
    End Sub

End Class
