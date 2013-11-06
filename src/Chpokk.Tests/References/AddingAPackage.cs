using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
using Microsoft.Build.Construction;
using Mono.Collections.Generic;
using NuGet.Common;
using Shouldly;
using Console = System.Console;

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
			//keep packages from the search session
			var cache = Context.Container.Get<PackageInfoCache>();
			var id = "elmah";
			var packages = Context.Container.Get<PackageFinder>().FindPackages(id);
			cache.Keep(packages);

			var installer = Context.Container.Get<PackageInstaller>();
			installer.InstallPackage(id, TargetFolder, Context.ProjectPath);


		}

		private string TargetFolder {
			get { return Context.SolutionFolder.AppendPath("packages"); }
		}
	}
}
