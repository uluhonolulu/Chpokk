using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.ProjectManagement.References {
	public class BclReferencesEndpoint {
		private readonly BclAssembliesProvider _provider;
		public BclReferencesEndpoint(BclAssembliesProvider provider) {
			_provider = provider;
		}

		public BclReferencesModel DoIt() {
			return new BclReferencesModel{Assemblies = _provider.BclAssemblies.ToArray()};;
		}
	}
}