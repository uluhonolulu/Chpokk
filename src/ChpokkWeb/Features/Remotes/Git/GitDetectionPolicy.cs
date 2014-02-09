using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FubuCore;

namespace ChpokkWeb.Features.Remotes.Git {
	public class GitDetectionPolicy : IVersionControlDetectionPolicy {
		public bool Matches(string repositoryPath) {
			var path = repositoryPath.AppendPath(".git");
			return Directory.Exists(path);
		}
	}
}