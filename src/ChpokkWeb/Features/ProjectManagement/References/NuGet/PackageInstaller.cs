using ChpokkWeb.Features.Exploring;
using FubuCore;
using Microsoft.Build.Construction;
using NuGet;
using NuGet.Commands;
using NuGet.Common;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class PackageInstaller {
		private readonly IPackageRepository _packageRepository;
		public PackageInstaller(IPackageRepository packageRepository) {
			_packageRepository = packageRepository;
		}


		public void InstallPackage(string packageId, string projectPath, string targetFolder = null) {
			const string packagesFolder = "packages";
			if (targetFolder == null) 
				targetFolder = projectPath.ParentDirectory().ParentDirectory().AppendPath(packagesFolder);
			var packagePathResolver = new DefaultPackagePathResolver(targetFolder);
			var localRepository = new LocalPackageRepository(packagePathResolver, new PhysicalFileSystem(targetFolder));
			var projectSystem = new BetterThanMSBuildProjectSystem(projectPath);
			var projectManager = new ProjectManager(_packageRepository, packagePathResolver, projectSystem,
													localRepository);

			projectManager.AddPackageReference(packageId);
			projectSystem.Save();
		}
	}

	public class BetterThanMSBuildProjectSystem : MSBuildProjectSystem {

		public override void AddFile(string path, System.IO.Stream stream) {
			base.AddFile(path, stream);
			var rootElement = ProjectRootElement.Open(ProjectPath);
			rootElement.AddItem("Content", path);
		}

		public string ProjectPath { get; private set; }

		public BetterThanMSBuildProjectSystem(string projectFile) : base(projectFile) {
			ProjectPath = projectFile;
		}
	}
}