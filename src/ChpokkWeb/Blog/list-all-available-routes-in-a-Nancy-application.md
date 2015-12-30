Title: How to list all available routes in a Nancy application
Date: 2015-12-28
==
If you want to list all available routes in your Nancy Web application, here's a simple way for doing this in a unit test.
--
```csharp
var bootstrapper = new Flyent.Bootstrapper();
bootstrapper.Initialise();
var appContainer = bootstrapper.GetAppContainer();
var routeCacheProvider = appContainer.Resolve<IRouteCacheProvider>();
var routeCache = routeCacheProvider.GetCache();
foreach (var pair in routeCache) {
	Console.WriteLine("====== " + pair.Key.Name + " ======");
	foreach (var tuple in pair.Value) {
		var order = tuple.Item1;
		var routeDescription = tuple.Item2;
		Console.WriteLine("{0}. {1}: {2} {3}", order + 1, routeDescription.Name, routeDescription.Method, routeDescription.Path);
	}
}
```

You have to add this to your bootstrapper class:
```csharp
public TinyIoCContainer GetAppContainer() {
	return this.ApplicationContainer;
}
```