using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.References.Files {
	public class UploadAssemblyEndpoint {
		public void DoIt(UploadAssemblyInputModel model) {
			if (model.Assembly == null) {
				return;
			}
			model.Assembly.SaveAs(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\sim1".AppendPath(model.Assembly.FileName));
		}
	}

	public class UploadAssemblyInputModel {
		public HttpPostedFileBase Assembly { get; set; }
	}
}