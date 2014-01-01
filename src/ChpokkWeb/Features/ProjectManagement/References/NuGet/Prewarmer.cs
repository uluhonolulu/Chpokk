using System.Threading;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class Prewarmer {
		private readonly PackageFinder _packageFinder;
		public Prewarmer(PackageFinder packageFinder) {
			_packageFinder = packageFinder;
		}

		public void PrewarmAsync() {
			new Thread(this.PrewarmSync).Start();
		}

		private void PrewarmSync() {
			_packageFinder.FindPackages("elmah");
		}
	}
}