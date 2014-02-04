using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Editor.Menu;
using FubuCore;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.RepositoryManagement.Git {
	public class GitEditorMenuPolicy : VersionControlEditorMenuPolicy {
		public override bool Matches(RepositoryInfo info, string approot) {
			var path = FubuCore.FileSystem.Combine(approot, info.Path, ".git");
			return Directory.Exists(path);
		}


	}
}