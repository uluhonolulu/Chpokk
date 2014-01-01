using System.IO;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using Microsoft.Build.Construction;
using NuGet;
using NuGet.Commands;
using NuGet.Common;
using System.Collections.Generic;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class PackageInstaller {
		private readonly IPackageRepository _packageRepository;
		private readonly NuGetInitializer _initializer;
		private readonly IConsole _console;
		public PackageInstaller(IPackageRepository packageRepository, NuGetInitializer initializer, IConsole console) {
			_packageRepository = packageRepository;
			_initializer = initializer;
			_console = console;
		}


		public void InstallPackage(string packageId, string projectPath, string targetFolder = null) {
			const string packagesFolder = "packages";
			if (targetFolder == null) 
				targetFolder = projectPath.ParentDirectory().ParentDirectory().AppendPath(packagesFolder);
			var packagePathResolver = new DefaultPackagePathResolver(targetFolder);
			var packagesFolderFileSystem = new PhysicalFileSystem(targetFolder);
			var localRepository = new LocalPackageRepository(packagePathResolver, packagesFolderFileSystem);
			var projectSystem = new MSBuildProjectSystem(projectPath){Logger = _console};
			var projectManager = new ProjectManager(_packageRepository, packagePathResolver, projectSystem,
													localRepository);

			projectManager.Logger = _console;
			projectManager.PackageReferenceAdded += (sender, args) =>
			{
				args.Package.GetLibFiles()
				    .Each(file =>
				    {
					    var targetPath = args.InstallPath.AppendPath(file.Path);
					    var relativePath = targetPath.PathRelativeTo(args.FileSystem.Root);
						projectSystem.AddFile(relativePath, file.GetStream());
				    });
				args.Package.GetContentFiles().Each(file =>
				{
					var targetPath = args.InstallPath.AppendPath(file.Path);
					var relativePath = targetPath.PathRelativeTo(args.FileSystem.Root);
					var rootElement = ProjectRootElement.Open(projectPath);
					rootElement.AddItem("Content", file.EffectivePath);
					rootElement.Save();
				});
			};
			projectManager.AddPackageReference(packageId);
			projectSystem.Save();


			//var command = _initializer.CreateObject<InstallCommand>();
			//command.OutputDirectory = targetFolder;
			//command.Source.Add(NuGetConstants.DefaultFeedUrl);
			//command.Arguments.Add(packageId);
			var repository = new CommandLineRepositoryFactory(_console).CreateRepository(NuGetConstants.DefaultFeedUrl);
			var packageManager = new PackageManager(repository, packagePathResolver, packagesFolderFileSystem, localRepository){}; // ILogger
			//using (packageManager.SourceRepository.StartOperation(
			//   RepositoryOperationNames.Install,
			//   packageId,
			//   null)) {
			//	packageManager.InstallPackage(packageId, null);
			//}
			//command.PackageSaveMode = PackageSaveModes.Nupkg.ToString() + ";" + PackageSaveModes.Nuspec.ToString();
			//command.ExecuteCommand();
			
			//command + project: no files
			//command: no reference, no content, no cotent files
	
		}
	}

	public class BetterThanMSBuildProjectSystem : MSBuildProjectSystem {

		public override void AddFile(string path, System.IO.Stream stream) {
			base.AddFile(path, stream);
			//var rootElement = ProjectRootElement.Open(ProjectPath);
			//rootElement.AddItem("Content", path);
		}

		public string ProjectPath { get; private set; }

		public BetterThanMSBuildProjectSystem(string projectFile) : base(projectFile) {
			ProjectPath = projectFile;
		}
	}
}