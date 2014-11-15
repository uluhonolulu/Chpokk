using System.IO;
using System.Linq;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using MbUnit.Framework;
using FubuCore;
using Microsoft.Build.Construction;
using Shouldly;

namespace UnitTests.References {
	[TestFixture]
	public class AddingAPackage : BaseCommandTest<ProjectFileContext> {
		[Test]
		public void CreatesThePackageFolder() {
			Directory.EnumerateDirectories(TargetFolder).ShouldContain(s => s.PathRelativeTo(TargetFolder).StartsWith("FubuMVC."));
		}

		[Test]
		public void AddsPackageContent() {
			var project = ProjectRootElement.Open(Context.ProjectPath);
			var contentFiles = project.Items.Where(element => element.ItemType == "Content").Select(element => element.Include);
			contentFiles.ShouldContain(@"ConfigureFubuMVC.cs");
		}

		[Test]
		public void AddsContentFiles() {
			var contentFiles = Directory.EnumerateFiles(Context.ProjectPath.ParentDirectory(), "*.*", SearchOption.AllDirectories).Select(path => path.PathRelativeTo(Context.ProjectPath.ParentDirectory()));
			contentFiles.ShouldContain("ConfigureFubuMVC.cs");
		}

		[Test]
		public void AddsAssemblyReference() {
			var project = ProjectRootElement.Open(Context.ProjectPath);
			var projectReferences = project.Items.Where(element => element.ItemType == "Reference").Select(element => element.Include);
			projectReferences.ShouldContain("FubuMVC.Core");
		}

		[Test]
		public void AddsReferencedFiles() {
			var assemblyFiles = Directory.EnumerateFiles(TargetFolder, "*.dll", SearchOption.AllDirectories);
			assemblyFiles.ShouldContain(path => path.EndsWith("FubuMVC.Core.dll"));
		}


		public override void Act() {
			const string packageId = "FubuMvc";
			var projectPath = Context.ProjectPath;
			var packageInstaller = Context.Container.Get<PackageInstaller>();
			packageInstaller.InstallPackage(packageId, projectPath);

		}



		private string TargetFolder {
			get { return Context.SolutionFolder.AppendPath("packages"); }
		}
	}
}
