using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.Remotes.Git.Init {
	public class GitInitPolicy: IRetrievePolicy, IMenuItemSource {
		public bool Matches(RepositoryInfo info, string approot) {
			//match when we don't have a .git folder
			var path = FileSystem.Combine(approot, info.Path, ".git");
			return !Directory.Exists(path);
		}

		public MenuItem GetMenuItem() {
			return new MenuItem(){Caption = "Git init", Id = "gitinit"};
		}
	}
}