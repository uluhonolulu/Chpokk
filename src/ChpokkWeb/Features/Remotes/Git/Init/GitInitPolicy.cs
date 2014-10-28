using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using ChpokkWeb.Infrastructure.Menu;
using FubuCore;

namespace ChpokkWeb.Features.Remotes.Git.Init {
	public class GitInitPolicy: IRetrievePolicy, IMenuItemSource {
		public bool Matches(string repositoryRoot) {
			//match when we don't have a .git folder
			var path = FileSystem.Combine(repositoryRoot, ".git");
			return !Directory.Exists(path);
		}

		public MenuItem GetMenuItem(string repositoryRoot) {
			return new MenuItem(){Caption = "Git init", Id = "gitinit"};
		}
	}
}