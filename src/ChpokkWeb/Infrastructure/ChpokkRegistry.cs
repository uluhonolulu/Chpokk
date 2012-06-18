using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace ChpokkWeb.Infrastructure {
	public class ChpokkRegistry : Registry {
		public ChpokkRegistry() {
			For<RepositoryManager>().LifecycleIs(new HybridSessionLifecycle());
			For<HttpContext>().Use(() => HttpContext.Current);
		}
	}
}