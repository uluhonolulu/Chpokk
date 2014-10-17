@Imports Microsoft.AspNet.Mvc.Facebook.Models
@ModelType $safeprojectname$.MyAppUser
@Code
    ViewData("Title") = "Home Page"
End Code

<article class="intro">
    <span id="profilePicture">
        @if Not (Model.ProfilePicture Is Nothing) AndAlso Not (Model.ProfilePicture.Data Is Nothing) Then
            @<img src="@Model.ProfilePicture.Data.Url" />
        End If
    </span>
    <h3>Welcome @Model.Name</h3>
    <label>Email: @Model.Email</label>
</article>

<article id="content">
    <div class="left">
        <h4>Friends</h4>
        @If Not (Model.Friends Is Nothing) AndAlso Not (Model.Friends.Data Is Nothing) AndAlso Model.Friends.Data.Count > 0 Then
            @For Each myFriend In Model.Friends.Data
              @<a href="@myFriend.Link" target="_blank">
                  <div class="photoTile">
                      <label>@myFriend.Name</label>
                      @if Not (myFriend.Picture Is Nothing) AndAlso Not (myFriend.Picture.Data Is Nothing) Then
                          @<img src="@myFriend.Picture.Data.Url" />
                      End If
                  </div>
              </a>
            Next
        Else
            @<p>No friends found.</p>
        End If
    </div>
    <div class="right">
        <h4>Photos</h4>
        @If Not (Model.Photos Is Nothing) AndAlso Not (Model.Photos.Data Is Nothing) AndAlso Model.Photos.Data.Count > 0 Then
            @For Each photo In Model.Photos.Data
              @<a href="@photo.Link" target="_blank">
                <div class="photoTile">
                    <img src="@photo.ThumbnailUrl" />
                </div>
              </a>
            Next
        Else
            @<p>No photo available.</p>
        End If
    </div>
</article>