using System.Web;

namespace ChpokkWeb.Features.Remotes.UploadZip {
	public class UploadZipInputModel {
		public HttpPostedFileBase ZippedRepository { get; set; }
		public string PhysicalApplicationPath { get; set; }
	}
}