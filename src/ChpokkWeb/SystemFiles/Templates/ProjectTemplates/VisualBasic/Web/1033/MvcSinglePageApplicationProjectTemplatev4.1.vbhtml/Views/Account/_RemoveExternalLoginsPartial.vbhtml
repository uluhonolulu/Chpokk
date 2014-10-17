@ModelType ICollection(Of $safeprojectname$.ExternalLogin)

@If Model.Count > 0 Then
    @<h3>Registered external logins</h3>
    @<table>
        <tbody>
        @For Each externalLogin As $safeprojectname$.ExternalLogin In Model
            @<tr>
                <td>@externalLogin.ProviderDisplayName</td>
                <td>
                    @If ViewData("ShowRemoveButton") Then
                            Using Html.BeginForm("Disassociate", "Account")
                            @Html.AntiForgeryToken()
                            @<div>
                                @Html.Hidden("provider", externalLogin.Provider)
                                @Html.Hidden("providerUserId", externalLogin.ProviderUserId)
                                <input type="submit" value="Remove" title="Remove this @externalLogin.ProviderDisplayName credential from your account" />
                            </div>
                        End Using
                    Else
                        @: &nbsp;
                    End If
                </td>
            </tr>
        Next
        </tbody>
    </table>
End If