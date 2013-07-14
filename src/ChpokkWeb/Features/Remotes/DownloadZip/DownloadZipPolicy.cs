using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Remotes.DownloadZip {
	public class DownloadZipPolicy: IRetrievePolicy, IMenuItemSource {
		public bool Matches(RepositoryInfo info, string approot) {
			return true;
		}

		public MenuItem GetMenuItem() {
			return new DownloadZipMenuItem();
		}
	}
}