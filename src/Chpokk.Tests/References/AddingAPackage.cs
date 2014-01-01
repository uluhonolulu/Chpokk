using System;
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
			var project = ProjectRootElement.Open(Context.ProjectPath);
			var contentFiles = project.Items.Where(element => element.ItemType == "Content").Select(element => element.Include);
			contentFiles.ShouldContain(@"App_Readme\Elmah.txt");
		}

		[Test]
		public void AddsContentFiles() {
			var contentFilePath = Context.ProjectPath.ParentDirectory().AppendPath(@"App_Readme\Elmah.txt");
			File.Exists(contentFilePath).ShouldBe(true);
		}

		[Test]
		public void AddsAssemblyReference() {
			var project = ProjectRootElement.Open(Context.ProjectPath);
			var projectReferences = project.Items.Where(element => element.ItemType == "Reference");
			projectReferences.First().Include.ShouldBe("elmah", Case.Insensitive);
		}

		[Test]
		public void AddsReferencedFiles() {
			Directory.EnumerateFiles(TargetFolder, "Elmah.dll", SearchOption.AllDirectories).Any().ShouldBe(true);
		}


		public override void Act() {
			const string packageId = "elmah";
			var projectPath = Context.ProjectPath;
			var packageInstaller = Context.Container.Get<PackageInstaller>();
			packageInstaller.InstallPackage(packageId, projectPath);

		}



		private string TargetFolder {
			get { return Context.SolutionFolder.AppendPath("packages"); }
		}
	}
}
