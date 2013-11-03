using System.Linq;

namespace ChpokkWeb.Features.ProjectManagement.References.Bcl {
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