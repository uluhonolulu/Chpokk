using System.IO;
using System.Linq;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using NuGet;
using NuGet.Common;
using System.Collections.Generic;
using Console = System.Console;
using IFileSystem = NuGet.IFileSystem;

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
			ProjectParser.UnloadProject(projectPath); //so that it is not cached in the global project collection -- might keep references
			if (targetFolder == null)
				targetFolder = projectPath.ParentDirectory().ParentDirectory().AppendPath(PackagesFolder);
			var packagePathResolver = new DefaultPackagePathResolver(targetFolder);
			var packagesFolderFileSystem = new PhysicalFileSystem(targetFolder);
			var projectSystem = new BetterThanMSBuildProjectSystem(projectPath) { Logger = _console };
			var localRepository = new BetterThanLocalPackageRepository(packagePathResolver, packagesFolderFileSystem, projectSystem);
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

	public class BetterThanLocalPackageRepository: LocalPackageRepository {
		private readonly MSBuildProjectSystem _projectSystem;

		public BetterThanLocalPackageRepository(IPackagePathResolver pathResolver, IFileSystem fileSystem, MSBuildProjectSystem projectSystem) : base(pathResolver, fileSystem) {
			_projectSystem = projectSystem;
		}


		public override bool Exists(string packageId, SemanticVersion version) {
			//if no package file exists, return false
			if (!base.Exists(packageId, version))
				return false;
			//find the package and check whether all its assemblies are referenced
			var package = this.FindPackage(packageId, version);
			foreach (var reference in package.AssemblyReferences) {
				Console.WriteLine(reference.Name + ": " + _projectSystem.ReferenceExists(reference.Name));
			}
			return package.AssemblyReferences.All(reference => _projectSystem.ReferenceExists(reference.Name)); 
		}
	}

}