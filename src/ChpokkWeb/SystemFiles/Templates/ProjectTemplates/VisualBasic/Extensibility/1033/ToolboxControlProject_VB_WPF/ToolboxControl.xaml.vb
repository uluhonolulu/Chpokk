<ProvideToolboxControl("$projectname$", true)>
Class ToolboxControl
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        MessageBox.Show(String.Format(System.Globalization.CultureInfo.CurrentUICulture, "$message$", Me.ToString()))
    End Sub
End Class
