using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using FubuCore;
using MbUnit.Framework;
using NuGet;
using Shouldly;

namespace Chpokk.Tests.References.LocalNuget {
	[TestFixture]
	public class InstallingPackageFromLocalFolder : BaseCommandTest<ProjectFileContext> {
		private const string PackageId = "EntityFramework";
		private const string Version = "6.1.1";

		[Test]
		public void PackagesFolderShouldContainAnAssembly() {
			var packageFolder = Context.ProjectPath.ParentDirectory().ParentDirectory().AppendPath("packages", string.Concat(PackageId, ".", Version));
			var assemblies = Directory.EnumerateFileSystemEntries(packageFolder, "*.dll", SearchOption.AllDirectories);
			assemblies.ShouldNotBeEmpty();
		}

		public override void Act() {
			var path = @"C:\Program Files (x86)\Microsoft ASP.NET\ASP.NET Web Stack 5\Packages\";
			var projectPath = Context.ProjectPath;
			var packageInstaller = Context.Container.Get<PackageInstaller>();
			packageInstaller.CopyPackageFromLocalFolder(path, PackageId, Version, projectPath);
			ProjectParser.UnloadProject(projectPath); //so that it is not cached in the global project collection -- might keep references
			var targetFolder = projectPath.ParentDirectory().ParentDirectory().AppendPath("packages");
			//var packagePathResolver = new DefaultPackagePathResolver(targetFolder);
			//var packagesFolderFileSystem = new PhysicalFileSystem(targetFolder);
			//var projectSystem = new BetterThanMSBuildProjectSystem(projectPath);
			//var localRepository = new BetterThanLocalPackageRepository(packagePathResolver, packagesFolderFileSystem, projectSystem);
			var packageRepository = PackageRepositoryFactory.Default.CreateRepository(path);
			//var packages = packageRepository.GetPackages().ToArray();
			//var projectManager = new ProjectManager(packageRepository, packagePathResolver, projectSystem,
			//	localRepository);
			//var packageManager = new PackageManager(packageRepository, targetFolder);
			//packageManager.InstallPackage(PackageId, SemanticVersion.Parse(Version));
			//packageManager is enough

			//projectManager.AddPackageReference(packageId, SemanticVersion.Parse("1.10.2"));
			//projectSystem.Save();
		}
	}
}
