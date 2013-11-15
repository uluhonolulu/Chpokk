using System.IO;
using System.Linq;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using MbUnit.Framework;
using FubuCore;
using Microsoft.Build.Construction;
using NuGet;
using Shouldly;

namespace Chpokk.Tests.References {
	[TestFixture]
	public class AddingAPackage : BaseCommandTest<ProjectFileContext> {
		[Test]
		public void CreatesThePackageFolder() {
			Directory.EnumerateDirectories(TargetFolder).ShouldContain(s => s.PathRelativeTo(TargetFolder).StartsWith("elmah."));
		}

		[Test]
		public void AddsPackageContent() {
			
		}

		[Test]
		public void AddsAssemblyReference() {
			var project = ProjectRootElement.Open(Context.ProjectPath);
			var projectReferences = project.Items.Where(element => element.ItemType == "Reference");
			projectReferences.First().Include.ShouldBe("elmah", Case.Insensitive);
		}


		public override void Act() {
			const string PackagesFolder = "packages";
			const string packageId = "elmah";
			var repository = PackageRepositoryFactory.Default.CreateRepository(NuGetConstants.DefaultFeedUrl);
			var path = Context.SolutionFolder.AppendPath(PackagesFolder);
			var packagePathResolver = new DefaultPackagePathResolver(path);
			var localRepository = new LocalPackageRepository(packagePathResolver, new PhysicalFileSystem(path));
			var projectSystem = new NuGet.Common.MSBuildProjectSystem(Context.ProjectPath);
			var projectManager = new ProjectManager(repository, packagePathResolver, projectSystem,
			                                        localRepository);

			projectManager.AddPackageReference(packageId);
			projectSystem.Save();



		}

		private string TargetFolder {
			get { return Context.SolutionFolder.AppendPath("packages"); }
		}
	}
}
