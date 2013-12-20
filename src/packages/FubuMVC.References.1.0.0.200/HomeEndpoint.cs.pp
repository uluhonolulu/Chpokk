using System.Web.Routing;
using Bottles;
using FubuMVC.Core;
using FubuMVC.StructureMap;

namespace $rootnamespace$
{
	public class HomeModel
	{
		public string Message { get; set; }
	}
	
	// Fubu's default policies look for classes suffixed with "Endpoint" or "Endpoints"
    public class HomeEndpoint
	{
		// Fubu will use HomeEndpoint.Index as the default "home" route
		public HomeModel Index()
		{
			return new HomeModel { Message = "Hello, World!" };
		}
	}
}