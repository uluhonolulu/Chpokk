﻿using System.IO;
using FubuCore;
using Microsoft.Build.Construction;
using NuGet;
using NuGet.Common;
using System.Collections.Generic;
using Console = System.Console;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class PackageInstaller {
		private const string PackagesFolder = "packages";
		private readonly IPackageRepository _packageRepository;
		private readonly IConsole _console;
		public PackageInstaller(IPackageRepository packageRepository, IConsole console) {
			_packageRepository = packageRepository;
			_console = console;
		}


		public void InstallPackage(string packageId, string projectPath, string targetFolder = null) {
			if (targetFolder == null)
				targetFolder = projectPath.ParentDirectory().ParentDirectory().AppendPath(PackagesFolder);
			var packagePathResolver = new DefaultPackagePathResolver(targetFolder);
			var packagesFolderFileSystem = new PhysicalFileSystem(targetFolder);
			var localRepository = new LocalPackageRepository(packagePathResolver, packagesFolderFileSystem);
			var projectSystem = new BetterThanMSBuildProjectSystem(projectPath) { Logger = _console };
			var projectManager = new ProjectManager(_packageRepository, packagePathResolver, projectSystem,
													localRepository) {Logger = _console};

			projectManager.PackageReferenceAdded += (sender, args) => args.Package.GetLibFiles()
			                                                              .Each(file => SaveAssemblyFile(args.InstallPath, file));
			projectManager.AddPackageReference(packageId);
			projectSystem.Save();
		}

		private void SaveAssemblyFile(string installPath, IPackageFile file) {
			var targetPath = installPath.AppendPath(file.Path);
			Directory.CreateDirectory(targetPath.ParentDirectory());
			using (Stream outputStream = File.Create(targetPath)){
                file.GetStream().CopyTo(outputStream);
            }

			//var relativePath = targetPath.PathRelativeTo(targetFolder);
			//packagesFolderFileSystem.AddFile(relativePath, file.GetStream());
		}

		public IEnumerable<IPackage> GetAllPackages(string rootFolder) { //rootFolder is repository root
			var targetFolder = rootFolder.AppendPath(PackagesFolder);
			var packagePathResolver = new DefaultPackagePathResolver(targetFolder);
			var packagesFolderFileSystem = new PhysicalFileSystem(targetFolder);
			var localRepository = new LocalPackageRepository(packagePathResolver, packagesFolderFileSystem);
			return localRepository.GetPackages();
		}

		public void ClearPackages(string repositoryPath) {
			Directory.Delete(repositoryPath.AppendPath(PackagesFolder), true);
		}
	}
	public class BetterThanMSBuildProjectSystem : MSBuildProjectSystem {

		public override void AddFile(string path, System.IO.Stream stream) {
			base.AddFile(path, stream);
			//Console.WriteLine("My good class just added " + path);
			var rootElement = ProjectRootElement.Open(ProjectPath);
			rootElement.AddItem("Content", path);
		}

		public string ProjectPath { get; private set; }

		public BetterThanMSBuildProjectSystem(string projectFile)
			: base(projectFile) {
			ProjectPath = projectFile;
		}
	}

}