using FubuMVC.Core;

namespace ChpokkWeb.Features.Remotes.DownloadZip {
	[UrlPattern("RepositoryName={RepositoryName}")]
	public class DownloadZipInputModel {
		public string RepositoryName { get; set; }
		public string PhysicalApplicationPath { get; set; }
	}
}