using ChpokkWeb.Features.ProjectManagement.References;
using ChpokkWeb.Features.ProjectManagement.References.Bcl;
using MbUnit.Framework;
using Shouldly;

namespace Chpokk.Tests.References {
	public class StandardLibraryList {

		[Test]
		public void CanRetrieveDisplaysBclAssemblies() {
			var assemblies = new BclAssembliesProvider().BclAssemblies;

			assemblies.ShouldContain("System.Data");
			//also use AdditionalExplicitAssemblyReferences to add references and maybe TargetFrameworkDirectory
		}
	}
}
