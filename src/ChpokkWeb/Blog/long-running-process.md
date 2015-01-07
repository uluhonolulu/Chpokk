Title: Providing user feedback for long running tasks in Asp.Net
Date: 2015-01-06
Categories: CodeProject
==
There are many cases when you want to start a long operation and watch its progress. 
In my [Chpokk project](http://chpokk.apphb.com/) (a C# and VB.Net IDE), you can see it when you create a project (primarily when adding NuGet packages, which can be long), compiling, executing, and automated testing. 
I also plan to add it to source control related operations, like Git cloning or pushing. The problem is, we need to push the progress notifications from the server to the client, which is the opposite to what we are doing most of the time. 

Fortunately, we already have a library that has been designed for this purpose. It's called SignalR.
--

### Let's use SignalR to make our life easier.
In most tutorials, SignalR is used to call the clients in response to some client's call. 
A typical example is a chat application where a client sends a message, and the server broadcasts it to all clients.
Such requests are handled by a Hub class, which lives in a universe somewhat parallel to the rest of our application.

Our case is different. We want a long running process to send notifications back to the client that started it. 
The SignalR machinery should react to these notifications rather than to a call from a client.
We can't handle these notifications by a Hub class, which is designed to handle client requests directly.

Fortunately, there's also the HubContext class, which is more suitable in our situation. The client issues a standard call (e.g. an AJAX call in response to a button click), 
we handle it by an Endpoint/Controller/whatever our main Web framework uses, and use the HubContext class as a portal to the SignalR universe. 
All it needs to know is the connectionId value so that it can send messages to the right user.
Our endpoint retrieves this value, along with the other data, from the request, passes it to a long running process which starts on a new thread, and returns 200 OK to the client.
The process keeps running and sending notification to the client via SignalR using the code that I'm going to show you just a few lines below.

### Let's see the code.
To send a message to the client, we still need a Hub class, but it can be empty. We also need the ConnectionId value in order to send the message to the right client. The message itself can be sent with this code:

```csharp
GlobalHost.ConnectionManager.GetHubContext<LogHub>()
                                  .Clients.Client(connectionId)
                                  .log(message);
```

Here "log(message)" is a method defined on the client (more on this later).

In order to make this work, we have to tell the server the value of connectionId. We can capture it at any time after the connection is established:

```javascript
var connectionId = $.connection.hub.id;
```

Later we send it to the server along with the data required for our long running process:
```javascript
$.post(url, {ConnectionId: connectionId, ...});
```
The `$.post` request starts the long running process on a separate thread and returns immediately.

Now that the server sends us messages, we need to handle them somehow (e.g. display them). Remember the `log` method we're calling on the server? We need to implement it on the client, which is standard for SignalR applications:

```javascript
$.connection.logHub.client.log = function(message){...};
```
We're done, let's celebrate!

While the implementation turned out to be very simple, I'm still happy that I forced myself to write this post. I think logging a long running process is quite a common task for the apps that do something more interesting than CRUD, and having a post like this for a reminder is quite handy.

As always, you can get the [sample code](https://github.com/uluhonolulu/BlogSamples/tree/master/LongRunningProcess) (a fully functional application) on GitHub. The actual code is a little bit different from what I write here, since I wanted it to be a bit structured. But the general idea is the same. 

Also, please check it in action in my [online .Net code editor](http://chpokk.apphb.com/) -- tell me what you think!