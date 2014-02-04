using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.Remotes.Push {
	public class PushPolicy: IRetrievePolicy, IMenuItemSource {
		public bool Matches(RepositoryInfo info, string approot) {
			var path = FileSystem.Combine(approot, info.Path, ".git");
			return Directory.Exists(path);
		}

		public MenuItem GetMenuItem() {
			return new PushMenuItem();
		}
	}
}