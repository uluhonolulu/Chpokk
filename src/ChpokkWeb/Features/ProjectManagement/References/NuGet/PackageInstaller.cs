using System.IO;
using FubuCore;
using Microsoft.Build.Construction;
using NuGet;
using NuGet.Common;
using System.Collections.Generic;
using Console = System.Console;

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
			var projectSystem = new BetterThanMSBuildProjectSystem(projectPath) { Logger = _console };
			var projectManager = new ProjectManager(_packageRepository, packagePathResolver, projectSystem,
													localRepository);

			projectManager.Logger = _console;
			projectManager.PackageReferenceAdded += (sender, args) =>
			{
				args.Package.GetLibFiles()
				    .Each(file =>
				    {
						SaveAssemblyFile(targetFolder, args.InstallPath, file, packagesFolderFileSystem);
				    });
				args.Package.GetContentFiles().Each(file =>
				{
					//AddContentToProject(projectPath, file);
				});
			};
			projectManager.AddPackageReference(packageId);
			projectSystem.Save();
		}

		private void AddContentToProject(string projectPath, IPackageFile file) {
			var rootElement = ProjectRootElement.Open(projectPath);
			rootElement.AddItem("Content", file.EffectivePath);
			rootElement.Save();
		}

		private void SaveAssemblyFile(string targetFolder, string installPath, IPackageFile file,
									  PhysicalFileSystem packagesFolderFileSystem) {
			var targetPath = installPath.AppendPath(file.Path);
			Directory.CreateDirectory(targetPath.ParentDirectory());
			using (Stream outputStream = File.Create(targetPath)){
                file.GetStream().CopyTo(outputStream);
            }

			//var relativePath = targetPath.PathRelativeTo(targetFolder);
			//packagesFolderFileSystem.AddFile(relativePath, file.GetStream());
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