using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.Properties;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using NuGet;
using NuGet.Common;
using Console = System.Console;
using Shouldly;

namespace Chpokk.Tests.ProjectLoading {
	[TestFixture]
	public class PackageSpike: BaseQueryTest<SimpleConfiguredContext, IEnumerable<dynamic>> {
		[Test]
		public void ReferencedPackageShouldBeDisplayed() {
			((object) FindPackage("NUnit")).ShouldNotBe(null);
		}

		[Test, DependsOn("ReferencedPackageShouldBeDisplayed")]
		public void ReferencedPackageShouldBeChecked() {
			((object)FindPackage("NUnit").Selected).ShouldBe(true);
		}

		[Test]
		public void ShouldlyShouldNotBeReferenced() {
			((object) FindPackage("Shouldly").Selected).ShouldBe(false);
		}

		public dynamic FindPackage(string name) {
			return Result.FirstOrDefault(reference => reference.Name == name);
		}

		public override IEnumerable<dynamic> Act() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.MethodName.EndsWith("Files")));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is PhysicalFileSystem));
			//CThruEngine.StartListening();
			var projectPath =
				@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\Chpokk-SampleSol\src\NewVB\NewVB.vbproj"; //@"D:\Projects\Chpokk\src\ChpokkWeb\ChpokkWeb.csproj";
			const string packagesFolder = "packages";
			var repositoryRoot = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\Chpokk-SampleSol";
			var targetFolder = repositoryRoot.AppendPath(packagesFolder);
			var packagePathResolver = new DefaultPackagePathResolver(targetFolder);
			var packagesFolderFileSystem = new PhysicalFileSystem(targetFolder);
			var localRepository = new LocalPackageRepository(packagePathResolver, packagesFolderFileSystem);
			var projectSystem = new MSBuildProjectSystem(projectPath);

			foreach (var package in localRepository.GetPackages()) {
				Console.WriteLine(package);
				foreach (var assemblyReference in package.AssemblyReferences) {
					Console.WriteLine(assemblyReference + ": " + projectSystem.ReferenceExists(assemblyReference.Name));
				}
				Console.WriteLine();
			}
			var packageInstaller = Context.Container.Get<PackageInstaller>();
			return Context.Container.Get<ProjectParser>().GetPackageReferences(projectPath, packageInstaller.GetAllPackages(repositoryRoot));

		}
	}
}
