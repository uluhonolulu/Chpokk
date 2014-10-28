using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using ChpokkWeb.Infrastructure.Menu;

namespace ChpokkWeb.Features.Remotes.DownloadZip {
	public class DownloadZipPolicy: IRetrievePolicy, IMenuItemSource {
		public bool Matches(string repositoryRoot) {
			return true;
		}

		public MenuItem GetMenuItem(string repositoryRoot) {
			return new DownloadZipMenuItem();
		}
	}
}