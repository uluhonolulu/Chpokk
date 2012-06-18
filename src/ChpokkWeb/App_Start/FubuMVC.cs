using System.Web.Routing;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using StructureMap;

// You can remove the reference to WebActivator by calling the Start() method from your Global.asax Application_Start
//[assembly: WebActivator.PreApplicationStartMethod(typeof(ChpokkWeb.App_Start.AppStartFubuMVC), "Start")]

namespace ChpokkWeb.App_Start
{
    public static class AppStartFubuMVC
    {
        public static void Start() {
        	var container = new Container();
        	container.Configure(expression => expression.AddRegistry<ChpokkRegistry>());
        	FubuApplication.For<ConfigureFubuMVC>() 
                .StructureMap(container)
                .Bootstrap();
        }
    }
}