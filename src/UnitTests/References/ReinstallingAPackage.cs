using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Construction;
using System.Linq;
using Microsoft.Build.Evaluation;
using Shouldly;

namespace Chpokk.Tests.References {
	[TestFixture]
	public class ReinstallingAPackage : BaseCommandTest<ProjectWithInstalledPackageAndRemovedReferenceContext> {
		[Test]
		public void ReferenceShouldBeThereAgain() {
			var project = ProjectRootElement.Open(Context.ProjectPath);
			GetFirstReference(project).ShouldBe(Context.PackageAssemblyName);
		}

		private string GetFirstReference(ProjectRootElement project) {
			var references = project.Items.Where(element => element.ItemType == "Reference");
			if (!references.Any()) {
				Assert.Fail("No references in the project");
			}
			return references.First().Include;
		}

		public override void Act() {
			var packageInstaller = Context.Container.Get<PackageInstaller>();
			packageInstaller.InstallPackage(Context.PackageName, Context.ProjectPath);

		}

	}
	public class ProjectWithPackageContext: ProjectFileContext {
		public string PackageName = "NUnit";
		public string PackageAssemblyName = "nunit.framework";
		public override void Create() {
			base.Create();
			var packageInstaller = this.Container.Get<PackageInstaller>();
			packageInstaller.InstallPackage(PackageName, this.ProjectPath);
		}
	}

	public class ProjectWithInstalledPackageAndRemovedReferenceContext: ProjectWithPackageContext {
		public override void Create() {
			base.Create();
			//remove the reference manually
			var project = ProjectRootElement.Open(this.ProjectPath);
			var reference =
				project.Items.First(element => element.ItemType == "Reference" && element.Include == PackageAssemblyName);
			reference.Parent.RemoveChild(reference);
			project.Save();
		}
	}
}
